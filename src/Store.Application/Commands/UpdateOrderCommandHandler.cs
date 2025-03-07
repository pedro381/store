using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Store.Domain;
using Store.Domain.Dtos;
using Store.Infrastructure.Interfaces;

namespace Store.Application.Commands
{
    public class UpdateOrderCommandHandler(
        IOrderRepository _orderRepository,
        IDiscountConfigurationRepository _discountConfigurationRepository,
        IMapper _mapper,
        ILogger<UpdateOrderCommandHandler> _logger) : IRequestHandler<UpdateOrderCommand, OrderDto>
    {

        public async Task<OrderDto> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var orderCreated = await _orderRepository.GetOrderByItemIdAsync(request.OrderDto.OrderItemId);
                if (orderCreated == null)
                {
                    _logger.LogError(Resource.msgOrderNotFound);
                    var orderDto = new OrderDto();
                    orderDto.AddErro(Resource.msgOrderNotFound);
                    return orderDto;
                }

                if (orderCreated.IsCancelled)
                {
                    _logger.LogError(Resource.msgOrderIsCancelled);
                    var orderDto = new OrderDto();
                    orderDto.AddErro(Resource.msgOrderIsCancelled);
                    return orderDto;
                }

                if (request.OrderDto.Quantity > 20)
                {
                    var erro = string.Format(Resource.msgItemsMoreThan20, request.OrderDto.OrderItemId);
                    _logger.LogError(erro);
                    var orderDto = new OrderDto();
                    orderDto.AddErro(erro);
                    return orderDto;
                }

                _logger.LogInformation(Resource.msgOrderSuccess, orderCreated.Number);

                var item = orderCreated.OrderItems.Single(x => x.OrderItemId == request.OrderDto.OrderItemId);
                item.Quantity = request.OrderDto.Quantity;

                orderCreated.TotalAmount = await _discountConfigurationRepository.GetActiveDiscountsAsync(orderCreated);

                var existingOrder = await _orderRepository.UpdateOrderAsync(orderCreated);

                var updatedOrderDto = _mapper.Map<OrderDto>(existingOrder);
                return updatedOrderDto;
            }
            catch (Exception ex)
            {
                var erro = string.Format(Resource.msgOrderError, ex.Message);
                _logger.LogError(ex, erro);
                var orderDto = new OrderDto();
                orderDto.AddErro(erro);
                return orderDto;
            }
        }
    }
}
