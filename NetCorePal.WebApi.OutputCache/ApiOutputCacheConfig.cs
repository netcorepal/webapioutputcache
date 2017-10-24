using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NetCorePal.WebApi.OutputCache
{
    /// <summary>
    /// config class
    /// </summary>
    public sealed class ApiOutputCacheConfig
    {
        /// <summary>
        /// Default provider
        /// </summary>
        public static ApiOutputCacheProvider DefaultProvider { get; private set; } = new InternalApiOutputCacheProvider();

        /// <summary>
        /// set default provider
        /// </summary>
        /// <param name="provider"></param>
        public static void SetDefaultProvider(ApiOutputCacheProvider provider)
        {
            DefaultProvider = provider;
        }

        /// <summary>
        /// Global VaryByCustom Function
        /// </summary>
        public static Func<HttpRequestMessage, string, string> GlobalVaryByCustomFunc { get; set; } = null;
    }
}
