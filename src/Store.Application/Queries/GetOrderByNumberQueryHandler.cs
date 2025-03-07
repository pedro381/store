using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Store.Domain;
using Store.Domain.Dtos;
using Store.Infrastructure.Interfaces;

namespace Store.Application.Queries
{
    public class GetOrderByNumberQueryHandler(
        IOrderRepository orderRepository,
        IMapper mapper,
        ILogger<GetOrderByNumberQueryHandler> logger) : IRequestHandler<GetOrderByNumberQuery, OrderDto>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetOrderByNumberQueryHandler> _logger = logger;

        public async Task<OrderDto> Handle(GetOrderByNumberQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(Resource.msgRetrievingOrder, request.Number);
            var order = await _orderRepository.GetOrderByNumberAsync(request.Number);
           if (order == null)
            {
                _logger.LogError(Resource.msgOrderNotFound);
                var orderDtoError = new OrderDto();
                orderDtoError.AddErro(Resource.msgOrderNotFound);
                return orderDtoError;
            }

            var orderDto = _mapper.Map<OrderDto>(order);
            return orderDto;
        }
    }
}
