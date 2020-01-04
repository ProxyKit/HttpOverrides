# ProxyKit HttpOverrides

[![NuGet][nuget badge]][nuget package] [![feedz.io](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fdh%2Foss-ci%2Fshield%2FProxyKit.HttpOverrides%2Flatest)](https://f.feedz.io/dh/oss-ci/packages/ProxyKit.HttpOverrides/latest/download)

## Introduction
Applications that are deployed behind a reverse proxy typically need to be
aware of that so they can generate correct URLs and paths when
responding to requests. That is, they look at `X-Forward-*` / `Forwarded`
headers and use their values accordingly.

In ASP.NET Core, this means using the `ForwardedHeaders` middleware in your
application. However, this middleware does not support `X-Forwarded-PathBase`
that tells upstream hosts what the path of the incoming request is. For example,
if you proxy `http://example.com/foo/` to `http://upstream-host/` the `/foo/`
part is lost and absolute URLs cannot be generated unless you configure your
application's `PathBase` directly using [`app.UsePathBase()`][app usepathbase].

However, the problems with [`app.UsePathBase()`][app usepathbase] are:

- You need to know the value at deployment time coupling knowledge of your
  reverse proxy configuration with your application.
- The value is used at start up time meaning if you want to change your reverse
  proxy routing configuration you have to restart your application.
- Your application can only support one path base at a time. If you want your
  reverse proxy to forward two or more routes to a single upstream host, you are
  out of luck.

Related issues and discussions:

- https://github.com/aspnet/AspNetCore/issues/5978
- https://github.com/aspnet/AspNetCore/issues/5898

This project extends `ForwardedHeaders` with support for `X-Forwarded-PathBase`
that allows the path base to be determined at runtime / per request basis.

## Using

Install the NuGet package:

```bash
dotnet package add ProxyKit.HttpOverrides
```

As this package _extends_ ASP.NET Cores `ForwardedHeadersMiddleware` use it
instead `UseForwardedHeaders()` in  your `Startup.ConfiguratApplication()`:

```csharp
var options = new ForwardedHeadersWithPathBaseOptions
{
   ForwardedHeaders = ForwardedHeadersWithPathBase.All
};
app.UseForwardedHeadersWithPathBase(options);
```

`ForwardedHeadersWithPathBaseOptions` extends `ForwardedHeadersOptions` with
additional properties that follows the same patterns of the base class:

- `ForwardedPathBaseHeaderName` - the PathBase header to look for defaulting to
  `X-Forwarded-PathBase`
- `OriginalPathBaseHeaderName` - the header to add that will contain the
  original path base if it has been applied, defaulting to
  `X-Original-PathBase`.

Please refer to the [`ForwaredHeadersMiddleware` documentation][forwarded headers middleware]
for correct usage of other properties (and note the security advisory!).

---
Questions to [@randompunter][twitter].

[nuget badge]: https://img.shields.io/nuget/v/ProxyKit.HttpOverrides.svg
[nuget package]: https://www.nuget.org/packages/ProxyKit.HttpOverrides
[app usepathbase]: https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.usepathbaseextensions.usepathbase?view=aspnetcore-3.1
[forwarded headers middleware]: https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-3.1
[twitter]: https://twitter.com/randompunter
