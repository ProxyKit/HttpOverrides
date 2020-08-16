using Microsoft.AspNetCore.Builder;

namespace ProxyKit.HttpOverrides
{
    /// <summary>
    /// Default values related to <see cref="ForwardedHeadersWithPathBaseMiddleware"/> middleware
    /// </summary>
    /// <seealso cref="ForwardedHeadersWithPathBaseOptions"/>
    public static class ForwardedHeadersWithPathBaseDefaults
    {
        /// <summary>
        /// X-Forwarded-PathBase
        /// </summary>
        public static string XForwardedPathBaseHeaderName { get; } = "X-Forwarded-PathBase";

        /// <summary>
        /// X-Original-PathBase
        /// </summary>
        public static string XOriginalPathBaseHeaderName { get; } = "X-Original-PathBase";
    }
}
