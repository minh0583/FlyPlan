using System;
using System.Collections.Generic;
using FlyPlan.Api.Models.Response;

namespace FlyPlan.Api.Models.Request
{
    public class FulfillBookingRequest
    {
        public Guid FlightId { get; set; }
        public List<TravellerViewModel> TravellerViewModels { get; set; }
        public PaymentViewModel PaymentViewModel { get; set; }
        public ConfirmationInfoViewModel ConfirmationInfoViewModel { get; set; }
    }
}
