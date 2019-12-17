using System;
using System.Collections.Generic;

namespace FlyPlan.Data.Models
{
    public partial class Flight
    {
        public Flight()
        {
            Order = new HashSet<Order>();
        }

        public Guid Id { get; set; }
        public Guid? FlightDetailId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public ICollection<Order> Order { get; set; }
    }
}
