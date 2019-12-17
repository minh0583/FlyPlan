using System;
using System.Collections.Generic;

namespace FlyPlan.Data.Models
{
    public partial class Order
    {
        public Order()
        {
            TravellerOrder = new HashSet<TravellerOrder>();
        }

        public Guid Id { get; set; }
        public string Code { get; set; }
        public Guid? FlightId { get; set; }
        public Guid? PaymentId { get; set; }
        public Guid? ConfirmationId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public ConfirmationInfo Confirmation { get; set; }
        public Flight Flight { get; set; }
        public Payment Payment { get; set; }
        public ICollection<TravellerOrder> TravellerOrder { get; set; }
    }
}
