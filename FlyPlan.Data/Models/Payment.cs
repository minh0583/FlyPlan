using System;
using System.Collections.Generic;

namespace FlyPlan.Data.Models
{
    public partial class Payment
    {
        public Payment()
        {
            Order = new HashSet<Order>();
        }

        public Guid Id { get; set; }
        public string CreditCardType { get; set; }
        public string CardNumber { get; set; }
        public string NameOnTheCard { get; set; }
        public string ExpiryDateInMonth { get; set; }
        public string ExpiryDateInYear { get; set; }
        public string Cvvcode { get; set; }
        public string CountryId { get; set; }
        public string BillingAddress { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public ICollection<Order> Order { get; set; }
    }
}
