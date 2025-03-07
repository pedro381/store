using Store.Domain.Entities;

namespace Store.Infrastructure.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
        Task DeleteOrderAsync(string orderNumber);
        Task<Order?> UpdateOrderAsync(Order order); 
        Task<Order?> RemoveAsync(int orderItemId);
        Task<Order?> GetOrderByNumberAsync(string orderNumber);
        Task<Order?> GetOrderByItemIdAsync(int orderItemId);
        Task<(IEnumerable<Order>, int)> GetPagedOrdersAsync(int pageNumber, int pageSize, string order);
    }
}
