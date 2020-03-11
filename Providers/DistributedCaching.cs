using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClaimsDemo.Providers
{
    public static class DistributedCaching
    {
       
        public async static Task SetAsyncModel<Stream>(this IDistributedCache distributedCache, string key, Stream value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken)
        )
        {
            try
            {
                await distributedCache.SetAsync(key, value.ToByteArray(), options, token);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async static Task<Stream> GetAsyncModel<Stream>(this IDistributedCache distributedCache, string key, CancellationToken token = default(CancellationToken)) where Stream : class
        {
            try
            {
                var result = await distributedCache.GetAsync(key, token);
                return result.FromByteArray<Stream>();
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
