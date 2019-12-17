using System;
using AutoMapper;
using FlyPlan.Api.Models.Response;
using FlyPlan.Data.Models;

namespace FlyPlan.Api.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<OrderViewModel, Order>()
                .ForMember(p => p.CreatedDate, opt => opt.MapFrom(p => p.Id == Guid.Empty ? DateTime.Now : (DateTime?)null))
                .ForMember(p => p.UpdatedDate, opt => opt.MapFrom(p => p.Id != Guid.Empty ? DateTime.Now : (DateTime?)null));

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
