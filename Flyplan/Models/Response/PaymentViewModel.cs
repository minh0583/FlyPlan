using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlyPlan.Api.Models.Response
{
    public class PaymentViewModel
    {
        public Guid Id { get; set; }
        public string CreditCardType { get; set; }
        public string CardNumber { get; set; }
        public string NameOnTheCard { get; set; }
        public string ExpiryDateInMonth { get; set; }
        public string ExpiryDateInYear { get; set; }
        public string CVVCode { get; set; }
        public string CountryId { get; set; }
        public string BillingAddress { get; set; }
        public string City { get; set; }
        public string ZIPCode { get; set; }
    }
}
