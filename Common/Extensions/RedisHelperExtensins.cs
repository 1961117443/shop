using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Common.Extensions
{
    public static class RedisHelperExtensins
    {
        /// <summary>
        /// 一天秒数
        /// </summary>
        public static readonly int DAY_SECONDS= 60*60*24;
        /// <summary>
        /// 一小时秒数
        /// </summary>
        public static readonly int HOUR_SECONDS = 60*60;
        //
        // 摘要:
        //     缓存壳
        //
        // 参数:
        //   key:
        //     不含prefix前辍
        //
        //   timeoutSeconds:
        //     缓存秒数 默认 86400
        //
        //   getDataAsync:
        //     获取源数据的函数
        //
        // 类型参数:
        //   T:
        //     缓存类型
        //public static async Task<T> CacheShellAsync<T>(this RedisHelper redis, string key,Func<Task<T>> getDataAsync)
        //{
        //    return await RedisHelper.CacheShellAsync(key, DAYSECONDS, getDataAsync);
        //}
    }
}
