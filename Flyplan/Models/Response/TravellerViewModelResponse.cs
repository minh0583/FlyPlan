﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlyPlan.Api.Models.Request;

namespace FlyPlan.Api.Models.Response
{
    public class TravellerViewModelResponse : TravellerViewModel
    {
        public Guid Id { get; set; }
    }
}
