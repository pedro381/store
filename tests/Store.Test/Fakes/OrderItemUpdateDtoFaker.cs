using Bogus;
using Store.Domain.Dtos;

namespace Store.Test.Fakes
{
    public class OrderItemUpdateDtoFaker : Faker<OrderItemUpdateDto>
    {
        public OrderItemUpdateDtoFaker()
        {
            RuleFor(o => o.OrderItemId, f => f.IndexFaker + 1);
            RuleFor(oi => oi.Quantity, f => f.Random.Int(1, 10));
        }
    }
}
