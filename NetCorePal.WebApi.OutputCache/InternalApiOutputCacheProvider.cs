using System;

namespace NetCorePal.WebApi.OutputCache
{
    class InternalApiOutputCacheProvider : ApiOutputCacheProvider
    {
        public override object Get(string key)
        {
            return System.Web.HttpRuntime.Cache.Get(key);
        }

        public override void Set(string key, object data, DateTime expiry)
        {
            System.Web.HttpRuntime.Cache.Insert(key, data, null, expiry, System.Web.Caching.Cache.NoSlidingExpiration);
        }
    }
}
