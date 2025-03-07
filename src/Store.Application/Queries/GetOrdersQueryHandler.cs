using AutoMapper;
using MediatR;
using Store.Domain.Dtos;
using Store.Infrastructure.Interfaces;

namespace Store.Application.Queries
{
    public class GetOrdersQueryHandler(
        IOrderRepository orderRepository,
        IMapper mapper) : IRequestHandler<GetOrdersQuery, (IEnumerable<OrderDto>, int)>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<(IEnumerable<OrderDto>, int)> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var (orders, total) = await _orderRepository.GetPagedOrdersAsync(request.PageNumber, request.PageSize, request.OrderBy);
            var ordersDto = orders.Select(s => _mapper.Map<OrderDto>(s));
            return (ordersDto, total);
        }
    }
}
