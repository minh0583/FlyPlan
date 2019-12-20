using System;
using System.Collections.Generic;

namespace FlyPlan.Api.Models.Request
{
    public class BookingRequest
    {
        public Guid FlightId { get; set; }
        public List<Guid> TravellerIds { get; set; }
        public Guid PaymentId { get; set; }
        public Guid ConfirmationId { get; set; }
    }
}
