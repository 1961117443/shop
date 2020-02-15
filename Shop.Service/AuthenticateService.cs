using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shop.IService;
using Shop.IService.MetaServices;
using Shop.ViewModel;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shop.Service
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IUserService userService;
        private readonly TokenManagement tokenManagement;

        public AuthenticateService(IUserService userService, IOptions<TokenManagement> options)
        {
            this.userService = userService;
            this.tokenManagement = options.Value;
        }
        public bool IsAuthenticated(LoginViewModel loginViewModel, out string token)
        {
            token = string.Empty;
            var user = this.userService.GetAsync(w => w.Code == loginViewModel.Username && w.Password == loginViewModel.Password).Result;
            if (user==null)
            {
                return false;
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.Sid,user.ID.ToString()),
                new Claim(ClaimTypes.Name,user.Name)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(tokenManagement.Issuer, tokenManagement.Audience, claims, expires: DateTime.Now.AddMinutes(tokenManagement.AccessExpiration), signingCredentials: credentials);
            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return true;
        }
    }
}
