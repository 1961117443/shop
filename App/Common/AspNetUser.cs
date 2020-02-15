using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Shop.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Shop.Common.Extensions;

namespace App.Common
{
    public class AspNetUser : IUser
    {
        private readonly IHttpContextAccessor _accessor;

        private string id;
        public string Id
        {
            get
            {
                if (id.IsEmpty())
                {
                    this.id = GetUserInfoFromTokenAsync(JwtClaimTypes.Subject).Result.FirstOrDefault(); 
                }
                if (id.IsEmpty())
                {
                    this.id = GetUserInfoFromTokenAsync(ClaimTypes.Sid).Result.FirstOrDefault();
                }
                return id;
            }
        }
        private string name;
        public string Name
        {
            get
            {
                if (name.IsEmpty())
                {
                    this.name = this._accessor.HttpContext.User.Identity.Name;
                }
                return this.name;
            }
        }

        public AspNetUser(IHttpContextAccessor httpContextAccessor)
        {
            this._accessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetTokenAsync()
        {
            string token = await this._accessor.HttpContext.GetTokenAsync("access_token");
            if (token.IsEmpty())
            {
                token= _accessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            }
            if (token.IsEmpty())
            { 
                foreach (var item in _accessor.HttpContext.Request.Query.Where(w => w.Key.Contains("token")))
                {
                    if (!item.Value.ToString().IsEmpty())
                    {
                        token = item.Value.ToString();
                    }
                }
            }
            return token;
        }

        /// <summary>
        /// 根据ClaimType获取用户信息
        /// </summary>
        /// <param name="ClaimType"></param>
        /// <returns></returns>
        public async Task<List<string>> GetUserInfoFromTokenAsync(string ClaimType)
        {
            var claims = GetClaimsIdentity();
            if (claims==null)
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                string token = await GetTokenAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(token);
                    claims = jwtToken.Claims;
                }
            }
            if (claims!=null)
            { 
                return (from item in claims
                        where item.Type == ClaimType
                        select item.Value).ToList();
            }
            else
            {
                return new List<string>() { };
            }
        }

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _accessor.HttpContext.User.Claims;
        }

        public string GetClaimValueByType(string ClaimType)
        {
            return (from item in GetClaimsIdentity()
                    where item.Type == ClaimType
                    select item.Value).FirstOrDefault();

        }

        public bool IsAuthenticated()
        {
            return this._accessor.HttpContext.User.Identity.IsAuthenticated;
        }
    }
}
