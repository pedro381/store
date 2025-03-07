using AutoMapper;
using NSubstitute;

namespace Store.Test.Mocks
{
    public static class MapperMock
    {
        public static IMapper Create() => Substitute.For<IMapper>();
    }
}
