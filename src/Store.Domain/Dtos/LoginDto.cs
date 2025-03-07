using Store.Domain.Base;

namespace Store.Domain.Dtos
{
    public class LoginDto : ResponseBase
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
