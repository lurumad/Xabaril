using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xabaril;
using Xabaril.Core;
using Xabaril.Core.Activators;
using Xabaril.Store;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class XabarilServiceCollectionExtensions
    {
        public static IXabarilBuilder AddXabaril(this IServiceCollection serviceCollection)
        {
            var builder = new XabarilBuilder(serviceCollection);

            builder.Services.TryAdd(new ServiceCollection()
                .AddOutOfBoxServices());

            return builder;
        }

        static IServiceCollection AddOutOfBoxServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddOptions();

            serviceCollection.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            serviceCollection.TryAdd(new ServiceCollection()
                .AddScoped<SingleInstanceFactory>(provider=>
                {
                    return new SingleInstanceFactory((type) => provider.GetRequiredService(type));
                })
                .AddScoped<IFeaturesService,FeaturesService>()
                .AddScoped<IFeaturesStore,NoFeaturesStore>()
                .AddScoped<IUserProvider, HttpContextUserProvider>()
                .AddScoped<IRoleProvider, HttpContextRoleProvider>()
                .AddScoped<IGeoLocationProvider,NoGeoLocationProvider>()
                .AddScoped<IRuntimeParameterAccessor, StoreRuntimeParameterAccesor>());

            serviceCollection.TryAdd(new ServiceCollection()
                .AddScoped<UTCActivator>()
                .AddScoped<LocationActivator>()
                .AddScoped<RolloutHeaderValueActivator>()
                .AddScoped<RolloutUsernameActivator>()
                .AddScoped<UserActivator>()
                .AddScoped<RoleActivator>());

            return serviceCollection;
        }
    }
}
