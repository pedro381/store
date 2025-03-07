using Store.Domain.Entities;
using Store.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Store.Infrastructure.Data;

namespace Store.Infrastructure.Repositories
{
    public class DiscountConfigurationRepository(StoreDbContext context) : IDiscountConfigurationRepository
    {
        public async Task<decimal> GetActiveDiscountsAsync(Order order)
        {
            var discountRules = await context.DiscountConfigurations
                .Where(dc => dc.IsActive)
                .ToListAsync();

            decimal totalOrderAmount = 0;
            foreach (var orderItem in order.OrderItems)
            {
                var rule = discountRules.FirstOrDefault(r =>
                    orderItem.Quantity >= r.MinQuantity
                    && orderItem.Quantity <= r.MaxQuantity);

                var discountAmount = 0.0m;
                if (rule != null)
                {
                    var discountPercentage = rule.DiscountPercentage;
                    var baseAmount = orderItem.Quantity * orderItem.UnitPrice;
                    discountAmount = baseAmount * discountPercentage;
                    orderItem.TotalItemAmount = baseAmount - discountAmount;
                }
                orderItem.Discount = discountAmount;
                totalOrderAmount += orderItem.TotalItemAmount;
            }
            return totalOrderAmount;
        }
    }
}
