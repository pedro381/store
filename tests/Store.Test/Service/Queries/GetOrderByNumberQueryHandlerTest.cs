using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Store.Domain.Dtos;
using Store.Domain.Entities;
using Store.Infrastructure.Interfaces;
using Store.Application.Queries;
using Store.Test.Mocks;
using Store.Domain;

namespace Store.Test.Service.Queries
{
    public class GetOrderByNumberQueryHandlerTest
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetOrderByNumberQueryHandler> _logger;
        private readonly GetOrderByNumberQueryHandler _handler;

        public GetOrderByNumberQueryHandlerTest()
        {
            _orderRepository = OrderRepositoryMock.Create();
            _mapper = MapperMock.Create();
            _logger = LoggerMock<GetOrderByNumberQueryHandler>.Create();
            _handler = new GetOrderByNumberQueryHandler(_orderRepository, _mapper, _logger);
        }

        [Fact]
        public async Task Handle_OrderExists_ReturnsOrderDto()
        {
            var orderNumber = "12345";
            var query = new GetOrderByNumberQuery(orderNumber);
            var orderEntity = new Order { Number = orderNumber };
            var orderDto = new OrderDto { Number = orderNumber };

            _orderRepository.GetOrderByNumberAsync(orderNumber).Returns(orderEntity);
            _mapper.Map<OrderDto>(orderEntity).Returns(orderDto);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(orderNumber, result.Number);
        }

        [Fact]
        public async Task Handle_OrderDoesNotExist_ThrowsKeyNotFoundException()
        {
            var orderNumber = "12345";
            var query = new GetOrderByNumberQuery(orderNumber);
            _orderRepository.GetOrderByNumberAsync(orderNumber).Returns((Order?)null);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(Resource.msgOrderNotFound, result.Errors.First().Message);
        }
    }
}
