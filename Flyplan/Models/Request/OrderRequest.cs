using System;
using System.Collections.Generic;

namespace FlyPlan.Api.Models.Request
{
    public class OrderRequest
    {
        public Guid Id { get; set; }
        public  string Code { get; set; }
        public Guid FlightId { get; set; }
        public List<Guid> TravellerIds { get; set; }
        public Guid PaymentId { get; set; }
        public Guid ConfirmationId { get; set; }
    }
}
