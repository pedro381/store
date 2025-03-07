using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Store.Domain;
using Store.Domain.Dtos;
using Store.Domain.Entities;
using Store.Infrastructure.Interfaces;

namespace Store.Application.Commands
{
    public class CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IDiscountConfigurationRepository discountConfigurationRepository,
        IMapper mapper,
        ILogger<CreateOrderCommandHandler> logger) : IRequestHandler<CreateOrderCommand, OrderDto>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IDiscountConfigurationRepository _discountConfigurationRepository = discountConfigurationRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CreateOrderCommandHandler> _logger = logger;

        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderDto = request.OrderDto;
            try
            {
                var orderCreated = await _orderRepository.GetOrderByNumberAsync(orderDto.Number);
                if (orderCreated != null)
                {
                    var erro = string.Format(Resource.msgOrderAlreadyExists, orderDto.Number);
                    _logger.LogError(erro);
                    orderDto.AddErro(erro);
                    return orderDto;
                }

                foreach (var itemDto in orderDto.OrderItems)
                {
                    if (itemDto.Quantity > 20)
                    {
                        var erro = string.Format(Resource.msgItemsMoreThan20, itemDto.OrderItemId);
                        _logger.LogError(erro);
                        orderDto.AddErro(erro);
                    }
                }

                if (orderDto.HasErrors) return orderDto;

                var order = _mapper.Map<Order>(orderDto);
                order.TotalAmount = await _discountConfigurationRepository.GetActiveDiscountsAsync(order);

                _logger.LogInformation(Resource.msgOrderSuccess, order.Number);
                await _orderRepository.AddOrderAsync(order);
                var createdOrderDto = _mapper.Map<OrderDto>(order);
                return createdOrderDto;
            }
            catch (Exception ex)
            {
                var erro = string.Format(Resource.msgOrderError, ex.Message);
                _logger.LogError(ex, erro);
                orderDto.AddErro(erro);
                return orderDto;
            }
        }

    }
}
