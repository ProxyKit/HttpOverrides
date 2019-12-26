using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;

namespace ProxyKit.HttpOverrides
{
    public class ForwardedHeadersWithPathBaseOptions : ForwardedHeadersOptions
    {
        private ForwardedHeadersWithPathBase _forwardedHeadersWithPathBase;

        /// <summary>
        /// Use this header instead of <see cref="ForwardedHeadersWithPathBaseDefaults.XForwardedPathBaseHeaderName"/>
        /// </summary>
        /// <seealso cref="ForwardedHeadersWithPathBaseDefaults"/>
        public string ForwardedPathBaseHeaderName { get; set; } = ForwardedHeadersWithPathBaseDefaults.XForwardedPathBaseHeaderName;

        /// <summary>
        /// Use this header instead of <see cref="ForwardedHeadersWithPathBaseDefaults.XOriginalPathBaseHeaderName"/>
        /// </summary>
        /// <seealso cref="ForwardedHeadersWithPathBaseDefaults"/>
        public string OriginalPathBaseHeaderName { get; set; } = ForwardedHeadersWithPathBaseDefaults.XOriginalPathBaseHeaderName;

        public new ForwardedHeadersWithPathBase ForwardedHeaders
        {
            get => _forwardedHeadersWithPathBase;
            set
            {
                _forwardedHeadersWithPathBase = value;
                var x = _forwardedHeadersWithPathBase & ~ForwardedHeadersWithPathBase.XForwardedPathBase;
                base.ForwardedHeaders = (ForwardedHeaders)x;
            }
        }
    }
}
