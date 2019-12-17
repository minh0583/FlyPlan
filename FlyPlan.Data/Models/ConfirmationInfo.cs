using System;
using System.Collections.Generic;

namespace FlyPlan.Data.Models
{
    public partial class ConfirmationInfo
    {
        public ConfirmationInfo()
        {
            Order = new HashSet<Order>();
        }

        public Guid Id { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAcceptedRule { get; set; }
        public bool IsSendEmail { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public ICollection<Order> Order { get; set; }
    }
}
