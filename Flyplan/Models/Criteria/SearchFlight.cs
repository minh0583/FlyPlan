using System;

namespace FlyPlan.Api.Models.Criteria
{
    public class SearchFlight
    {
        public string From { get; set; }
        public string To { get; set; }
        public DateTime? DepartDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Adults { get; set; }
        public string Children { get; set; }
        public string Infants { get; set; }
        public string ClassType { get; set; }
        public int? RoundTrip { get; set; }
        public float? PriceFrom { get; set; }
        public float? PriceTo { get; set; }
        public string DepartTime { get; set; }
        public string Airlines { get; set; }
    }
}
