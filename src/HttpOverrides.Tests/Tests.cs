using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProxyKit.HttpOverrides;
using Shouldly;
using Xunit;

namespace ProxyKit.XForwardedExtensions
{
    public class Tests
    {
        [Fact]
        public async Task Should_handle_forwarded_path_base()
        {
            var builder = new WebHostBuilder()
                .Configure(app =>
                {
                    var options = new ForwardedHeadersWithPathBaseOptions()
                    {
                        ForwardedHeaders = ForwardedHeadersWithPathBase.All
                    };
                    app.UseForwardedHeadersWithPathBase(options);
                    app.Run(async ctx =>
                    {
                        await ctx.Response.WriteAsync("ok");
                    });
                });
            var server = new TestServer(builder);

            var context = await server.SendAsync(c =>
            {
                c.Request.Headers["X-Forwarded-For"] = "11.111.111.11:9090";
                c.Request.Headers["X-Forwarded-Proto"] = "https";
                c.Request.Headers["X-Forwarded-PathBase"] = "/foo";
            });

            context.Request.Headers.ContainsKey("X-Original-PathBase").ShouldBeTrue();
            context.Request.PathBase.Value.ShouldBe("/foo");
            context.Request.Headers.ContainsKey("X-Forwarded-PathBase").ShouldBeFalse();
        }
    }
}
