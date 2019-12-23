using System;
using FlyPlan.Data.Models;

namespace FlyPlan.Api.UnitTests
{
    public static class DbContextExtensions
    {
        public static void Seed(this FlyplanContext dbContext)
        {
            // Add entities for DbContext instance
            var flight1 = new Flight
            {
                Id = Guid.Parse("86FCB407-4EDF-C220-4B12-0002FD2BB55E"),
                RoundTrip = 0,
                Depart = "Shanghai",
                DepartTime = "Fri Dec 27 2019 17:34:05",
                DepartAirport = "Tokyo TK, Japan",
                Return = "Seoul",
                ReturnTime = "Sat Dec 28 2019 17:34:05",
                ReturnAirport = "Moscow MC, Rusia",
                TotalTime = "a day",
                ClassType = "Business Class",
                DepartAirlinePicture = "http://placehold.it/30x30",
                DepartAirlineName = "Singapore Airlines",
                DepartAirlinePlane = "CL-600-2D24",
                ReturnAirlinePicture = "http://placehold.it/30x30",
                ReturnAirlineName = "Singapore Airlines",
                ReturnAirlinePlane = "EMB-145XR",
                TotalMoney = 423,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            var flight2 = new Flight
            {
                Id = Guid.Parse("9E5072D8-8420-C137-297B-0003C457D86A"),
                RoundTrip = 0,
                Depart = "Mexico City",
                DepartTime = "Mon Dec 23 2019 00:25:00",
                DepartAirport = "New York City NY, USA",
                Return = "Tokyo",
                ReturnTime = "Thu Jan 16 2020 13:34:45",
                TotalTime = "a day",
                ClassType = "Business Class",
                DepartAirlinePicture = "http://placehold.it/30x30",
                DepartAirlineName = "Vietnam Airlines",
                DepartAirlinePlane = "CL-600-2D24",
                TotalMoney = 423,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            var flight3 = new Flight
            {
                Id = Guid.Parse("69DAC8FE-3C2A-AB18-95FF-0028D9E555BE"),
                RoundTrip = 1,
                Depart = "Beijing",
                DepartTime = "Sat Dec 21 2019 01:25:33",
                DepartAirport = "Los Angeles LA, USA",
                Return = "Beijing",
                ReturnTime = "Wed Jan 15 2020 15:27:10",
                ReturnAirport = "Singapore SG, Singapore",
                TotalTime = "5 days",
                ClassType = "Business Class",
                DepartAirlinePicture = "https://www.logolynx.com/images/logolynx/99/9987227f610e705459709216ce49dce3.jpeg",
                DepartAirlineName = "China Airlines",
                DepartAirlinePlane = "A321-211",
                ReturnAirlinePicture = "https://www.chatbotkorea.com/img/bot/logo/1501161171c210a09e200b3cbb3d1f33efc219f8da",
                ReturnAirlineName = "Austrian Airlines",
                ReturnAirlinePlane = "757-224",
                TotalMoney = 555,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            dbContext.Flight.Add(flight1);
            dbContext.Flight.Add(flight2);
            dbContext.Flight.Add(flight3);

            var confirmation = new ConfirmationInfo
            {
                Id = Guid.Parse("198EAEA2-9578-CE16-4CF6-000BBBD22AF3"),
                EmailAddress = "tgcj3@mlbn.zhsfmb.org",
                PhoneNumber = "691-6639473",
                IsAcceptedRule = true,
                IsSendEmail = true,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
            dbContext.ConfirmationInfo.Add(confirmation);

            var payment = new Payment
            {
                Id = Guid.Parse("DC74E778-00E3-749A-56E3-001D7EAD42D0"),
                CreditCardType = "VISA",
                CardNumber = "50766",
                NameOnTheCard = "Chad8",
                ExpiryDateInMonth = "03/01",
                ExpiryDateInYear = "12/05",
                Cvvcode = "0487",
                CountryId = "Belarus",
                BillingAddress = "219 Second Avenue",
                City = "New Orleans",
                Zipcode = "10497",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
            dbContext.Payment.Add(payment);

            var travellerId1 = Guid.Parse("CEB33942-5E21-2937-A603-002F4CA237A0");
            dbContext.Traveller.Add(new Traveller
            {
                Id = travellerId1,
                PersonType = "Infants",
                FirstName = "Lewis",
                LastName = "Reed",
                DateOfBirth = Convert.ToDateTime("1955-08-03"),
                Gender = 2,
                Nationality = 877807523,
                PasportId = "12974",
                PasportExpiryDateMonth = "07/09",
                PasportExpiryDateYear = "10/14",
                PasportNoExpiry = false,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            });

            var travellerId2 = Guid.Parse("478FE9F6-51F6-455D-3769-00403F39F130");
            dbContext.Traveller.Add(new Traveller
            {
                Id = travellerId2,
                PersonType = "Children",
                FirstName = "Herbert",
                LastName = "Pineda",
                DateOfBirth = Convert.ToDateTime("1959-03-29"),
                Gender = 2,
                Nationality = 1888668374,
                PasportId = "10001",
                PasportExpiryDateMonth = "08/09",
                PasportExpiryDateYear = "04/10",
                PasportNoExpiry = false,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            });

            var travellerId3 = Guid.Parse("D6DB9A51-111D-B7C8-910B-00876A9F2291");
            dbContext.Traveller.Add(new Traveller
            {
                Id = travellerId3,
                PersonType = "Infants",
                FirstName = "Sherry",
                LastName = "Porter",
                DateOfBirth = Convert.ToDateTime("1978-09-02"),
                Gender = 3,
                Nationality = 61963030,
                PasportId = "61343",
                PasportExpiryDateMonth = "03/07",
                PasportExpiryDateYear = "12/13",
                PasportNoExpiry = false,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            });

            var orderId = Guid.Parse("FBF76089-F220-409C-A3AA-454E584999E6");
            dbContext.Order.Add(new Order
            {
                Id = orderId,
                Code = "CDJSDI",
                Payment = payment,
                Confirmation = confirmation,
                Flight = flight1,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            });

            dbContext.TravellerOrder.Add(new TravellerOrder
            {
                TravellerId = travellerId1,
                OrderId = orderId
            });

            dbContext.TravellerOrder.Add(new TravellerOrder
            {
                TravellerId = travellerId2,
                OrderId = orderId
            });

            dbContext.TravellerOrder.Add(new TravellerOrder
            {
                TravellerId = travellerId3,
                OrderId = orderId
            });

            dbContext.SaveChanges();
        }
    }
}
