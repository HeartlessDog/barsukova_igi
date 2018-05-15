using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
namespace Lab3IGI
{
    public class DbCacheMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _memoryCache;

        public DbCacheMiddleware(RequestDelegate next, IMemoryCache memCache)
        {
            this._next = next;
            this._memoryCache = memCache;
        }

        public async Task Invoke(HttpContext context, LibraryContext db)
        {
            string path = context.Request.Path.Value.ToLower();
            object model = null;
            switch (path)
            {
                case "/home/first":
                    model = db.Books.Last();
                    break;
                case "/home/second":
                    model = db.Readers.Last();
                    break;
                case "/home/third":
                    model = db.Issuances.Last();
                    break;
                case "/htmlpage.html":
                    model = db.Readers.Last();
                    break;
            }
            var modelTemp = model;
            if (!_memoryCache.TryGetValue(path, out modelTemp))
            {
                _memoryCache.Set(path, model,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)));
            }

            await _next.Invoke(context);
        }

    }

    public static class DbCacheExtensions
    {
        public static IApplicationBuilder UseLastElementCache(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DbCacheMiddleware>();
        }
    }
}
