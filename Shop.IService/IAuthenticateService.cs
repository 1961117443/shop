using Shop.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shop.IService
{
    /// <summary>
    /// 认证服务
    /// </summary>
    public interface IAuthenticateService
    {
        /// <summary>
        /// 登录成功并颁发token
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        bool IsAuthenticated(LoginViewModel loginViewModel, out KeyValuePair<string,string> token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        string GetToken(string token);
    }
}
