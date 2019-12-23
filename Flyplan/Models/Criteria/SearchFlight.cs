﻿using System;
using System.Collections.Generic;
using FlyPlan.Api.Classes;

namespace FlyPlan.Api.Models.Criteria
{
    public class SearchFlight
    {
        public string From { get; set; }
        public string To { get; set; }
        public DateTime? DepartDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string ClassType { get; set; }
        public int? RoundTrip { get; set; }
        public float? PriceFrom { get; set; }
        public float? PriceTo { get; set; }
        public List<string> Airlines { get; set; }
        public OrderByEnum OrderBy { get; set; }
        public int? NumberOfRecord { get; set; }
    }
}
