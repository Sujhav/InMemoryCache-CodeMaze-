using Microsoft.Extensions.Caching.Memory;

namespace IN_MemoryCache
{
    public  class CacheEntryOptionFactory 
    {
        public static MemoryCacheEntryOptions GetCacheConfiguration()
        {
            var cacheOprions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(60))
                .SetPriority(CacheItemPriority.High);
            return cacheOprions;
        }
    }
}
