using global::Xabaril;
using global::Xabaril.Core.Activators;
using global::Xabaril.InMemoryStore;
using global::Xabaril.Store;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace UnitTests.Xabaril.InMemoryStore
{
    public class xabaril_builder_extension_should
    {
        [Fact]
        public void configure_required_services()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();

            IXabarilBuilder xabarilBuilder = new XabarilBuilder(serviceCollection);

            xabarilBuilder = xabarilBuilder.AddXabarilInMemoryStore();

            Assert.NotNull(serviceCollection);

            var provider = serviceCollection.BuildServiceProvider();

            var featuresStore = provider.GetRequiredService<IFeaturesStore>();

            Assert.NotNull(featuresStore);
            Assert.Equal(typeof(InMemoryFeaturesStore), featuresStore.GetType());
        }

        [Fact]
        public void AddXabarilInMemoryStore_use_features_configurer()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();

            IXabarilBuilder xabarilBuilder = new XabarilBuilder(serviceCollection);

            xabarilBuilder = xabarilBuilder.AddXabarilInMemoryStore(options=>
            {
                options.AddFeature("simple")
                    .WithActivator<UserActivator>(@params =>
                    {
                        @params.Add("USER", "some_user");
                    });
            });

            Assert.NotNull(serviceCollection);

            var provider = serviceCollection.BuildServiceProvider();

            var featuresStore = provider.GetRequiredService<IFeaturesStore>();

            Assert.NotNull(featuresStore);
            Assert.Equal(typeof(InMemoryFeaturesStore), featuresStore.GetType());
        }
    }
}
