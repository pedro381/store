using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using Store.Domain.Entities;
using Store.Infrastructure.Data;
using Store.Infrastructure.Interfaces;
using Store.Domain;

namespace Store.Infrastructure.Repositories;

public class OrderRepository(StoreDbContext context, ILogger<OrderRepository> logger) : IOrderRepository
{
    private readonly StoreDbContext _context = context;
    private readonly ILogger<OrderRepository> _logger = logger;

    public async Task AddOrderAsync(Order order)
    {
        try
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Resource.msgOrderError, order.Number);
            throw;
        }
    }

    public async Task<Order?> UpdateOrderAsync(Order order)
    {
        try
        {
            await _context.SaveChangesAsync();
            return order;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Resource.msgOrderError, order.Number);
            throw;
        }
    }

    public async Task<Order?> RemoveAsync(int orderItemId)
    {
        try
        {
            var orderItem = await _context.OrderItems.SingleAsync(s => s.OrderItemId == orderItemId);
             _context.OrderItems.Remove(orderItem);            
            await _context.SaveChangesAsync();

            return await GetOrderByItemIdAsync(orderItemId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Resource.msgOrderError, orderItemId);
            throw;
        }
    }

    public async Task DeleteOrderAsync(string orderNumber)
    {
        try
        {
            var order = await _context.Orders.FirstOrDefaultAsync(s => s.Number == orderNumber);
            if (order != null)
            {
                order.IsCancelled = true;
                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning(Resource.msgOrderNotFound, orderNumber);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Resource.msgOrderError, orderNumber);
            throw;
        }
    }

    public async Task<Order?> GetOrderByNumberAsync(string orderNumber)
    {
        try
        {
            return await _context.Orders
                .Include(s => s.OrderItems)
                .FirstOrDefaultAsync(s => s.Number == orderNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Resource.msgOrderError, orderNumber);
            throw;
        }
    }

    public async Task<Order?> GetOrderByItemIdAsync(int orderItemId)
    {
        try
        {
            return await _context.Orders
                .Include(s => s.OrderItems)
                .FirstOrDefaultAsync(s => s.OrderItems.Any(y => y.OrderItemId == orderItemId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Resource.msgOrderError, orderItemId);
            throw;
        }
    }

    public async Task<(IEnumerable<Order>, int)> GetPagedOrdersAsync(int pageNumber, int pageSize, string order)
    {
        try
        {
            var query = _context.Orders.Include(s => s.OrderItems);
            var totalRecords = await query.CountAsync();
            var orders = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(order)
                .ToListAsync();
            return (orders, totalRecords);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Resource.msgErrorOrderPage, pageNumber, pageSize);
            throw;
        }
    }
}
