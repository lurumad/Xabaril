using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using Xabaril;
using Xabaril.InMemoryStore;
using Xabaril.Store;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class XabarilBuilderExtensions
    {
        public static IXabarilBuilder AddXabarilInMemoryStore(this IXabarilBuilder xabarilBuilder)
        {
            return AddXabarilInMemoryStore(xabarilBuilder, options => { });
        }

        public static IXabarilBuilder AddXabarilInMemoryStore(this IXabarilBuilder xabarilBuilder, Action<InMemoryOptions> configure)
        {
            var options = new InMemoryOptions();

            var setup = configure ?? (opts => { });

            setup(options);

            xabarilBuilder
                .Services
                .AddMemoryCache()
                .AddSingleton<IFeaturesStore, InMemoryFeaturesStore>(provider =>
                {
                    var logger = provider.GetService<ILogger<XabarilModule>>();
                    var cache = provider.GetService<IMemoryCache>();

                    var store = new InMemoryFeaturesStore(logger, cache);

                    if (options.FeatureConfiguration != null)
                    {
                        store.PersistConfiguratioAsync(options.FeatureConfiguration);
                    }

                    return store;
                });
            
            return xabarilBuilder;
        }
    }
}
