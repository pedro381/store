using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Store.Domain.Dtos;
using Store.Domain.Entities;
using Store.Infrastructure.Interfaces;
using Store.Application.Commands;
using Store.Test.Mocks;
using Store.Test.Fakes;
using Store.Domain;

namespace Store.Test.Service.Commands
{
    public class UpdateOrderCommandHandlerTest
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDiscountConfigurationRepository _discountConfigurationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;
        private readonly UpdateOrderCommandHandler _handler;

        public UpdateOrderCommandHandlerTest()
        {
            _orderRepository = OrderRepositoryMock.Create();
            _discountConfigurationRepository = DiscountConfigurationRepositoryMock.Create();
            _mapper = MapperMock.Create();
            _logger = LoggerMock<UpdateOrderCommandHandler>.Create();
            _handler = new UpdateOrderCommandHandler(_orderRepository, _discountConfigurationRepository, _mapper, _logger);
        }

        [Fact]
        public async Task Handle_OrderNotFound_ReturnsOrderDtoWithError()
        {
            var orderDto = new OrderItemUpdateDtoFaker().Generate();
            _orderRepository.GetOrderByItemIdAsync(orderDto.OrderItemId).Returns(Task.FromResult((Order?)null));
            var command = new UpdateOrderCommand(orderDto);
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(Resource.msgOrderNotFound, result.Errors.First().Message);
        }

        [Fact]
        public async Task Handle_IsCancelled_ReturnsOrderDtoWithError()
        {
            var orderDto = new OrderItemUpdateDtoFaker().Generate();
            var order = new OrderFaker().Generate();
            order.IsCancelled = true;
            _orderRepository.GetOrderByItemIdAsync(orderDto.OrderItemId)!.Returns(Task.FromResult(order));
            var command = new UpdateOrderCommand(orderDto);
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(Resource.msgOrderIsCancelled, result.Errors.First().Message);
        }

        [Fact]
        public async Task Handle_QuantityExceedsLimit_ReturnsOrderDtoWithError()
        {
            var orderDto = new OrderItemUpdateDtoFaker().Generate();
            orderDto.Quantity = 21;
            var order = new OrderFaker().Generate();
            _orderRepository.GetOrderByItemIdAsync(orderDto.OrderItemId)!.Returns(Task.FromResult(order));
            var command = new UpdateOrderCommand(orderDto);
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(string.Format(Resource.msgItemsMoreThan20, orderDto.OrderItemId), result.Errors.First().Message);
        }

        [Fact]
        public async Task Handle_SuccessfulUpdate_ReturnsUpdatedOrderDto()
        {
            var orderItemUpdateDto = new OrderItemUpdateDtoFaker().Generate();
            orderItemUpdateDto.Quantity = 10;
            var orderDto = new OrderDtoFaker().Generate();
            var order = new OrderFaker().Generate();
            _orderRepository.GetOrderByItemIdAsync(orderItemUpdateDto.OrderItemId)!.Returns(Task.FromResult(order));
            _discountConfigurationRepository.GetActiveDiscountsAsync(order).Returns(Task.FromResult(50m));
            _orderRepository.UpdateOrderAsync(order)!.Returns(Task.FromResult(order));
            _mapper.Map<OrderDto>(order).Returns(orderDto);
            var command = new UpdateOrderCommand(orderItemUpdateDto);
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Empty(result.Errors);
            Assert.Equal(10, order.OrderItems.Single(x => x.OrderItemId == orderItemUpdateDto.OrderItemId).Quantity);
        }

        [Fact]
        public async Task Handle_ExceptionDuringUpdate_ReturnsOrderDtoWithError()
        {
            var orderDto = new OrderItemUpdateDtoFaker().Generate();
            var order = new OrderFaker().Generate();
            order.OrderItems.Add(new OrderItem { OrderItemId = 123, Quantity = 1 });
            _orderRepository.GetOrderByItemIdAsync(orderDto.OrderItemId)!.Returns(Task.FromResult(order));
            _discountConfigurationRepository.GetActiveDiscountsAsync(order).Returns(Task.FromResult(50m));
            _orderRepository.UpdateOrderAsync(order)!.Returns<Task<Order>>(x => { throw new Exception("Update failed"); });
            var command = new UpdateOrderCommand(orderDto);
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(string.Format(Resource.msgOrderError, "Update failed"), result.Errors.First().Message);
        }
    }
}