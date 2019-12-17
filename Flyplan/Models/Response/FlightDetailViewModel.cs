using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlyPlan.Api.Models.Response
{
    public class FlightDetailViewModel
    {
        public Guid Id { get; set; }
        public int RoundTrip { get; set; }
        public string Depart { get; set; }
        public string DepartTime { get; set; }
        public string DepartAirport { get; set; }
        public string Return { get; set; }
        public string ReturnTime { get; set; }
        public string ReturnAirport { get; set; }
        public string TotalTime { get; set; }
        public string ClassType { get; set; }
        public string DepartAirlinePicture { get; set; }
        public string DepartAirlineName { get; set; }
        public string DepartAirlinePlane { get; set; }
        public string ReturnAirlinePicture { get; set; }
        public string ReturnAirlineName { get; set; }
        public string ReturnAirlinePlane { get; set; }
        public int TotalMoney { get; set; }
    }
}
