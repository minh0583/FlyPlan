using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using AutoMapper;
using FlyPlan.Api.Classes;
using FlyPlan.Api.Controllers;
using FlyPlan.Api.Data;
using FlyPlan.Api.Mapper;
using FlyPlan.Api.Models.Criteria;
using FlyPlan.Api.Models.Request;
using FlyPlan.Api.Models.Response;
using FlyPlan.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Xunit;

namespace FlyPlan.Api.UnitTests
{
    public class FlyPlanControllerUnitTest
    {
        private FlyplanContext _dbContext;

        private FulfillBookingRequest initialData = new FulfillBookingRequest
        {
            FlightId = Guid.Parse("86FCB407-4EDF-C220-4B12-0002FD2BB55E"),
            PaymentViewModel = new PaymentViewModel
            {
                CreditCardType = "VISA",
                CardNumber = "50766",
                NameOnTheCard = "Chad8",
                ExpiryDateInMonth = "03/01",
                ExpiryDateInYear = "12/05",
                CVVCode = "0487",
                CountryId = "Belarus",
                BillingAddress = "219 Second Avenue",
                City = "New Orleans",
                ZIPCode = "10497"
            },
            ConfirmationInfoViewModel = new ConfirmationInfoViewModel
            {
                EmailAddress = "tgcj3@mlbn.zhsfmb.org",
                PhoneNumber = "691-6639473",
                IsAcceptedRule = true,
                IsSendEmail = true
            },
            TravellerViewModels = new List<TravellerViewModel>
            {
                new TravellerViewModel
                {
                    PersonType = "Infants",
                    FirstName = "Lewis",
                    LastName = "Reed",
                    DateOfBirth = Convert.ToDateTime("1955-08-03"),
                    Gender = 2,
                    Nationality = 877807523,
                    PasportId = "12974",
                    PasportExpiryDateMonth = "07/09",
                    PasportExpiryDateYear = "10/14",
                    PasportNoExpiry = false
                },
                new TravellerViewModel
                {
                    PersonType = "Children",
                    FirstName = "Herbert",
                    LastName = "Pineda",
                    DateOfBirth = Convert.ToDateTime("1959-03-29"),
                    Gender = 2,
                    Nationality = 1888668374,
                    PasportId = "10001",
                    PasportExpiryDateMonth = "08/09",
                    PasportExpiryDateYear = "04/10",
                    PasportNoExpiry = false
                },
                new TravellerViewModel
                {
                    PersonType = "Infants",
                    FirstName = "Sherry",
                    LastName = "Porter",
                    DateOfBirth = Convert.ToDateTime("1978-09-02"),
                    Gender = 3,
                    Nationality = 61963030,
                    PasportId = "61343",
                    PasportExpiryDateMonth = "03/07",
                    PasportExpiryDateYear = "12/13",
                    PasportNoExpiry = false
                }
            }
        };

        [Fact]
        public void GetAllFlights()
        {
            var controller = InitialFlyPlanController(nameof(GetAllFlights));
            var response = controller.GetAllFlights() as ObjectResult;
            var value = response?.Value as ListResponse<Flight>;

            _dbContext.Dispose();

            // Assert
            Assert.False(value?.DidError);
        }

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
                DepartDate = Convert.ToDateTime("12/27/2019"),
                ReturnDate = Convert.ToDateTime("12/28/2019"),
                Airlines = new List<string> { "Singapore Airlines", "Vietnam Airlines" }

            }) as ObjectResult;
            var value = response?.Value as ListResponse<Flight>;

            _dbContext.Dispose();

            // Assert
            Assert.True(value?.TotalRecord == 1);
        }

        [Fact]
        public void TestGetFlightsByAirlines()
        {
            var controller = InitialFlyPlanController(nameof(TestGetFlightsByAirlines));
            var response = controller.GetFlights(new SearchFlight
            {
                Airlines = new List<string> { "Singapore Airlines", "Vietnam Airlines", "Austrian Airlines" }

            }) as ObjectResult;
            var value = response?.Value as ListResponse<Flight>;

            _dbContext.Dispose();

            // Assert
            Assert.True(value?.TotalRecord == 3);
        }

        [Fact]
        public void TestGetFlightsAndOrderyByDepartTime()
        {
            var controller = InitialFlyPlanController(nameof(TestGetFlightsAndOrderyByDepartTime));
            var response = controller.GetFlights(new SearchFlight
            {
                OrderBy = OrderByEnum.DepartTime

            }) as ObjectResult;
            var value = response?.Value as ListResponse<Flight>;

            _dbContext.Dispose();

            // Assert
            Assert.True(value?.TotalRecord == 3);
            Assert.True(value?.Model.ElementAt(0).Depart == "Beijing");
            Assert.True(value?.Model.ElementAt(1).Depart == "Mexico City");
            Assert.True(value?.Model.ElementAt(2).Depart == "Shanghai");
        }

        [Fact]
        public void CreateFulfillOrderAsync()
        {
            var controller = InitialFlyPlanController(nameof(CreateFulfillOrderAsync));
            var response = controller.CreateFulfillOrderAsync(initialData).Result as ObjectResult;

            var value = response?.Value as SingleResponse<ExpandoObject>;
            var data = value?.Model as IDictionary<string, object>;

            if (data != null)
            {
                var code = data["Code"].ToString();
                var bookingId = Guid.Parse(data["BookingId"].ToString());

                var order = _dbContext.Order.FirstOrDefault(p => p.Code == code && p.Id == bookingId);

                Assert.Equal(code, order.Code);
                Assert.Equal(bookingId, order.Id);
            }

            _dbContext.Dispose();
        }

        [Fact]
        public void Test400BadRequest_CreateFulfillOrderAsync()
        {
            initialData.FlightId = Guid.NewGuid();
            var controller = InitialFlyPlanController(nameof(Test400BadRequest_CreateFulfillOrderAsync));
            var response = controller.CreateFulfillOrderAsync(initialData).Result as ObjectResult;

            var value = response?.Value as SingleResponse<ExpandoObject>;

            _dbContext.Dispose();

            if (value != null)
            {
                Assert.True(value.ErrorMessage.Contains(initialData.FlightId.ToString()));
                Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        [Fact]
        public void Test400BadRequestMissingRequiredObject_CreateFulfillOrderAsync()
        {
            initialData.FlightId = Guid.NewGuid();
            initialData.ConfirmationInfoViewModel = null;
            initialData.PaymentViewModel = null;
            initialData.TravellerViewModels = null;
            var controller = InitialFlyPlanController(nameof(Test400BadRequestMissingRequiredObject_CreateFulfillOrderAsync));
            var response = controller.CreateFulfillOrderAsync(initialData).Result as ObjectResult;

            var value = response?.Value as SingleResponse<ExpandoObject>;

            _dbContext.Dispose();

            if (value != null)
            {
                Assert.True(value.ErrorMessage.Contains(initialData.FlightId.ToString()));
                Assert.True(value.ErrorMessage.Contains("Confirmation is missing"));
                Assert.True(value.ErrorMessage.Contains("Payment is missing"));
                Assert.True(value.ErrorMessage.Contains("Traveller is missing"));
                Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        [Fact]
        public void Test400BadRequestMissingRequiredField_CreateFulfillOrderAsync()
        {
            initialData.ConfirmationInfoViewModel.EmailAddress = " ";
            initialData.ConfirmationInfoViewModel.PhoneNumber = "";
            initialData.TravellerViewModels[0].DateOfBirth = null;

            var controller = InitialFlyPlanController(nameof(Test400BadRequestMissingRequiredField_CreateFulfillOrderAsync));
            var response = controller.CreateFulfillOrderAsync(initialData).Result as ObjectResult;

            var value = response?.Value as SingleResponse<ExpandoObject>;

            _dbContext.Dispose();

            if (value != null)
            {
                Assert.True(value.ErrorMessage.Contains("Confirmation.EMAILADDRESS is required"));
                Assert.True(value.ErrorMessage.Contains("Confirmation.PHONENUMBER is required"));
                Assert.True(value.ErrorMessage.Contains("Traveller.DATEOFBIRTH is required"));
                Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
            }
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
