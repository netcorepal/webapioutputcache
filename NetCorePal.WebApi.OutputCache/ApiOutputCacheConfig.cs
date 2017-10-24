using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.WebApi.OutputCache
{
    public sealed class ApiOutputCacheConfig
    {
        public static ApiOutputCacheProvider DefaultProvider { get; private set; } = new InternalApiOutputCacheProvider();


        public static void SetDefaultProvider(ApiOutputCacheProvider provider)
        {
            DefaultProvider = provider;
        }

        /// <summary>
        /// 全局自定义缓存key算法
        /// </summary>
        public static Func<HttpRequestMessage, string, string> GlobalVaryByCustomFunc { get; set; } = null;
    }
}
