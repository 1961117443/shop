using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Cache
{
    /// <summary>
    /// Redis缓存接口
    /// </summary>
    [Obsolete("使用RedisHelper代替")]
    public interface IRedisCacheManager
    {
        /// <summary>
        /// 缓存是否能够使用
        /// </summary>
        bool EnableUse { get; }
        //获取 Reids 缓存值
        string Get(string key);

        //获取值，并序列化
        TEntity Get<TEntity>(string key);

        //保存
        void Set(string key, string value, TimeSpan? cacheTime = null);
        //保存
        void Set<TEntity>(string key, TEntity value, TimeSpan? cacheTime = null);

        //判断是否存在
        bool Exists(string key);

        //移除某一个缓存值
        void Remove(string key);

        //全部清除
        void Clear();
    }
}
