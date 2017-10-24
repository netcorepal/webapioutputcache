using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web.Http;

namespace NetCorePal.WebApi.OutputCache
{
    /// <summary>
    /// webapi 缓存设置类
    /// </summary>
    public class ApiOutputCacheAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 全局自定义缓存key算法
        /// </summary>
        public static Func<HttpRequestMessage, string, string> GlobalVaryByCustomFunc { get; set; } = null;
        /// <summary>
        /// 缓存时间，单位：秒
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// 缓存可以存储的位置
        /// </summary>
        public ApiCacheLocation Location { get; set; }

        /// <summary>
        /// 缓存key参数依赖设置，多个参数用 “;”分隔
        /// </summary>
        public string VaryByParam { get; set; } = "*";
        /// <summary>
        /// 自定义缓存依赖,多个参数用“;”分隔
        /// </summary>
        public string VaryByCustom { get; set; }
        /// <summary>
        /// 指定根据mediaType进行缓存,默认false;如果输出同时支持xml、json等多种数据格式，则需要设置为true
        /// </summary>
        public bool VaryByMediaType { get; set; } = false;

        private ApiOutputCacheProvider Provider { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ApiOutputCacheAttribute()
        {
            this.Provider = ApiOutputCacheProviderFactory.DefaultProvider;
        }

        const string ApiCachePropertiesKey = "sp:api:outputcache:key";
        private bool Private
        {
            get { return Location == ApiCacheLocation.Client || Location == ApiCacheLocation.ServerAndClient; }
        }

        private bool Public
        {
            get { return Location == ApiCacheLocation.Any || Location == ApiCacheLocation.Downstream; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            if (!HttpMethod.Get.Equals(request.Method))
            {
                return; //仅支持get请求
            }

            var key = GetCacheKey(actionContext);
            request.Properties[ApiCachePropertiesKey] = key;
            var cachedData = Provider.Get(key) as CachedData;
            if (cachedData == null)
            {
                return; //未命中
            }

            if (request.Headers.IfNoneMatch != null)
            {
                if (request.Headers.IfNoneMatch.Any(p => p.Tag == cachedData.Etag))
                {
                    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.NotModified);
                    SetCacheHeader(actionContext.Response, cachedData.CreateAt.AddSeconds(Duration));
                    return;
                }
            }
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = new ByteArrayContent(cachedData.ContentData);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(cachedData.MediaType);
            actionContext.Response = response;
            if (Location != ApiCacheLocation.Server)
            {
                SetCacheHeader(actionContext.Response, cachedData.CreateAt.AddSeconds(Duration));
                actionContext.Response.Headers.ETag = EntityTagHeaderValue.Parse(cachedData.Etag);
            }
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response != null && actionExecutedContext.Response.IsSuccessStatusCode)
            {
                var cacheData = new CachedData
                {
                    Etag = $"\"{Guid.NewGuid().ToString()}\"",
                    CreateAt = DateTime.Now
                };
                if (Location != ApiCacheLocation.Client && Location != ApiCacheLocation.Downstream)
                {
                    cacheData.ContentData = actionExecutedContext.Response.Content.ReadAsByteArrayAsync().Result;
                    cacheData.MediaType = actionExecutedContext.Response.Content.Headers.ContentType.MediaType;
                    string key;
                    if (!TryGetCacheKeyFromProperties(actionExecutedContext.Request, out key))
                    {
                        key = GetCacheKey(actionExecutedContext.ActionContext);
                    }
                    Provider.Set(key, cacheData, cacheData.CreateAt.AddSeconds(Duration));
                }
                if (Location != ApiCacheLocation.Server)
                {
                    SetCacheHeader(actionExecutedContext.Response, cacheData.CreateAt.AddSeconds(Duration));
                    actionExecutedContext.Response.Headers.ETag = EntityTagHeaderValue.Parse(cacheData.Etag);
                }
            }
        }


        void SetCacheHeader(HttpResponseMessage response, DateTime expirAt)
        {
            response.Headers.CacheControl = new CacheControlHeaderValue
            {
                MaxAge = expirAt - DateTime.Now,
                Private = this.Private,
                Public = this.Public
            };
        }

        string GetCacheKey(HttpActionContext actionContext)
        {
            StringBuilder sb = new StringBuilder("b1", actionContext.Request.RequestUri.AbsolutePath.Length + 2);
            sb.Append(actionContext.Request.RequestUri.AbsolutePath.ToLower());

            if (!"none".Equals(VaryByParam, StringComparison.OrdinalIgnoreCase))
            {
                NameValueCollection querys = actionContext.Request.RequestUri.ParseQueryString();
                if ("*".Equals(VaryByParam) || string.IsNullOrEmpty(VaryByParam))
                {
                    foreach (var item in querys.AllKeys)
                    {
                        sb.Append(item.ToLower());
                        sb.Append(querys[item]);
                    }
                }
                else
                {
                    var keys = VaryByParam.Split(';');
                    foreach (var item in keys)
                    {
                        if (querys.AllKeys.Contains(item, StringComparer.OrdinalIgnoreCase))
                        {
                            sb.Append(item.ToLower());
                            sb.Append(querys[item]);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(VaryByCustom))
            {
                var customs = VaryByParam.Split(';');

                foreach (var item in customs)
                {
                    if (GlobalVaryByCustomFunc != null)
                    {
                        sb.Append(item.ToLower());
                        sb.Append(GlobalVaryByCustomFunc(actionContext.Request, item));
                    }
                    else
                    {
                        throw new Exception("要使用VaryByCustom，必须先定义VaryByCustomFunc或GlobalVaryByCustomFunc");
                    }
                }
            }

            if (VaryByMediaType)
            {
                var negotiator = actionContext.Request.GetConfiguration().Services.GetContentNegotiator();
                var negotiateResult = negotiator.Negotiate(actionContext.ActionDescriptor.ReturnType, actionContext.Request, actionContext.Request.GetConfiguration().Formatters);
                sb.Append(negotiateResult.MediaType);
            }
            return sb.ToString();
        }

        

        bool TryGetCacheKeyFromProperties(HttpRequestMessage request, out string key)
        {
            if (!request.Properties.ContainsKey(ApiCachePropertiesKey))
            {
                key = null;
                return false;
            }
            key = request.Properties[ApiCachePropertiesKey]?.ToString();
            return !string.IsNullOrEmpty(key);
        }
    }
}
