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
    public class CreateOrderCommandHandlerTest
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IDiscountConfigurationRepository _discountConfigurationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateOrderCommandHandler> _logger;
        private readonly CreateOrderCommandHandler _handler;

        public CreateOrderCommandHandlerTest()
        {
            _orderRepository = OrderRepositoryMock.Create();
            _discountConfigurationRepository = DiscountConfigurationRepositoryMock.Create();
            _mapper = MapperMock.Create();
            _logger = LoggerMock<CreateOrderCommandHandler>.Create();
            _handler = new CreateOrderCommandHandler(_orderRepository, _discountConfigurationRepository, _mapper, _logger);
        }

        [Fact]
        public async Task Handle_OrderAlreadyExists_ReturnsOrderDtoWithError()
        {
            var orderDto = new OrderDtoFaker().Generate();
            _orderRepository.GetOrderByNumberAsync(orderDto.Number)!.Returns(Task.FromResult(new OrderFaker().Generate()));
            var command = new CreateOrderCommand(orderDto);
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(string.Format(Resource.msgOrderAlreadyExists, orderDto.Number), result.Errors.First().Message);
        }

        [Fact]
        public async Task Handle_ItemQuantityExceedsLimit_ReturnsOrderDtoWithError()
        {
            var orderDto = new OrderDtoFaker().Generate();
            orderDto.OrderItems.First().Quantity = 25;
            _orderRepository.GetOrderByNumberAsync(orderDto.Number).Returns(Task.FromResult((Order?)null));
            var command = new CreateOrderCommand(orderDto);
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Errors);
            var orderItemId = orderDto.OrderItems.First().OrderItemId;
            Assert.Contains(string.Format(Resource.msgItemsMoreThan20, orderItemId), result.Errors.First().Message);
        }

        [Fact]
        public async Task Handle_SuccessfulCreation_ReturnsCreatedOrderDto()
        {
            var orderDto = new OrderDtoFaker().Generate();
            foreach (var item in orderDto.OrderItems)
            {
                item.Quantity = 5;
            }
            _orderRepository.GetOrderByNumberAsync(orderDto.Number).Returns(Task.FromResult((Order?)null));
            var order = new OrderFaker().Generate();
            order.OrderItems.Clear();
            foreach (var item in orderDto.OrderItems)
            {
                order.OrderItems.Add(new OrderItem { OrderItemId = item.OrderItemId, Quantity = item.Quantity, ProductId = item.ProductId });
            }
            _mapper.Map<Order>(orderDto).Returns(order);
            _discountConfigurationRepository.GetActiveDiscountsAsync(order).Returns(Task.FromResult(50m));
            _orderRepository.AddOrderAsync(order).Returns(Task.CompletedTask);
            var createdOrderDto = new OrderDtoFaker().Generate();
            _mapper.Map<OrderDto>(order).Returns(createdOrderDto);
            var command = new CreateOrderCommand(orderDto);
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Empty(result.Errors);
            Assert.Equal(createdOrderDto.Number, result.Number);
        }

        [Fact]
        public async Task Handle_ExceptionDuringCreation_ReturnsOrderDtoWithError()
        {
            var orderDto = new OrderDtoFaker().Generate();
            foreach (var item in orderDto.OrderItems)
            {
                item.Quantity = 5;
            }
            _orderRepository.GetOrderByNumberAsync(orderDto.Number).Returns(Task.FromResult((Order?)null));
            var order = new OrderFaker().Generate();
            order.OrderItems.Clear();
            foreach (var item in orderDto.OrderItems)
            {
                order.OrderItems.Add(new OrderItem { OrderItemId = item.OrderItemId, Quantity = item.Quantity, ProductId = item.ProductId });
            }
            _mapper.Map<Order>(orderDto).Returns(order);
            _discountConfigurationRepository.GetActiveDiscountsAsync(order).Returns(Task.FromResult(50m));
            _orderRepository.AddOrderAsync(order).Returns<Task>(x => { throw new Exception("Creation failed"); });
            var command = new CreateOrderCommand(orderDto);
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(string.Format(Resource.msgOrderError, "Creation failed"), result.Errors.First().Message);
        }
    }
}