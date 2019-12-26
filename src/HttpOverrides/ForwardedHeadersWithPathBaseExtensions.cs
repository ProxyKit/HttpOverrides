using System;
using Microsoft.Extensions.Options;
using ProxyKit.HttpOverrides;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    public static class ForwardedHeadersWithPathBaseExtensions
    {
        private const string PathBaseForwardedHeaderAdded = "PathBaseForwardedHeaderAdded";

        /// <summary>
        ///     Forwards proxied X-Forwarded-PathBase headers onto current request.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseForwardedHeadersWithPathBase(this IApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.UseForwardedHeaders();

            // Copied from ForwardedHeadersExtensions.UseForwardedHeaders (I don't like it) . -DH
            // Don't add more than one instance of this middleware to the pipeline using the options from the DI container.
            // Doing so could cause a request to be processed multiple times and the ForwardLimit to be exceeded.
            if (!builder.Properties.ContainsKey(PathBaseForwardedHeaderAdded))
            {
                builder.Properties[PathBaseForwardedHeaderAdded] = true;
                builder.UseMiddleware<ForwardedHeadersWithPathBaseMiddleware>();
            }

            return builder;
        }

        /// <summary>
        ///     Forwards proxied X-Forwarded-PathBase headers onto current request.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="options">Enables the different forwarding options.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseForwardedHeadersWithPathBase(
            this IApplicationBuilder builder,
            ForwardedHeadersWithPathBaseOptions options)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            if (options == null) throw new ArgumentNullException(nameof(options));

            builder.UseForwardedHeaders(options);

            return builder.UseMiddleware<ForwardedHeadersWithPathBaseMiddleware>(Options.Create(options));
        }
    }
}