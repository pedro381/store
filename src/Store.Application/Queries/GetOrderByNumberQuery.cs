using MediatR;
using Store.Domain.Dtos;

namespace Store.Application.Queries
{
    public class GetOrderByNumberQuery(string orderNumber) : IRequest<OrderDto>
    {
        public string Number { get; } = orderNumber;
    }
}
