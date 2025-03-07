using MediatR;
using Store.Domain.Dtos;

namespace Store.Application.Commands
{
    public class UpdateOrderCommand(OrderItemUpdateDto orderDto) : IRequest<OrderDto>
    {
        public OrderItemUpdateDto OrderDto { get; } = orderDto;
    }
}
