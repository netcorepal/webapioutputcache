using System;

namespace NetCorePal.WebApi.OutputCache
{
    /// <summary>
    /// 提供程序
    /// </summary>
    public abstract class ApiOutputCacheProvider
    {
        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract object Get(string key);
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">缓存的key；注意：key可能包含空格</param>
        /// <param name="data"></param>
        /// <param name="expiry">过期时间</param>
        public abstract void Set(string key, object data, DateTime expiry);
    }
}
