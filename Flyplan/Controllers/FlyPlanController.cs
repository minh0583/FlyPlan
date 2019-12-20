using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
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

        [HttpGet("flight")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetAllFlights()
        {
            Logger?.LogDebug("'{0}' has been invoked", nameof(GetAllFlights));

            var response = new ListResponse<Flight>();

            try
            {
                var flightDetails = DbContext.Flight.ToList();

                response.Model = flightDetails;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                Logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(GetFlights), ex);
            }

            return response.ToHttpResponse();
        }

        [HttpGet("flight/{Id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetFlight(Guid Id)
        {
            Logger?.LogDebug("'{0}' has been invoked", nameof(GetFlight));

            var response = new SingleResponse<Flight>();

            try
            {
                var flightDetail = DbContext.Flight.FirstOrDefault(p => p.Id == Id);

                response.Model = flightDetail;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;

                Logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(GetFlights), ex);
            }

            return response.ToHttpResponse();
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
                        (searchFlightCriteria.DepartDate == null || p.DepartTime.ToDateTime() == searchFlightCriteria.DepartDate.Value.Date) &&
                        (searchFlightCriteria.ReturnDate == null || p.ReturnTime.ToDateTime() == searchFlightCriteria.ReturnDate.Value.Date) &&
                        (string.IsNullOrEmpty(searchFlightCriteria.ClassType) || p.ClassType.Contains(searchFlightCriteria.ClassType)) &&
                        (searchFlightCriteria.RoundTrip == null || p.RoundTrip == searchFlightCriteria.RoundTrip) &&
                        (searchFlightCriteria.PriceFrom == null || p.TotalMoney >= searchFlightCriteria.PriceFrom) &&
                        (searchFlightCriteria.PriceTo == null || p.TotalMoney <= searchFlightCriteria.PriceTo) &&
                        (string.IsNullOrEmpty(searchFlightCriteria.DepartTime) || p.DepartTime == searchFlightCriteria.DepartTime) &&
                        (string.IsNullOrEmpty(searchFlightCriteria.Airlines) || p.DepartAirlineName == searchFlightCriteria.Airlines || p.ReturnAirlineName == searchFlightCriteria.Airlines)
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


        [HttpPost("booking")]
        [ProducesResponseType(200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateFulfillOrderAsync([FromBody] FulfillBookingRequest fulfillOrderRequest)
        {
            Logger?.LogDebug("'{0}' has been invoked", nameof(CreateFulfillOrderAsync));

            var response = new SingleResponse<ExpandoObject>();

            try
            {
                var invalidData = ValidationData(fulfillOrderRequest);

                if (invalidData != null)
                {
                    return invalidData;
                }

                var confimrationInfo = Mapper.Map<ConfirmationInfo>(fulfillOrderRequest.ConfirmationInfoViewModel);
                var payment = Mapper.Map<Payment>(fulfillOrderRequest.PaymentViewModel);
                var travellers = Mapper.Map<List<Traveller>>(fulfillOrderRequest.TravellerViewModels);
                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    FlightId = fulfillOrderRequest.FlightId,
                    Code = Utils.GenerateReservationCode(6),
                    Payment = payment,
                    Confirmation = confimrationInfo
                };

                DbContext.Order.Add(order);
                DbContext.Traveller.AddRange(travellers);

                foreach (var traveller in travellers)
                {
                    DbContext.TravellerOrder.Add(new TravellerOrder
                    {
                        OrderId = order.Id,
                        TravellerId = traveller.Id
                    });
                }

                await DbContext.SaveChangesAsync();

                dynamic returnModel = new ExpandoObject();
                returnModel.BookingId = order.Id;
                returnModel.Code = order.Code;

                response.Model = returnModel;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                Logger?.LogCritical("There was an error on '{0}' invocation: {1}", nameof(CreateFulfillOrderAsync), ex);
            }

            return response.ToHttpResponse();
        }

        List<string> ignoreProperties = new List<string> { "TravelInsurance", "CheckedBaggae" };

        private IActionResult ValidationData(FulfillBookingRequest fulfillOrderRequest)
        {
            var errorMessages = new StringBuilder();
            var response = new SingleResponse<ExpandoObject>();

            if (DbContext.Flight.FirstOrDefault(p => p.Id == fulfillOrderRequest.FlightId) == null)
            {
                errorMessages.AppendLine($"Flight with {fulfillOrderRequest.FlightId} is not found.");
            }

            if (fulfillOrderRequest.ConfirmationInfoViewModel == null)
            {
                errorMessages.AppendLine("Confirmation is missing.");
            }
            else
            {
                foreach (var property in fulfillOrderRequest.ConfirmationInfoViewModel.GetProperties())
                {
                    errorMessages.AppendLine($"Confirmation.{property.Name.ToUpper()} is required.");
                }
            }

            if (fulfillOrderRequest.PaymentViewModel == null)
            {
                errorMessages.AppendLine("Payment is missing.");
            }
            else
            {
                foreach (var property in fulfillOrderRequest.PaymentViewModel.GetProperties())
                {
                    errorMessages.AppendLine($"Payment.{property.Name.ToUpper()} is required.");
                }
            }

            if (fulfillOrderRequest.TravellerViewModels == null || fulfillOrderRequest.TravellerViewModels.Count == 0)
            {
                errorMessages.AppendLine("Traveller is missing.");
            }
            else
            {
                var travellerErrors = new List<string>();
                foreach (var travellerViewModel in fulfillOrderRequest.TravellerViewModels)
                {
                    foreach (var property in travellerViewModel.GetProperties())
                    {
                        if (ignoreProperties.Contains(property.Name))
                            continue;

                        travellerErrors.Add($"Traveller.{property.Name.ToUpper()} is required.");
                    }
                }

                if (travellerErrors.Count > 0)
                {
                    foreach (var travellerError in travellerErrors.Distinct())
                    {
                        errorMessages.AppendLine(travellerError);
                    }
                }
            }

            if (errorMessages.Length == 0)
            {
                return null;
            }

            response.ErrorMessage = errorMessages.ToString();

            return response.ToHttpResponse();
        }

        [HttpGet("booking/{code}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetOrderDetail(string code)
        {
            Logger?.LogDebug("'{0}' has been invoked", nameof(GetOrderDetail));

            var response = new SingleResponse<BookingViewModelResponse>();

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

                    var orderViewModel = Mapper.Map<BookingViewModelResponse>(order);
                    orderViewModel.Travellers = Mapper.Map<List<TravellerViewModelResponse>>(travelOrders);

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

        [HttpGet("booking")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetAllOrders()
        {
            Logger?.LogDebug("'{0}' has been invoked", nameof(GetAllOrders));

            var response = new ListResponse<BookingViewModelResponse>();

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


                var orderViewModels = Mapper.Map<List<BookingViewModelResponse>>(orders);

                foreach (var orderViewModel in orderViewModels)
                {
                    var travellers = travelOrders.Where(p => p.OrderId == orderViewModel.Id).Select(p => p.Traveller);

                    orderViewModel.Travellers = Mapper.Map<List<TravellerViewModelResponse>>(travellers);
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
