using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
                var flightDetail = DbContext.Flight.Where(p =>
                    p.ClassType.Contains(searchFlightCriteria.ClassType) ||
                    p.DepartTime == searchFlightCriteria.DepartTime
                    ).ToList();

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
                var order = Mapper.Map<Order>(orderRequest);
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

        [HttpGet("order/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetOrderDetail(Guid Id)
        {
            Logger?.LogDebug("'{0}' has been invoked", nameof(GetOrderDetail));

            var response = new SingleResponse<OrderViewModel>();

            try
            {
                var order = DbContext.Order
                    .Include(p => p.Confirmation)
                    .Include(p => p.Flight)
                    .Include(p => p.Payment)
                    .FirstOrDefault(p => p.Id == Id);

                var travelOrders = DbContext.TravellerOrder
                    .Include(p => p.Traveller)
                    .Where(p => p.OrderId == Id)
                    .Select(p => p.Traveller);

                var orderViewModel = Mapper.Map<OrderViewModel>(order);
                orderViewModel.Travellers = Mapper.Map<List<TravellerViewModel>>(travelOrders);

                response.Model = orderViewModel;
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
