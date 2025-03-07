using AutoMapper;
using Store.Domain.Dtos;
using Store.Domain.Entities;

namespace Store.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderDto, Order>();
            CreateMap<Order, OrderDto>();

            CreateMap<OrderItemDto, OrderItem>();
            CreateMap<OrderItem, OrderItemDto>();

            CreateMap<OrderItemCreateDto, OrderItemDto>()
                .ForMember(dest => dest.Discount, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.TotalItemAmount, opt => opt.MapFrom(src => src.Quantity * src.UnitPrice));

            CreateMap<OrderCreateDto, OrderDto>()
                .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => (DateTime?)null))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.TotalAmount = dest.OrderItems.Sum(item => item.TotalItemAmount);
                });
        }
    }
}
