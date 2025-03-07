using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Store.CrossCutting.Services;
using Store.Domain.Configurations;
using Store.Domain.Dtos;
using Store.Domain.Entities;

namespace Store.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController(TokenSettings tokenSettings) : Controller
    {
        private readonly TokenSettings _tokenSettings = tokenSettings;

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<LoginDto>> Login()
        {
            //TODO: Get User by login and password
            var response = new User
            {
                UserId = 1,
                Name = "Pedro",
                Document = "84903334082",
                Roles = [new Role { Name = "Admin"}]
            };

            TokenService tokenService = new(_tokenSettings);

            LoginDto token = new()
            {
                Token = tokenService.GenerateToken(response),
                RefreshToken = TokenService.GenerateRefreshToken()
            };

            TokenService.SaveRefreshToken(response.UserId, token.RefreshToken);

            await Task.CompletedTask;

            return Ok(token);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<LoginDto>> Refresh([FromBody] LoginDto model)
        {
            TokenService tokenService = new(_tokenSettings);

            var principal = tokenService.GetPrincipalFromExpiredToken(model.Token);
            if (principal == null)
                return Unauthorized();

            var systemUserID = int.Parse(principal.Identity!.Name!);

            if (!TokenService.AnyRefreshToken(systemUserID, model.RefreshToken))
                throw new SecurityTokenException("Invalid refresh token");

            LoginDto token = new()
            {
                Token = tokenService.GenerateToken(principal.Claims),
                RefreshToken = TokenService.GenerateRefreshToken()
            };

            TokenService.SaveRefreshToken(systemUserID, token.RefreshToken);

            await Task.CompletedTask;

            return Ok(token);
        }
    }
}
