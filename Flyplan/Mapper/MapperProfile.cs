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

            CreateMap<Order, OrderViewModelResponse>()
                .ForMember(p => p.PaymentViewModel, opt => opt.MapFrom(p => p.Payment))
                .ForMember(p => p.ConfirmationInfoViewModel, opt => opt.MapFrom(p => p.Confirmation))
                .ForMember(p => p.FlightViewModel, opt => opt.MapFrom(p => p.Flight));

            CreateMap<Payment, PaymentViewModelResponse>();
            CreateMap<ConfirmationInfo, ConfirmationInfoViewModelResponse>();
            CreateMap<Flight, FlightViewModelResponse>();
            CreateMap<Traveller, TravellerViewModelResponse>();

            CreateMap<PaymentViewModel, Payment>()
                .ForMember(p => p.Id, opt => opt.MapFrom(p => Guid.NewGuid()))
                .ForMember(p => p.CreatedDate, opt => opt.MapFrom(p => DateTime.Now));

            CreateMap<ConfirmationInfoViewModel, ConfirmationInfo>()
                .ForMember(p => p.Id, opt => opt.MapFrom(p => Guid.NewGuid()))
                .ForMember(p => p.CreatedDate, opt => opt.MapFrom(p => DateTime.Now));

            CreateMap<TravellerViewModel, Traveller>()
                .ForMember(p => p.Id, opt => opt.MapFrom(p => Guid.NewGuid()))
                .ForMember(p => p.CreatedDate, opt => opt.MapFrom(p => DateTime.Now));
        }
    }
}
