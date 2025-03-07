using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Store.Test.Mocks
{
    public static class LoggerMock<T> where T : class
    {
        public static ILogger<T> Create() => Substitute.For<ILogger<T>>();
    }
}
