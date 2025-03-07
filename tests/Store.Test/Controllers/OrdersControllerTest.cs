using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Store.Api.Controllers;
using Store.Application.Commands;
using Store.Application.Queries;
using Store.Domain.Base;
using Store.Domain.Dtos;
using Store.Test.Fakes;

namespace Store.Test.Controllers
{
    public class OrdersControllerTest
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<OrdersController> _logger;
        private readonly OrdersController _controller;

        public OrdersControllerTest()
        {
            _mediator = Substitute.For<IMediator>();
            _mapper = Substitute.For<IMapper>();
            _logger = Substitute.For<ILogger<OrdersController>>();
            _controller = new OrdersController(_mediator, _mapper, _logger);
        }

        [Fact]
        public async Task GetOrders_ReturnsPagedResult()
        {
            var page = 2;
            var size = 5;
            var order = "CreatedDate asc";
            var orderFaker = new OrderDtoFaker();
            var orders = new List<OrderDto>
            {
                orderFaker.Generate(),
                orderFaker.Generate()
            };
            var total = 10;
            (IEnumerable<OrderDto>, int) resultOrder = (orders, total);
            _mediator.Send(Arg.Any<GetOrdersQuery>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(resultOrder));
            var result = await _controller.GetOrders(page, size, order);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var pagedResult = Assert.IsType<PagedResult<OrderDto>>(okResult.Value);
            Assert.Equal(orders, pagedResult.Data);
            Assert.Equal(total, pagedResult.TotalItems);
            Assert.Equal(page, pagedResult.CurrentPage);
        }

        [Fact]
        public async Task GetOrderByNumber_ReturnsOrderDto()
        {
            var orderNumber = "ORD123";
            var orderDto = new OrderDto { Number = orderNumber };
            _mediator.Send(Arg.Is<GetOrderByNumberQuery>(q => q.Number == orderNumber), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(orderDto));
            var result = await _controller.GetOrderByNumber(orderNumber);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(orderDto, okResult.Value);
        }

        [Fact]
        public async Task CreateOrder_WithErrors_ReturnsOkResult()
        {
            var orderCreateDto = new OrderCreateDto { Number = "ORD123" };
            var orderDto = new OrderDto { Number = "ORD123" };
            _mapper.Map<OrderDto>(orderCreateDto).Returns(orderDto);
            orderDto.AddErro("Error");
            _mediator.Send(Arg.Is<CreateOrderCommand>(c => c.OrderDto == orderDto), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(orderDto));
            var result = await _controller.CreateOrder(orderCreateDto);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(orderDto, okResult.Value);
        }

        [Fact]
        public async Task CreateOrder_Successful_ReturnsCreatedAtAction()
        {
            var orderCreateDto = new OrderCreateDto { Number = "ORD123" };
            var orderDto = new OrderDto { Number = "ORD123" };
            _mapper.Map<OrderDto>(orderCreateDto).Returns(orderDto);
            _mediator.Send(Arg.Is<CreateOrderCommand>(c => c.OrderDto == orderDto), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(orderDto));
            var result = await _controller.CreateOrder(orderCreateDto);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetOrderByNumber), createdResult.ActionName);
            Assert.Equal(orderDto.Number, createdResult.RouteValues!["orderNumber"]);
            Assert.Equal(orderDto, createdResult.Value);
        }

        [Fact]
        public async Task UpdateOrder_WithErrors_ReturnsOkResult()
        {
            var orderItemUpdateDto = new OrderItemUpdateDto { OrderItemId = 1, Quantity = 5 };
            var orderDto = new OrderDto { Number = "ORD123" };
            orderDto.AddErro("Error");
            _mediator.Send(Arg.Is<UpdateOrderCommand>(c => c.OrderDto == orderItemUpdateDto), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(orderDto));
            var result = await _controller.UpdateOrder(orderItemUpdateDto);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(orderDto, okResult.Value);
        }

        [Fact]
        public async Task UpdateOrder_Successful_ReturnsOkResult()
        {
            var orderItemUpdateDto = new OrderItemUpdateDto { OrderItemId = 1, Quantity = 5 };
            var orderDto = new OrderDto { Number = "ORD123" };
            _mediator.Send(Arg.Is<UpdateOrderCommand>(c => c.OrderDto == orderItemUpdateDto), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(orderDto));
            var result = await _controller.UpdateOrder(orderItemUpdateDto);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(orderDto, okResult.Value);
        }

        [Fact]
        public async Task CancelOrder_ReturnsOkResultWithMessage()
        {
            var orderNumber = "ORD123";
            _mediator.Send(Arg.Is<DeleteOrderCommand>(c => c.Number == orderNumber), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(true));
            var result = await _controller.CancelOrder(orderNumber);
            var okResult = Assert.IsType<OkResult>(result);
            Assert.True(okResult.StatusCode == 200);
        }
    }
}