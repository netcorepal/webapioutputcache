using Enyim.Caching;
using Enyim.Caching.Memcached;
using NetCorePal.WebApi.OutputCache;
using System;
using System.Collections.Specialized;

namespace NetCorePal.WebApi.OutputCache.Memcached
{
    /// <summary>
    /// memcache provider
    /// </summary>
    public class MemcachedApiOutputCacheProvider : ApiOutputCacheProvider
    {
        IMemcachedClient client;
        private string prefix = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionName">memcached sectionName</param>
        /// <param name="prefix">cache key prefix</param>
        public MemcachedApiOutputCacheProvider(string sectionName, string prefix)
        {
            this.client = new MemcachedClient(sectionName);
            this.prefix = prefix;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override object Get(string key)
        {
            return client.Get(GetCacheKey(key));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="expiry"></param>
        public override void Set(string key, object data, DateTime expiry)
        {
            if (key == null) { throw new ArgumentNullException("key"); }
            if (data == null) { throw new ArgumentNullException("value"); }
            var cacheKey = GetCacheKey(key);
            var r = client.Store(StoreMode.Set, cacheKey, data, GetExpiry(expiry));
        }

        #region 私有方法

        private string GetCacheKey(string key)
        {
            return this.prefix + System.Web.HttpUtility.UrlEncode(key);
        }

        private TimeSpan GetExpiry(DateTime expiryAt)
        {
            return expiryAt - DateTime.Now;
        }
        #endregion
    }
}
