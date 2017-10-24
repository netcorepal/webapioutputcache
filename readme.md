# NetCorePal.WebApi.OutputCache
OutputCache for ASP.NET WebApi

支持httpget请求的缓存

支持设置VaryByParam

支持设置VaryByMediaType

支持设置自定义VaryByCustom

支持设置缓存位置Location

支持memcached提供程序

## Install

```
Install-Package NetCorePal.WebApi.OutputCache
```

## Demo

#### ApiOutputCacheAttribute
```
    
    public class UserController : ApiController
    {
        [ApiOutputCache(Duration = 100, Location = ApiCacheLocation.Any, VaryByParam = "none")]
        public DateTime GetTime()
        {
            return DateTime.Now;
        }
    }
```
#### VaryByCustom
```
    Set GlobalVaryByCustomFunc In Global.asax.cs

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ApiOutputCacheConfig.GlobalVaryByCustomFunc = (request, custom) =>
            {
                if(custom=="userid")
                return  GetUserId();
            };
        }
    }
```

# NetCorePal.WebApi.OutputCache.Memcached

using  EnyimMemcached

## Install

```
Install-Package NetCorePal.WebApi.OutputCache.Memcached
```
## Init provider
```
    Global.asax.cs

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ApiOutputCacheConfig.SetDefaultProvider(new MemcachedApiOutputCacheProvider("memcached", "api"));
            //your code
        }
    }
```


#### config

configSections节添加memcached、globalOutputCacheSettings
```
  <configSections>
    <section name="memcached" type="Enyim.Caching.Configuration.MemcachedClientSection, Enyim.Caching" />
  </configSections>
```

memcached config for EnyimMemcached[see](https://github.com/enyim/EnyimMemcached/wiki/MemcachedClient-Configuration)
```
  <memcached protocol="Binary">
    <servers>
      <add address="127.0.0.1" port="11211" />
    </servers>
    <socketPool maxPoolSize="50" />
    <!--<locator type="MemcachedSessionProvider.SessionNodeLocator, MemcachedSessionProvider" />-->
    <!--<authentication type="Enyim.Caching.Memcached.PlainTextAuthenticator, Enyim.Caching" userName="" password="" zone="" />-->
  </memcached>
```