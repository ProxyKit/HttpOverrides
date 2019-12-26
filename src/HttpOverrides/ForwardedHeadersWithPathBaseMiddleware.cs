using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ProxyKit.HttpOverrides
{
    public class ForwardedHeadersWithPathBaseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<ForwardedHeadersWithPathBaseOptions> _options;

        public ForwardedHeadersWithPathBaseMiddleware(
            RequestDelegate next, 
            IOptions<ForwardedHeadersWithPathBaseOptions> options)
        {
            _next = next;
            _options = options;
        }

        public Task Invoke(HttpContext context)
        {
            var originalPathBase = context.Request.PathBase;

            if (_options.Value.ForwardedHeaders.HasFlag(ForwardedHeadersWithPathBase.XForwardedPathBase))
            {
                if (context.Request.Headers.TryGetValue(
                    _options.Value.ForwardedPathBaseHeaderName,
                    out var pathBase))
                {
                    context.Request.PathBase = new PathString(pathBase);
                    context.Request.Headers.Add(
                        _options.Value.OriginalPathBaseHeaderName,
                        originalPathBase.HasValue ? originalPathBase.Value : "/");
                    context.Request.Headers.Remove(_options.Value.ForwardedPathBaseHeaderName);
                }
            }

            return _next(context);
        }
    }
}
