﻿using System;
using System.Collections.Generic;
using FlyPlan.Api.Models.Request;

namespace FlyPlan.Api.Models.Response
{
    public class BookingViewModelResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public FlightViewModelResponse FlightViewModel { get; set; }
        public List<TravellerViewModelResponse> TravellerViewModels { get; set; }
        public PaymentViewModelResponse PaymentViewModel { get; set; }
        public ConfirmationInfoViewModelResponse ConfirmationInfoViewModel { get; set; }
    }
}
