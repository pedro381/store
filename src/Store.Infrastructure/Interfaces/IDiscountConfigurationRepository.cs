using Store.Domain.Entities;

namespace Store.Infrastructure.Interfaces
{
    public interface IDiscountConfigurationRepository
    {
        Task<decimal> GetActiveDiscountsAsync(Order order);
    }
}
