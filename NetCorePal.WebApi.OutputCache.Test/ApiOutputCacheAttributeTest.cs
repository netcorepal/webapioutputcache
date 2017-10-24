using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetCorePal.WebApi.OutputCache;
using NetCorePal.WebApi.OutputCache.Memcached;

namespace SchoolPal.Toolkit.Caching.Tests
{
    [TestClass]
    public class ApiOutputCacheAttributeTest
    {
        [TestMethod]
        public void SetDefaultProviderTest()
        {
            ApiOutputCacheConfig.SetDefaultProvider(new MemcachedApiOutputCacheProvider("caching/memcached", "api"));
        }
    }
}
