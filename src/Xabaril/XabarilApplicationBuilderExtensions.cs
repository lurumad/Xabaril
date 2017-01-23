using Microsoft.AspNetCore.Builder;
using System;
using Xabaril.UI;

namespace Xabaril
{
    public static class XabarilApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseXabarilUI(this IApplicationBuilder app)
        {
            return app.UseMiddleware<XabarilUIMiddleware>(
                new XabarilUIOptions());
        }

        public static IApplicationBuilder UseXabarilUI(this IApplicationBuilder app, Action<XabarilUIOptions> configureOptions)
        {
            var options = new XabarilUIOptions();

            configureOptions(options);

            return app.UseMiddleware<XabarilUIMiddleware>(
                options);
        }
    }
}
