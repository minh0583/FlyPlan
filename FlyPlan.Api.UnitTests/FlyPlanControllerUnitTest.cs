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
        [Fact]
        public void TestGetFlights()
        {
            // Arrange
            var dbContext = FlyplanDbContextMocker.GetFlyplanContext(nameof(TestGetFlights));
            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperProfile());
            });
            var mapper = mockMapper.CreateMapper();
            var controller = new FlyPlanController(null, dbContext, mapper);

            // Act
            var response = controller.GetFlights(new SearchFlight()) as ObjectResult;
            var value = response.Value as ListResponse<Flight>;

            dbContext.Dispose();

            // Assert
            Assert.False(value.DidError);
        }

        [Fact]
        public void TestCreateOrderAsync()
        {
            // Arrange
            var dbContext = FlyplanDbContextMocker.GetFlyplanContext(nameof(TestCreateOrderAsync));
            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperProfile());
            });
            var mapper = mockMapper.CreateMapper();
            var controller = new FlyPlanController(null, dbContext, mapper);

            // Act
            var response = controller.CreateOrderAsync(new OrderRequest
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


            var value = response.Value as SingleResponse<ExpandoObject>;

            dbContext.Dispose();

            // Assert
            Assert.False(value.DidError);
        }

        [Fact]
        public void TestGetOrderDetail()
        {
            // Arrange
            var dbContext = FlyplanDbContextMocker.GetFlyplanContext(nameof(TestGetOrderDetail));
            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperProfile());
            });
            var mapper = mockMapper.CreateMapper();
            var controller = new FlyPlanController(null, dbContext, mapper);

            // Act
            var response = controller.GetOrderDetail("CDJSDI") as ObjectResult;

            var value = response.Value as SingleResponse<OrderViewModel>;

            dbContext.Dispose();

            // Assert
            Assert.False(value.DidError);
        }

        [Fact]
        public void GetAllOrders()
        {
            // Arrange
            var dbContext = FlyplanDbContextMocker.GetFlyplanContext(nameof(GetAllOrders));
            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperProfile());
            });
            var mapper = mockMapper.CreateMapper();
            var controller = new FlyPlanController(null, dbContext, mapper);

            // Act
            var response = controller.GetAllOrders() as ObjectResult;

            var value = response.Value as ListResponse<OrderViewModel>;

            dbContext.Dispose();

            // Assert
            Assert.False(value.DidError);
        }
    }
}
