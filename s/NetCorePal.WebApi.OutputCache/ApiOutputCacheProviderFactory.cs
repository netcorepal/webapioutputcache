using System;

namespace NetCorePal.WebApi.OutputCache
{
    /// <summary>
    /// 缓存提供程序
    /// </summary>
    public class ApiOutputCacheProviderFactory
    {
        /// <summary>
        /// 默认提供程序
        /// </summary>
        public static ApiOutputCacheProvider DefaultProvider { get; private set; } = new InternalApiOutputCacheProvider();
        /// <summary>
        /// 设置默认提供程序
        /// </summary>
        /// <param name="provider"></param>
        public static void SetDefaultProvider(ApiOutputCacheProvider provider)
        {
            if (provider == null) { throw new ArgumentNullException(nameof(provider)); }

            DefaultProvider = provider;
        }
    }
}
