using System;
using System.Collections.Generic;
using System.Dynamic;
using AutoMapper;
using FlyPlan.Api.Controllers;
using FlyPlan.Api.Data;
using FlyPlan.Api.Mapper;
using FlyPlan.Api.Models.Criteria;
using FlyPlan.Api.Models.Request;
using FlyPlan.Api.Models.Response;
using FlyPlan.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FlyPlan.Api.UnitTests
{
    public class FlyPlanControllerUnitTest
    {
        private FlyplanContext _dbContext;

        [Fact]
        public void GetFlight()
        {
            var controller = InitialFlyPlanController(nameof(GetFlight));
            var response = controller.GetFlight(Guid.Parse("86FCB407-4EDF-C220-4B12-0002FD2BB55E")) as ObjectResult;
            var value = response?.Value as SingleResponse<Flight>;

            _dbContext.Dispose();

            // Assert
            Assert.False(value?.DidError);
        }

        [Fact]
        public void TestGetAllFlights()
        {
            var controller = InitialFlyPlanController(nameof(TestGetAllFlights));
            var response = controller.GetFlights(new SearchFlight()) as ObjectResult;
            var value = response?.Value as ListResponse<Flight>;

            _dbContext.Dispose();

            // Assert
            Assert.False(value?.DidError);
        }

        [Fact]
        public void TestGetFlightsByDateRange()
        {
            var controller = InitialFlyPlanController(nameof(TestGetFlightsByDateRange));
            var response = controller.GetFlights(new SearchFlight
            {
                DepartDate = Convert.ToDateTime("01/14/2020"),
                ReturnDate = Convert.ToDateTime("01/15/2020")

            }) as ObjectResult;
            var value = response?.Value as ListResponse<Flight>;

            _dbContext.Dispose();

            // Assert
            Assert.True(value?.TotalRecord == 1);
        }

        [Fact]
        public void TestCreateOrderAsync()
        {
            var controller = InitialFlyPlanController(nameof(TestCreateOrderAsync));
            var response = controller.CreateOrderAsync(new BookingRequest
            {
                ConfirmationId = Guid.Parse("198EAEA2-9578-CE16-4CF6-000BBBD22AF3"),
                FlightId = Guid.Parse("86FCB407-4EDF-C220-4B12-0002FD2BB55E"),
                PaymentId = Guid.Parse("DC74E778-00E3-749A-56E3-001D7EAD42D0"),
                TravellerIds = new List<Guid>
                {
                    Guid.Parse("CEB33942-5E21-2937-A603-002F4CA237A0"),
                    Guid.Parse("478FE9F6-51F6-455D-3769-00403F39F130"),
                    Guid.Parse("D6DB9A51-111D-B7C8-910B-00876A9F2291")
                }
            }).Result as ObjectResult;


            var value = response?.Value as SingleResponse<ExpandoObject>;

            _dbContext.Dispose();

            // Assert
            Assert.False(value?.DidError);
        }

        [Fact]
        public void TestGetOrderDetail()
        {
            var controller = InitialFlyPlanController(nameof(TestGetOrderDetail));
            var response = controller.GetOrderDetail("CDJSDI") as ObjectResult;

            var value = response?.Value as SingleResponse<BookingViewModelResponse>;

            _dbContext.Dispose();

            // Assert
            Assert.False(value?.DidError);
        }

        [Fact]
        public void GetAllOrders()
        {
            var controller = InitialFlyPlanController(nameof(GetAllOrders));
            var response = controller.GetAllOrders() as ObjectResult;

            var value = response?.Value as ListResponse<BookingViewModelResponse>;

            _dbContext.Dispose();

            // Assert
            Assert.False(value?.DidError);
        }

        private FlyPlanController InitialFlyPlanController(string action)
        {
            _dbContext = FlyplanDbContextMocker.GetFlyplanContext(action);
            var mockMapper = new MapperConfiguration(cfg => { cfg.AddProfile(new MapperProfile()); });
            var mapper = mockMapper.CreateMapper();
            var controller = new FlyPlanController(null, _dbContext, mapper);
            return controller;
        }
    }
}
