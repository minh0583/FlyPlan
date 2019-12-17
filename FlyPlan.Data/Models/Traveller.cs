using System;
using System.Collections.Generic;

namespace FlyPlan.Data.Models
{
    public partial class Traveller
    {
        public Traveller()
        {
            TravellerOrder = new HashSet<TravellerOrder>();
        }

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
        public bool? PasportNoExpiry { get; set; }
        public string CheckedBaggae { get; set; }
        public string TravelInsurance { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public ICollection<TravellerOrder> TravellerOrder { get; set; }
    }
}
