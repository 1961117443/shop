using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Common;
using Shop.IService;
using Shop.ViewModel;

namespace App.Controllers
{
    /// <summary>
    /// 授权认证
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticateService authenticateService;
        private readonly IUser user;

        public AuthenticationController(IAuthenticateService authenticateService,IUser user)
        {
            this.authenticateService = authenticateService;
            this.user = user;
        }

        /// <summary>
        /// 用户登录 颁发token jwt
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [HttpPost("/api/user/login")]
        [AllowAnonymous]
        public IActionResult RequestToken([FromBody]LoginViewModel loginViewModel)
        {
            AjaxResultModel<object> ajaxResult = new AjaxResultModel<object>();
            if (ModelState.IsValid)
            {
                if (loginViewModel.Username == "string")
                {
                    loginViewModel.Username = "admin";
                    loginViewModel.Password = "admin";
                }
                //Dictionary<string, string> tokens = new Dictionary<string, string>();
                if (this.authenticateService.IsAuthenticated(loginViewModel, out KeyValuePair<string, string> tokens))
                {
                    ajaxResult.data = new { token=tokens.Key, refresh_token=tokens.Value };
                    return Ok(ajaxResult);
                }
            }
            ajaxResult.data = "账号或者密码错误，请重新输入！";
            return BadRequest(ajaxResult);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/user/info")]
        [Authorize]
        public IActionResult GetUserInfo()
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            AjaxResultModel<UserViewModel> ajaxResult = new AjaxResultModel<UserViewModel>();
            ajaxResult.data = new UserViewModel()
            {
                Roles = new string[] { "admin" },
                Introduction = "I am a super administrator",
                Avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif",
                Name = user.Name
            };

            return Ok(ajaxResult);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpPost("/api/user/logout")]
        [AllowAnonymous]
        public IActionResult Logout()
        {
            AjaxResultModel<string> ajaxResult = new AjaxResultModel<string>();             
            ajaxResult.data = "success";
            return Ok(ajaxResult);
        }

        /// <summary>
        /// 根据refresh_token 获取新的token
        /// </summary>
        /// <param name="token">旧的token</param>
        /// <returns></returns>
        [HttpGet("/api/user/refresh-token")]
        public IActionResult RefreshToken(string token)
        {
            AjaxResultModel<string> ajaxResult = new AjaxResultModel<string>();
            ajaxResult.data = authenticateService.GetToken(token);
            return Ok(ajaxResult);
        }
    }
}