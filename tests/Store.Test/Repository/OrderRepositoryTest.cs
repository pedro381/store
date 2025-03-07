using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Store.Domain;
using Store.Domain.Entities;
using Store.Infrastructure.Repositories;
using Store.Test.Factories;
using Store.Test.Fakes;

namespace Store.Test.Repository
{
    public class UpdateOrderCommandHandlerTest
    {
        [Fact]
        public async Task AddOrderAsync_ShouldAddOrder()
        {
            using var context = TestDbContextFactory.Create();
            var logger = Substitute.For<ILogger<OrderRepository>>();
            var repository = new OrderRepository(context, logger);
            var orderFaker = new OrderFaker();
            var order = orderFaker.Generate();
            order.Number = "S001";
            await repository.AddOrderAsync(order);
            var result = await context.Orders.FirstOrDefaultAsync(s => s.Number == "S001");
            Assert.NotNull(result);
            Assert.Equal("S001", result.Number);
        }

        [Fact]
        public async Task UpdateOrderAsync_ShouldUpdateOrder()
        {
            using var context = TestDbContextFactory.Create();
            var logger = Substitute.For<ILogger<OrderRepository>>();
            var repository = new OrderRepository(context, logger);
            var orderFaker = new OrderFaker();
            var order = orderFaker.Generate();
            order.Number = "S002";
            order.OrderItems = [new OrderItem { OrderItemId = 1 }];
            await repository.AddOrderAsync(order);
            order = await repository.GetOrderByNumberAsync("S002");
            await repository.UpdateOrderAsync(order!);
            var result = await context.Orders.Include(s => s.OrderItems)
                                            .FirstOrDefaultAsync(s => s.Number == "S002");
            Assert.NotNull(result);
            Assert.Single(result.OrderItems);
        }

        [Fact]
        public async Task DeleteOrderAsync_ShouldDeleteOrder()
        {
            using var context = TestDbContextFactory.Create();
            var logger = Substitute.For<ILogger<OrderRepository>>();
            var repository = new OrderRepository(context, logger);
            var orderFaker = new OrderFaker();
            var order = orderFaker.Generate();
            order.Number = "S003";
            await repository.AddOrderAsync(order);
            await repository.DeleteOrderAsync("S003");
            var result = await context.Orders.FirstOrDefaultAsync(s => s.Number == "S003");
            Assert.True(result!.IsCancelled);
        }

        [Fact]
        public async Task DeleteOrderAsync_NonExistingOrder_ShouldLogWarning()
        {
            using var context = TestDbContextFactory.Create();
            var logger = Substitute.For<ILogger<OrderRepository>>();
            var repository = new OrderRepository(context, logger);
            await repository.DeleteOrderAsync("NonExistingOrder");
            logger.Received(1).Log(
                Arg.Is<LogLevel>(l => l == LogLevel.Warning),
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString()!.Contains(Resource.msgOrderNotFound)),
                Arg.Any<Exception>(),
                Arg.Any<Func<object, Exception?, string>>());
        }

        [Fact]
        public async Task GetOrderByNumberAsync_ShouldReturnOrder()
        {
            using var context = TestDbContextFactory.Create();
            var logger = Substitute.For<ILogger<OrderRepository>>();
            var repository = new OrderRepository(context, logger);
            var orderFaker = new OrderFaker();
            var order = orderFaker.Generate();
            order.Number = "S004";
            await repository.AddOrderAsync(order);
            var result = await repository.GetOrderByNumberAsync("S004");
            Assert.NotNull(result);
            Assert.Equal("S004", result?.Number);
        }

        [Fact]
        public async Task GetPagedOrdersAsync_ShouldReturnPagedResults()
        {
            using var context = TestDbContextFactory.Create();
            var logger = Substitute.For<ILogger<OrderRepository>>();
            var repository = new OrderRepository(context, logger);
            var orderFaker = new OrderFaker();
            for (var i = 1; i <= 15; i++)
            {
                var order = orderFaker.Generate();
                order.Number = $"S{i:D3}";
                await repository.AddOrderAsync(order);
            }
            var pageNumber = 2;
            var pageSize = 5;
            var orderBy = "Number";
            var (orders, totalRecords) = await repository.GetPagedOrdersAsync(pageNumber, pageSize, orderBy);
            Assert.Equal(15, totalRecords);
            Assert.Equal(pageSize, orders.Count());
            var expectedNumber = $"S{(pageNumber - 1) * pageSize + 1:D3}";
            Assert.Equal(expectedNumber, orders.First().Number);
        }
    }
}
