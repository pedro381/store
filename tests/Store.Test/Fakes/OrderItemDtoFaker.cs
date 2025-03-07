using Bogus;
using Store.Domain.Dtos;

namespace Store.Test.Fakes
{
    public class OrderItemDtoFaker : Faker<OrderItemDto>
    {
        public OrderItemDtoFaker()
        {
            RuleFor(oi => oi.OrderItemId, f => f.IndexFaker + 1);
            RuleFor(oi => oi.ProductId, f => f.Random.Int(1, 1000));
            RuleFor(oi => oi.Quantity, f => f.Random.Int(1, 10));
            RuleFor(oi => oi.UnitPrice, f => Math.Round(f.Finance.Amount(1, 100), 2));
            RuleFor(oi => oi.Discount, f => Math.Round(f.Finance.Amount(0, 50), 2));
            RuleFor(oi => oi.Discount, f => Math.Round(f.Finance.Amount(0, 50), 2));
            RuleFor(oi => oi.TotalItemAmount, (f, oi) => Math.Round(oi.Quantity * oi.UnitPrice - oi.Discount, 2));
        }
    }
}
