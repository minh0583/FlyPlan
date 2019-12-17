using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlyPlan.Api.Models.Response
{
    public class TravellerViewModel
    {
        public Guid Id { get; set; }
        public string PersonType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Gender { get; set; }
        public int? Nationality { get; set; }
        public string PasportId { get; set; }
        public string PasportExpiryDateMonth { get; set; }
        public string PasportExpiryDateYear { get; set; }
        public Boolean PasportNoExpiry { get; set; }
        public string CheckedBaggae { get; set; }
        public string TravelInsurance { get; set; }
    }
}
