using AutoMapper;
using NSubstitute;
using Store.Domain.Entities;
using Store.Infrastructure.Interfaces;
using Store.Application.Queries;
using Store.Test.Mocks;
using Store.Test.Fakes;
using Store.Domain.Dtos;

namespace Store.Test.Service.Queries
{
    public class GetOrdersQueryHandlerTest
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly GetOrdersQueryHandler _handler;

        public GetOrdersQueryHandlerTest()
        {
            _orderRepository = OrderRepositoryMock.Create();
            _mapper = MapperMock.Create();
            _handler = new GetOrdersQueryHandler(_orderRepository, _mapper);
        }

        [Fact]
        public async Task Handle_OrdersExist_ReturnsOrdersDtoWithTotal()
        {
            var query = new GetOrdersQuery(1, 10, "CreatedDate desc");
            var orderFaker = new OrderFaker();
            var orders = new List<Order> { orderFaker.Generate() };
            orders[0].Number = "123";
            _orderRepository.GetPagedOrdersAsync(query.PageNumber, query.PageSize, query.OrderBy)
                .Returns((orders, orders.Count));
            _mapper.Map<OrderDto>(Arg.Any<Order>())
                .Returns(callInfo =>
                {
                    var order = callInfo.Arg<Order>();
                    return new OrderDto { Number = order.Number };
                });
            var (result, total) = await _handler.Handle(query, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(orders.Count, total);
        }

        [Fact]
        public async Task Handle_NoOrders_ReturnsEmptyListWithZeroTotal()
        {
            var query = new GetOrdersQuery(1, 10, "CreatedDate desc");
            var orders = new List<Order>();
            _orderRepository.GetPagedOrdersAsync(query.PageNumber, query.PageSize, query.OrderBy)
                .Returns((orders, orders.Count));
            var (result, total) = await _handler.Handle(query, CancellationToken.None);
            Assert.Empty(result);
            Assert.Equal(0, total);
        }
    }
}
