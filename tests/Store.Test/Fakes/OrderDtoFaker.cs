using Bogus;
using Store.Domain.Dtos;

namespace Store.Test.Fakes
{
    public class OrderDtoFaker : Faker<OrderDto>
    {
        private readonly string[] paymentMethod = ["Credit Card", "Debit Card", "Cash", "Bank Transfer", "Paypal"];
        private readonly OrderItemDtoFaker orderItemFaker = new();

        public OrderDtoFaker()
        {
            RuleFor(o => o.OrderId, f => f.IndexFaker + 1);
            RuleFor(o => o.Number, f => f.Commerce.Ean13());
            RuleFor(o => o.CustomerName, f => f.Person.FullName);
            RuleFor(o => o.PaymentMethod, f => f.PickRandom(paymentMethod));
            RuleFor(o => o.IsCancelled, f => false);
            RuleFor(o => o.CreatedDate, f => f.Date.Past(1));
            RuleFor(o => o.UpdatedDate, f => f.Random.Bool(0.5f) ? f.Date.Recent(30) : null);
            RuleFor(o => o.OrderItems, (f, o) =>
            {
                var items = orderItemFaker.Generate(f.Random.Int(1, 5));
                items.ForEach(item => item.OrderId = o.OrderId);
                return items;
            });
            RuleFor(o => o.TotalAmount, (f, o) =>
            {
                return o.OrderItems.Sum(i => i.TotalItemAmount);
            });
        }
    }
}
