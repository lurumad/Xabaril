using System;
using System.Linq;
using System.Reflection;
using Xabaril;
using Xabaril.Core;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class XabarilBuilderExtensions
    {
        public static IXabarilBuilder AddXabarilOptions(this IXabarilBuilder builder, Action<XabarilOptions> setup)
        {
            if (setup == null)
            {
                throw new ArgumentNullException(nameof(setup));
            }

            builder.Services
                .Configure<XabarilOptions>(setup);

            return builder;
        }

        public static IXabarilBuilder AddGeoLocationProvider<TGeoLocationProvider>(this IXabarilBuilder builder)
            where TGeoLocationProvider : class, IGeoLocationProvider
        {
            builder.Services
                .AddScoped<IGeoLocationProvider, TGeoLocationProvider>();

            return builder;
        }

        public static IXabarilBuilder AddUserProvider<TUserProvider>(this IXabarilBuilder builder)
            where TUserProvider : class, IUserProvider
        {
            builder.Services
                .AddScoped<IUserProvider, TUserProvider>();

            return builder;
        }

        public static IXabarilBuilder AddRoleProvider<TRoleProvider>(this IXabarilBuilder builder)
            where TRoleProvider : class, IRoleProvider
        {
            builder.Services
                .AddScoped<IRoleProvider, TRoleProvider>();
            
            return builder;
        }

        public static IXabarilBuilder AddCustomActivators(this IXabarilBuilder builder, Assembly assembly)
        {
            if (assembly != null)
            {
                var activators = assembly.GetTypes()
                    .Where(type => typeof(IFeatureActivator).GetTypeInfo().IsAssignableFrom(type));

                foreach (var item in activators)
                {
                    builder.Services
                        .AddTransient(item);
                }
            }

            return builder;
        }
    }
}
