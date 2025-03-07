using MediatR;
using Microsoft.Extensions.Logging;
using Store.Domain;
using Store.Infrastructure.Interfaces;

namespace Store.Application.Commands
{
    public class DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger<DeleteOrderCommandHandler> logger) : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly ILogger<DeleteOrderCommandHandler> _logger = logger;

        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(Resource.msgDeletingOrder, request.Number);
            await _orderRepository.DeleteOrderAsync(request.Number);
            return true;
        }
    }
}
