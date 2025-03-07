using MediatR;
using Microsoft.AspNetCore.Mvc;
using Store.Domain.Dtos;
using Store.Application.Commands;
using Store.Application.Queries;
using Microsoft.AspNetCore.Authorization;
using Store.Domain.Base;
using AutoMapper;

namespace Store.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController(IMediator mediator, IMapper mapper, ILogger<OrdersController> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<OrdersController> _logger = logger;

        [HttpGet]
        [Authorize(Roles = "Admin,GetOrders")]
        [ProducesResponseType(typeof(PagedResult<OrderDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrders(
            [FromQuery(Name = "_page")] int page = 1,
            [FromQuery(Name = "_size")] int size = 10,
            [FromQuery(Name = "_order")] string order = "CreatedDate desc")
        {
            var query = new GetOrdersQuery
            {
                PageNumber = page,
                PageSize = size,
                OrderBy = order

            };
            var (orders, total) = await _mediator.Send(query);

            var response = new PagedResult<OrderDto>(orders, total, page, size);

            return Ok(response);
        }

        [HttpGet("{orderNumber}")]
        [Authorize(Roles = "Admin,GetOrderByNumber")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrderByNumber(string orderNumber)
        {
            var query = new GetOrderByNumberQuery(orderNumber);
            var order = await _mediator.Send(query);
            return Ok(order);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,CreateOrder")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto orderCreateDto)
        {
            var orderDto = _mapper.Map<OrderDto>(orderCreateDto);
            var command = new CreateOrderCommand(orderDto);
            var createdOrder = await _mediator.Send(command);

            if (createdOrder.HasErrors)
                return Ok(createdOrder);            

            _logger.LogInformation("OrderCreated: {Number}", createdOrder.Number);

            return CreatedAtAction(nameof(GetOrderByNumber), new { orderNumber = createdOrder.Number }, createdOrder);
        }

        [HttpPut()]
        [Authorize(Roles = "Admin,UpdateOrder")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderItemUpdateDto orderDto)
        {
            var command = new UpdateOrderCommand(orderDto);
            var updatedOrder = await _mediator.Send(command);

            if (updatedOrder.HasErrors)
                return Ok(updatedOrder);

            _logger.LogInformation("OrderModified: {Number}", updatedOrder.Number);

            return Ok(updatedOrder);
        }

        [HttpDelete("{orderNumber}")]
        [Authorize(Roles = "Admin,CancelOrder")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> CancelOrder(string orderNumber)
        {
            var command = new DeleteOrderCommand(orderNumber);
            _ = await _mediator.Send(command);

            _logger.LogInformation("OrderCancelled: {Number}", orderNumber);

            return Ok();
        }
    }
}
