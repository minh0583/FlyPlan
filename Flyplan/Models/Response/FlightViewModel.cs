using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlyPlan.Api.Models.Response
{
    public class FlightViewModel
    {
        public Guid Id { get; set; }
        public Guid? FlightDetailId { get; set; }
        public FlightDetailViewModel FlightDetailViewModel { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
