using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Common
{
    public interface IUser
    {
        /// <summary>
        /// 用户id
        /// </summary>
        string Id { get;}
        /// <summary>
        /// 用户姓名
        /// </summary>
        string Name { get;}
        /// <summary>
        /// 是否登录认证成功
        /// </summary>
        /// <returns></returns>
        bool IsAuthenticated();
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        Task<string> GetTokenAsync();
    }
}
