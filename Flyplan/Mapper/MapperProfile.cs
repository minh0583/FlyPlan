using System;
using AutoMapper;
using FlyPlan.Api.Models.Request;
using FlyPlan.Api.Models.Response;
using FlyPlan.Data.Models;

namespace FlyPlan.Api.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<OrderRequest, Order>()
                .ForMember(p => p.CreatedDate, opt => opt.MapFrom(p => DateTime.Now));

            CreateMap<Order, OrderViewModel>()
                .ForMember(p => p.PaymentViewModel, opt => opt.MapFrom(p => p.Payment))
                .ForMember(p => p.ConfirmationInfoViewModel, opt => opt.MapFrom(p => p.Confirmation))
                .ForMember(p => p.FlightViewModel, opt => opt.MapFrom(p => p.Flight));

            CreateMap<Payment, PaymentViewModel>();
            CreateMap<ConfirmationInfo, ConfirmationInfoViewModel>();
            CreateMap<Flight, FlightViewModel>();
            CreateMap<Traveller, TravellerViewModel>();
        }
    }
}
