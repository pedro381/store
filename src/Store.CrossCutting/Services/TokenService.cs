using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Store.Domain.Configurations;
using Store.Domain.Entities;

namespace Store.CrossCutting.Services
{
    public class TokenService(TokenSettings tokenSettings)
    {
        private readonly TokenSettings _tokenSettings = tokenSettings;

        public string GenerateToken(User user)
        {
            List<Claim> claims =
            [
                new Claim(ClaimTypes.Name, user.UserId.ToString()),
            ];

            foreach (var item in user.Roles.Select(x => x.Name))
                claims.Add(new Claim(ClaimTypes.Role, item));

            return GenerateToken(claims);
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            var key = Encoding.ASCII.GetBytes(_tokenSettings.SecretKey);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_tokenSettings.ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes(_tokenSettings.SecretKey);
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            JwtSecurityTokenHandler tokenHandler = new();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException(Contants.InvalidToken);

                return principal;
            }
            catch { return null; }
        }

        private static List<(int, string)> _refreshTokens = [];

        public static void SaveRefreshToken(int systemUserID, string refreshToken)
        {
            _refreshTokens = _refreshTokens.Where(x => x.Item1 != systemUserID).ToList();
            _refreshTokens.Add(item: new ValueTuple<int, string>(systemUserID, refreshToken));
        }

        public static bool AnyRefreshToken(int systemUserID, string refreshToken)
            => _refreshTokens.Any(x => x.Item1 == systemUserID && x.Item2 == refreshToken);
    }
}
