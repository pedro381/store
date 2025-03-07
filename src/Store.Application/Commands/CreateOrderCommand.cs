using MediatR;
using Store.Domain.Dtos;

namespace Store.Application.Commands
{
    public class CreateOrderCommand(OrderDto orderDto) : IRequest<OrderDto>
    {
        public OrderDto OrderDto { get; } = orderDto;
    }
}
