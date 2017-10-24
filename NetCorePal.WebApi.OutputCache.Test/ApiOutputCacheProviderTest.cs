using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetCorePal.WebApi.OutputCache;
using NetCorePal.WebApi.OutputCache.Memcached;
using System;

namespace SchoolPal.Toolkit.Caching.Tests
{
    [TestClass]
    public class ApiOutputCacheProviderTest
    {
        [TestMethod]
        public void CachedData_Get_Set_Test()
        {

            ApiOutputCacheProvider provider = new InternalApiOutputCacheProvider();
            ProviderTest(provider);
            provider = new MemcachedApiOutputCacheProvider("caching/memcached", "api-test");
            ProviderTest(provider);
        }


        void ProviderTest(ApiOutputCacheProvider provider)
        {
            var cacheData = new CachedData
            {
                CreateAt = DateTime.Now,
                ContentData = new byte[] { 1, 2, 3, 4, 5 },
                Etag = "\"sdfsdfsdfsf\"",
                MediaType = "json"
            };

            string key = Guid.NewGuid().ToString();
            provider.Set(key, cacheData, DateTime.Now.AddSeconds(10));
            var data = provider.Get(key) as CachedData;
            Assert.IsNotNull(data);
            Assert.AreEqual(cacheData.Etag, data.Etag);
            Assert.AreEqual(cacheData.MediaType, data.MediaType);
            Assert.AreEqual(cacheData.CreateAt, data.CreateAt);
            Assert.IsNotNull(data.ContentData);
            Assert.AreEqual(cacheData.ContentData.Length, data.ContentData.Length);
            for (int i = 0; i < cacheData.ContentData.Length; i++)
            {
                Assert.AreEqual(cacheData.ContentData[i], data.ContentData[i]);
            }
        }
    }
}
