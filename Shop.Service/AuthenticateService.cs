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

        public string GetToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            if (jwtToken!=null)
            { 
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenManagement.Secret));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                jwtToken = new JwtSecurityToken(tokenManagement.Issuer, tokenManagement.Audience, jwtToken.Claims, expires: DateTime.Now.AddDays(tokenManagement.AccessExpiration), signingCredentials: credentials);
                return handler.WriteToken(jwtToken);
            }
            return string.Empty;
        }

        public bool IsAuthenticated(LoginViewModel loginViewModel, out KeyValuePair<string, string> token)
        {
            var user = this.userService.GetAsync(w => w.Code == loginViewModel.Username && w.Password == loginViewModel.Password).Result;
            if (user==null)
            {
                token = new KeyValuePair<string, string>();
                return false;
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.Sid,user.ID.ToString()),
                new Claim(ClaimTypes.Name,user.Name)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(tokenManagement.Issuer, tokenManagement.Audience, claims, 
                expires: DateTime.Now.AddDays(tokenManagement.AccessExpiration), 
                signingCredentials: credentials);
            var handler = new JwtSecurityTokenHandler();
            var access_token = handler.WriteToken(jwtToken);

            jwtToken = new JwtSecurityToken(tokenManagement.Issuer, tokenManagement.Audience, claims, 
                expires: DateTime.Now.AddDays(tokenManagement.RefreshExpiration), 
                signingCredentials: credentials);
            //token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            token = new KeyValuePair<string, string>(access_token, handler.WriteToken(jwtToken));
            return true;
        }

        //private KeyValuePair<string, string> CreateToken()
        //{
        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Sid,user.ID.ToString()),
        //        new Claim(ClaimTypes.Name,user.Name)
        //    };
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenManagement.Secret));
        //    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    var jwtToken = new JwtSecurityToken(tokenManagement.Issuer, tokenManagement.Audience, claims, expires: DateTime.Now.AddMinutes(tokenManagement.AccessExpiration), signingCredentials: credentials);
        //    var handler = new JwtSecurityTokenHandler();
        //    var token1 = handler.WriteToken(jwtToken);
        //    jwtToken = new JwtSecurityToken(tokenManagement.Issuer, tokenManagement.Audience, claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials);
        //    //token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        //    token = new KeyValuePair<string, string>(token1, handler.WriteToken(jwtToken));
        //}
    }
}
