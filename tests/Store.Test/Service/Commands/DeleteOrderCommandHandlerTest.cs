using Microsoft.Extensions.Logging;
using NSubstitute;
using Store.Infrastructure.Interfaces;
using Store.Application.Commands;
using Store.Test.Mocks;

namespace Store.Test.Service.Commands
{
    public class DeleteOrderCommandHandlerTest
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<DeleteOrderCommandHandler> _logger;
        private readonly DeleteOrderCommandHandler _handler;

        public DeleteOrderCommandHandlerTest()
        {
            _orderRepository = OrderRepositoryMock.Create();
            _logger = LoggerMock<DeleteOrderCommandHandler>.Create();
            _handler = new DeleteOrderCommandHandler(_orderRepository, _logger);
        }

        [Fact]
        public async Task Handle_ValidNumber_DeletesOrderAndReturnsTrue()
        {
            var orderNumber = "123";
            _orderRepository.DeleteOrderAsync(orderNumber).Returns(Task.CompletedTask);

            var command = new DeleteOrderCommand(orderNumber);
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result);
            await _orderRepository.Received(1).DeleteOrderAsync(orderNumber);
        }
    }
}
