using NSubstitute;
using Store.Infrastructure.Interfaces;

namespace Store.Test.Mocks
{
    public static class OrderRepositoryMock
    {
        public static IOrderRepository Create() => Substitute.For<IOrderRepository>();
    }
}
