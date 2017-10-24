using System;
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("NetCorePal.WebApi.OutputCache.Test")]
namespace NetCorePal.WebApi.OutputCache
{
    [Serializable]
    class CachedData
    {
        public string Etag { get; set; }

        public byte[] ContentData { get; set; }

        public string MediaType { get; set; }

        public DateTime CreateAt { get; set; }
    }
}
