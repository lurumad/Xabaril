using FluentAssertions;
using global::Xabaril;
using global::Xabaril.Core.Activators;
using global::Xabaril.InMemoryStore;
using global::Xabaril.Store;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Xabaril.InMemoryStore
{
    public class inmemory_feature_store_should
    {
        string featureName = "sample_feature";

        [Fact]
        public async Task find_feature_if_exist()
        {
            var store = new FeatureStoreBuilder()
                .WithFeatureConfigurer(new FeatureConfigurer(featureName)
                    .WithActivator<UserActivator>(@params =>
                    {
                        @params.Add("USER", "uzorrilla");
                    })).Build();

            (await store.FindFeatureAsync(featureName)).Name.Should().Be(featureName);
        }

        [Fact]
        public async Task return_null_feature_if_not_exist()
        {
            var store = new FeatureStoreBuilder()
                .Build();

            var feature = await store.FindFeatureAsync("non_existing_feature");

            (await store.FindFeatureAsync("non_existing_feature")).Should().BeNull();
        }

        [Fact]
        public async Task find_parameter_if_exist()
        {
            var store = new FeatureStoreBuilder()
                .WithFeatureConfigurer(new FeatureConfigurer(featureName)
                    .WithActivator<UserActivator>(@params =>
                    {
                        @params.Add("USER", "uzorrilla");
                    })).Build();

            (await store.FindParameterAsync("USER", featureName, typeof(UserActivator).FullName))
                .Should().NotBeNull();

            (await store.FindParameterAsync("USER", featureName, typeof(UserActivator).FullName)).Name
                .Should().Be("USER");

            (await store.FindParameterAsync("USER", featureName, typeof(UserActivator).FullName)).FeatureName
                .Should().Be(featureName);

            (await store.FindParameterAsync("USER", featureName, typeof(UserActivator).FullName)).ActivatorType
                .Should().Be(typeof(UserActivator).FullName);

            (await store.FindParameterAsync("USER", featureName, typeof(UserActivator).FullName)).Value
                .Should().Be("uzorrilla");
        }

        [Fact]
        public async Task return_null_parameter_if_not_exist()
        {
            var store = new FeatureStoreBuilder()
                .WithFeatureConfigurer(new FeatureConfigurer(featureName)
                    .WithActivator<UserActivator>(@params =>
                    {
                        @params.Add("USER", "uzorrilla");
                    })).Build();

            (await store.FindParameterAsync("non_existing_parameter", featureName, typeof(UserActivator).FullName))
                .Should().BeNull();
        }

        private class FeatureStoreBuilder
        {
            private FeatureConfigurer _configurer;

            public IFeaturesStore Build()
            {
                var loggerFactory = new LoggerFactory();
                var logger = loggerFactory.CreateLogger<XabarilModule>();

                var store = new InMemoryFeaturesStore(logger,
                    new MemoryCache(new MemoryCacheOptions()));

                if (_configurer != null)
                {
                    store.PersistConfiguratioAsync(new List<FeatureConfigurer>() { _configurer });
                }

                return store;
            }

            public FeatureStoreBuilder WithFeatureConfigurer(FeatureConfigurer configurer)
            {
                _configurer = configurer;

                return this;
            }

        }
    }
}
