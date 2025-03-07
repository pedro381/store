using NSubstitute;
using Store.Infrastructure.Interfaces;

namespace Store.Test.Mocks
{
    public static class DiscountConfigurationRepositoryMock
    {
        public static IDiscountConfigurationRepository Create() => Substitute.For<IDiscountConfigurationRepository>();
    }
}
