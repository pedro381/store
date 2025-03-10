﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Store.Api.Controllers;
using Store.Domain.Configurations;
using Store.Domain.Dtos;

namespace Store.Test.Controllers
{
    public class LoginControllerTest
    {
        private readonly TokenSettings _tokenSettings;
        private readonly LoginController _controller;

        public LoginControllerTest()
        {
            _tokenSettings = new TokenSettings
            {
                SecretKey = "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",
                ExpirationHours = 226
            };
            _controller = new LoginController(_tokenSettings);
        }

        [Fact]
        public async Task Login_ReturnsOkResultWithLoginDto()
        {
            var result = await _controller.Login();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var loginDto = Assert.IsType<LoginDto>(okResult.Value);
            Assert.False(string.IsNullOrEmpty(loginDto.Token));
            Assert.False(string.IsNullOrEmpty(loginDto.RefreshToken));
        }

        [Fact]
        public async Task Refresh_InvalidToken_ReturnsUnauthorized()
        {
            var model = new LoginDto { Token = "invalid", RefreshToken = "any" };
            var result = await _controller.Refresh(model);
            Assert.IsType<UnauthorizedResult>(result.Result);
        }

        [Fact]
        public async Task Refresh_InvalidRefreshToken_ThrowsSecurityTokenException()
        {
            var loginResult = await _controller.Login();
            var okResult = Assert.IsType<OkObjectResult>(loginResult.Result);
            var loginDto = Assert.IsType<LoginDto>(okResult.Value);
            var model = new LoginDto { Token = loginDto.Token, RefreshToken = "invalid_refresh" };
            await Assert.ThrowsAsync<SecurityTokenException>(async () => await _controller.Refresh(model));
        }

        [Fact]
        public async Task Refresh_Valid_ReturnsOkResultWithNewLoginDto()
        {
            var loginResult = await _controller.Login();
            var okResult = Assert.IsType<OkObjectResult>(loginResult.Result);
            var loginDto = Assert.IsType<LoginDto>(okResult.Value);
            Thread.Sleep(1000);
            var refreshResult = await _controller.Refresh(loginDto);
            var refreshOkResult = Assert.IsType<OkObjectResult>(refreshResult.Result);
            var newLoginDto = Assert.IsType<LoginDto>(refreshOkResult.Value);
            Assert.False(string.IsNullOrEmpty(newLoginDto.Token));
            Assert.False(string.IsNullOrEmpty(newLoginDto.RefreshToken));
            Assert.NotEqual(loginDto.Token, newLoginDto.Token);
        }
    }
}