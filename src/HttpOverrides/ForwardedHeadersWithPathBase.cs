using System;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    [Flags]
    public enum ForwardedHeadersWithPathBase
    {
        None = 0,
        XForwardedFor = 1 << 0,
        XForwardedHost = 1 << 1,
        XForwardedProto = 1 << 2,
        XForwardedPathBase = 1 << 3,
        All = XForwardedFor | XForwardedHost | XForwardedProto | XForwardedPathBase
    }
}