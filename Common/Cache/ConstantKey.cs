using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Cache
{
    /// <summary>
    /// 缓存的主键
    /// </summary>
    public class ConstantKey
    {
        #region 权限相关 PERMISSION
        /// <summary>
        /// 所有接口权限
        /// </summary>
        public static readonly string PERMISSION = "PERMISSION";
        /// <summary>
        /// 角色权限：占位符是用户id
        /// </summary>
        public static readonly string PERMISSION_ROLE_PERMISSION = "PERMISSION:ROLE_PERMISSION:{0}";
        /// <summary>
        /// 用户角色：占位符是用户id
        /// </summary>
        public static readonly string PERMISSION_USER_ROLE = "PERMISSION:USER_ROLE:{0}";
        #endregion

        /// <summary>
        /// 组合缓存主键
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Create(string cacheKey, params string[] value)
        {
            return string.Format(cacheKey, value);
        }
    }
}
