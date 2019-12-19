using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlyPlan.Api.Classes;
using FlyPlan.Api.Data;
using FlyPlan.Api.Models.Response;
using FlyPlan.Api.Models.Criteria;
using FlyPlan.Api.Models.Request;
using FlyPlan.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlyPlan.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class FlyPlanController : ControllerBase
    {
        protected readonly ILogger Logger;
        protected readonly FlyplanContext DbContext;
        protected readonly IMapper Mapper;

        public FlyPlanController(ILogger<FlyPlanController> logger, FlyplanContext dbContext, IMapper mapper)
        {
            Logger = logger;
            DbContext = dbContext;
            Mapper = mapper;
        }

        [HttpPost("flight")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetFlights([FromBody] SearchFlight searchFlightCriteria)
        {
            Logger?.LogDebug("'{0}' has been invoked", nameof(GetFlights));

            var response = new ListResponse<Flight>();

            try
            {
                if (searchFlightCriteria.GetType()
                    .GetProperties()
                    .Select(p => p.GetValue(searchFlightCriteria))
                    .Any(p => p != null)
                )
                {
                    var flightDetail = DbContext.Flight.Where(p =>
                        (string.IsNullOrEmpty(searchFlightCriteria.From) || p.Depart == searchFlightCriteria.From) &&
                        (string.IsNullOrEmpty(searchFlightCriteria.To) || p.Return == searchFlightCriteria.To) &&
                        (searchFlightCriteria.DepartDate == null || p.DepartTime.ToDateTime() >= searchFlightCriteria.DepartDate.Value.ChangeTime(0,0,0,0)) &&
                        (searchFlightCriteria.ReturnDate == null || p.ReturnTime.ToDateTime() <= searchFlightCriteria.ReturnDate.Value.ChangeTime(23,59,59,0)) &&
                        //searchFlightCriteria.Adults 
                        //searchFlightCriteria.Children 
                        //searchFlightCriteria.Infants
                        (string.IsNullOrEmpty(searchFlightCriteria.ClassType) || p.ClassType.Contains(searchFlightCriteria.ClassType)) &&
                        (searchFlightCriteria.RoundTrip == null || p.RoundTrip == searchFlightCriteria.RoundTrip) &&
                        (searchFlightCriteria.PriceFrom == null || p.TotalMoney >= searchFlightCriteria.PriceFrom) &&
                        (searchFlightCriteria.PriceTo == null || p.TotalMoney <= searchFlightCriteria.PriceTo) &&
                        (string.IsNullOrEmpty(searchFlightCriteria.DepartTime) || p.DepartTime == searchFlightCriteria.DepartTime) &&
                        (string.IsNullOrEmpty(searchFlightCriteria.Airlines) || p.DepartAirlineName == searchFlightCriteria.Airlines) &&
                        (string.IsNullOrEmpty(searchFlightCriteria.Airlines) || p.ReturnAirlineName == searchFlightCriteria.Airlines)
                    ).ToList();

                    response.Model = flightDetail;
                }
                else
                {
                    response.Model = DbContext.Flight.ToList();
                }
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                Logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(GetFlights), ex);
            }

            return response.ToHttpResponse();
        }

        [HttpPost("order")]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderRequest orderRequest)
        {
            Logger?.LogDebug("'{0}' has been invoked", nameof(CreateOrderAsync));

            var response = new SingleResponse<ExpandoObject>();

            try
            {
                if (DbContext.Flight.FirstOrDefault(p => p.Id == orderRequest.FlightId) == null)
                {
                    response.ErrorMessage = $"Flight with {orderRequest.FlightId} is not found";
                    return response.ToHttpResponse();
                }

                if (DbContext.Payment.FirstOrDefault(p => p.Id == orderRequest.PaymentId) == null)
                {
                    response.ErrorMessage = $"Payment with {orderRequest.PaymentId} is not found";
                    return response.ToHttpResponse();
                }

                if (DbContext.ConfirmationInfo.FirstOrDefault(p => p.Id == orderRequest.ConfirmationId) == null)
                {
                    response.ErrorMessage = $"Confirmation with {orderRequest.ConfirmationId} is not found";
                    return response.ToHttpResponse();
                }

                if (DbContext.Traveller.Count(p => orderRequest.TravellerIds.Contains(p.Id)) != orderRequest.TravellerIds.Count)
                {
                    response.ErrorMessage = "Travellers are not found";
                    return response.ToHttpResponse();
                }

                var order = Mapper.Map<Order>(orderRequest);
                order.Code = Utils.GenerateReservationCode(6);
                order.Id = Guid.NewGuid();

                DbContext.Order.Add(order);

                foreach (var travellerId in orderRequest.TravellerIds)
                {
                    DbContext.TravellerOrder.Add(new TravellerOrder
                    {
                        OrderId = order.Id,
                        TravellerId = travellerId
                    });
                }

                await DbContext.SaveChangesAsync();

                dynamic returnModel = new ExpandoObject();
                returnModel.OrderId = order.Id;
                returnModel.Code = order.Code;

                response.Model = returnModel;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                Logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(CreateOrderAsync), ex);
            }

            return response.ToHttpResponse();
        }

        [HttpGet("order/{code}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetOrderDetail(string code)
        {
            Logger?.LogDebug("'{0}' has been invoked", nameof(GetOrderDetail));

            var response = new SingleResponse<OrderViewModel>();

            try
            {
                var order = DbContext.Order
                    .Include(p => p.Confirmation)
                    .Include(p => p.Flight)
                    .Include(p => p.Payment)
                    .FirstOrDefault(p => p.Code == code);

                if (order != null)
                {
                    var travelOrders = DbContext.TravellerOrder
                        .Include(p => p.Traveller)
                        .Where(p => p.OrderId == order.Id)
                        .Select(p => p.Traveller);

                    var orderViewModel = Mapper.Map<OrderViewModel>(order);
                    orderViewModel.Travellers = Mapper.Map<List<TravellerViewModel>>(travelOrders);

                    response.Model = orderViewModel;
                }
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                Logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(GetFlights), ex);
            }

            return response.ToHttpResponse();
        }

        [HttpGet("order")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetAllOrders()
        {
            Logger?.LogDebug("'{0}' has been invoked", nameof(GetAllOrders));

            var response = new ListResponse<OrderViewModel>();

            try
            {
                var orders = DbContext.Order
                    .Include(p => p.Confirmation)
                    .Include(p => p.Flight)
                    .Include(p => p.Payment)
                    .ToList();

                var travelOrders = DbContext.TravellerOrder
                    .Include(p => p.Traveller)
                    .Where(p => orders.Select(o => o.Id).Contains(p.OrderId))
                    .Select(p => new { p.OrderId, p.Traveller })
                    .ToList();


                var orderViewModels = Mapper.Map<List<OrderViewModel>>(orders);

                foreach (var orderViewModel in orderViewModels)
                {
                    var travellers = travelOrders.Where(p => p.OrderId == orderViewModel.Id).Select(p => p.Traveller);

                    orderViewModel.Travellers = Mapper.Map<List<TravellerViewModel>>(travellers);
                }

                response.Model = orderViewModels;

            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                Logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(GetFlights), ex);
            }

            return response.ToHttpResponse();
        }
    }
}
