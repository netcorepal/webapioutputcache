using System;

namespace NetCorePal.WebApi.OutputCache
{
    /// <summary>
    /// 提供程序
    /// </summary>
    public abstract class ApiOutputCacheProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract object Get(string key);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="expiry"></param>
        public abstract void Set(string key, object data, DateTime expiry);
    }
}
