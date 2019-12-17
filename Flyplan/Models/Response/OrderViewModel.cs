using System;
using System.Collections.Generic;

namespace FlyPlan.Api.Models.Response
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }
        public  string Code { get; set; }
        public FlightViewModel FlightViewModel { get; set; }
        public List<TravellerViewModel> Travellers { get; set; }
        public PaymentViewModel PaymentViewModel { get; set; }
        public ConfirmationInfoViewModel ConfirmationInfoViewModel { get; set; }
    }
}
