using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Xabaril;
using Xabaril.Core;
using Xabaril.InMemoryStore;
using Xabaril.Store;

namespace UnitTests.Xabaril.Core
{
    internal class FeatureServiceBuilder
    {
        private InMemoryFeaturesStore _featuresStore;
        private FeatureConfigurer _featureConfigurer;
        private IFeatureActivator _featureActivator;
        private XabarilOptions _xabarilOptions;

        public FeaturesService Build()
        {
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<XabarilModule>();

            if (_featureConfigurer != null)
            {
                _featuresStore.PersistConfiguratioAsync(
                    new List<FeatureConfigurer> { _featureConfigurer });
            }
            if (_xabarilOptions != null)
            {
                Options.Create<XabarilOptions>(_xabarilOptions);
            }

            return new FeaturesService(
                Options.Create<XabarilOptions>(_xabarilOptions),
                logger,
                _featuresStore,
                t => _featureActivator);
        }

        public FeatureServiceBuilder WithOptions(XabarilOptions options)
        {
            _xabarilOptions = options;

            return this;
        }

        public FeatureServiceBuilder WithInMemoryStore()
        {
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<XabarilModule>();

            var store = new InMemoryFeaturesStore(
                logger,
                new MemoryCache(new MemoryCacheOptions()));

            _featuresStore = store;

            return this;
        }

        public FeatureServiceBuilder WithEnabledFeature(string featureName)
        {
            _featureActivator = new AlwaysEnabledFeatureActivator();

            _featureConfigurer = new FeatureConfigurer(featureName)
                .WithActivator<AlwaysEnabledFeatureActivator>(@params =>
                {

                });

            return this;
        }

        public FeatureServiceBuilder WithDisabledFeature(string featureName)
        {
            _featureActivator = new AlwaysDisabledFeatureActivator();

            _featureConfigurer = new FeatureConfigurer(featureName)
                .WithActivator<AlwaysDisabledFeatureActivator>(@params =>
                {

                });

            return this;
        }
    }
}
