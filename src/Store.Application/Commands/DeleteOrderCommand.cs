using MediatR;

namespace Store.Application.Commands
{
    public class DeleteOrderCommand(string orderNumber) : IRequest<bool>
    {
        public string Number { get; } = orderNumber;
    }
}
