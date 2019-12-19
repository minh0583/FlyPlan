using System;
using System.Collections.Generic;

namespace FlyPlan.Api.Models.Request
{
    public class OrderViewModel
    {
        public  string Code { get; set; }
        public FlightViewModel FlightViewModel { get; set; }
        public List<TravellerViewModel> Travellers { get; set; }
        public PaymentViewModel PaymentViewModel { get; set; }
        public ConfirmationInfoViewModel ConfirmationInfoViewModel { get; set; }
    }
}
