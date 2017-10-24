using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchoolPal.Toolkit.Caching.OutputCache.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.WebApi.OutputCache.Test
{
    [TestClass]
    public class ApiOutputCacheAttributeTest
    {
        [TestMethod]
        public void MyTestMethod()
        {
            ApiOutputCacheProviderFactory.SetDefaultProvider(new MemcachedApiOutputCacheProvider(new EnyimMemcached.EnyimMemcachedCache("caching/memcached", "api")));



        }
    }
}
