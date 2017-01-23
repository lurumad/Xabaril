using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xabaril.Store;

namespace Xabaril.InMemoryStore
{
    public class InMemoryFeaturesStore : IFeaturesStore
    {
        const string FEATURE_CACHE_KEY_FORMAT = "XABARIL_FEATURE_{0}";
        const string CONFIGURATION_CACHE_KEY_FORMAT = "XABARIL_FEATURE_PARAMETERS_{0}";

        private readonly ILogger<XabarilModule> _logger;
        private readonly IMemoryCache _memoryCache;

        public InMemoryFeaturesStore(ILogger<XabarilModule> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public Task<Feature> FindFeatureAsync(string featureName)
        {
            var entry = _memoryCache.Get(string.Format(FEATURE_CACHE_KEY_FORMAT, featureName));

            if (entry != null)
            {
                return Task.FromResult<Feature>((Feature)entry);
            }
            else
            {
                _logger.LogWarning($"The feature with name {featureName} is not on InMemoryFeaturesStore");

                return Task.FromResult<Feature>(null);
            }
        }

        public Task<IEnumerable<Type>> FindFeatureActivatorsTypesAsync(string featureName)
        {
            Dictionary<Type, Dictionary<string, object>> configuration;
            var key = string.Format(CONFIGURATION_CACHE_KEY_FORMAT, featureName);

            if (_memoryCache.TryGetValue(key, out configuration))
            {
                return Task.FromResult(configuration.Keys.AsEnumerable());
            }

            return Task.FromResult(Enumerable.Empty<Type>());
        }

        public Task<ActivatorParameter> FindParameterAsync(string name, string featureName, string activatorType)
        {
            ActivatorParameter parameter = null;

            var entry = _memoryCache.Get(string.Format(FEATURE_CACHE_KEY_FORMAT, featureName));

            if (entry != null)
            {
                var configuration = (Dictionary<Type, Dictionary<string, object>>)_memoryCache
                    .Get(string.Format(CONFIGURATION_CACHE_KEY_FORMAT, featureName));

                var parameters = configuration.Where(cfg => cfg.Key.FullName == activatorType)
                    .SingleOrDefault()
                    .Value;

                if (parameters.ContainsKey(name))
                {
                    parameter = new ActivatorParameter()
                    {
                        FeatureName = featureName,
                        ActivatorType = activatorType,
                        Name = name,
                        Value = parameters[name].ToString()
                    };
                }
                else
                {
                    _logger.LogWarning($"The parameter {name} for activator {activatorType} is not present in the store.");
                }
            }
            else
            {
                _logger.LogWarning($"The parameter {name} for activator {activatorType} can't be located because feature {featureName} is not persisted.");
            }

            return Task.FromResult<ActivatorParameter>(parameter);

        }

        public Task<bool> PersistConfiguratioAsync(IEnumerable<FeatureConfigurer> features)
        {
            var persisted = false;

            if (features != null)
            {
                foreach (var item in features)
                {
                    var feature = new Feature()
                    {
                        CreatedOn = DateTime.UtcNow,
                        Name = item.FeatureName
                    };

                    SetOnCache(string.Format(FEATURE_CACHE_KEY_FORMAT, item.FeatureName), feature);
                    SetOnCache(string.Format(CONFIGURATION_CACHE_KEY_FORMAT, item.FeatureName), item.Configuration);

                    _logger.LogInformation($"Persisted feature with name {item.FeatureName}");
                }
                persisted = true;
            }

            return Task.FromResult<bool>(persisted);
        }

        void SetOnCache(string key, object item)
        {
            _memoryCache.Set(key, item, new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.MaxValue,
                Priority = CacheItemPriority.Normal
            });
        }
    }
}
