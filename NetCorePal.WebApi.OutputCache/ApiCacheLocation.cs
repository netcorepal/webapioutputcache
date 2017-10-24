namespace NetCorePal.WebApi.OutputCache
{
    /// <summary>
    /// cache location
    /// </summary>
    public enum ApiCacheLocation
    {
        /// <summary>
        /// 缓存在服务器端、反向代理、浏览器
        /// 共享缓存
        ///     The output cache can be located on the browser client (where the request originated),
        ///     on a proxy server (or any other server) participating in the request, or on the
        ///     server where the request was processed. This value corresponds to the System.Web.HttpCacheability.Public
        ///     enumeration value.
        /// </summary>
        Any = 0,
        /// <summary>
        /// 缓存在浏览器；
        /// 私有缓存；
        /// 服务器端不缓存
        ///     The output cache is located on the browser client where the request originated.
        ///     This value corresponds to the System.Web.HttpCacheability.Private enumeration
        ///     value.
        /// </summary>
        Client = 1,
        /// <summary>
        /// 缓存在反向代理、浏览器
        /// 共享缓存
        /// 服务器端不缓存
        ///     The output cache can be stored in any HTTP 1.1 cache-capable devices other than
        ///     the origin server. This includes proxy servers and the client that made the request.
        /// </summary>
        Downstream = 2,
        /// <summary>
        /// 仅缓存在服务器端
        ///     The output cache is located on the Web server where the request was processed.
        ///     This value corresponds to the System.Web.HttpCacheability.Server enumeration
        ///     value.
        /// </summary>
        Server = 3,
        /// <summary>
        /// 缓存在服务器端、浏览器
        /// 私有缓存
        ///     The output cache can be stored only at the origin server or at the requesting
        ///     client. Proxy servers are not allowed to cache the response. This value corresponds
        ///     to the combination of the System.Web.HttpCacheability.Private and System.Web.HttpCacheability.Server
        ///     enumeration values.
        /// </summary>
        ServerAndClient = 4
    }
}
