using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlyPlan.Api.Models.Response
{
    public class ConfirmationInfoViewModel
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public Boolean IsAcceptedRule { get; set; }
        public Boolean IsSendEmail { get; set; }
    }
}
