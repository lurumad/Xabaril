using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xabaril.Store
{
    sealed class NoFeaturesStore : IFeaturesStore
    {
        public Task<Feature> FindFeatureAsync(string featureName)
        {
            return Task.FromResult<Feature>(null);
        }

        public Task<IEnumerable<Type>> FindFeatureActivatorsTypesAsync(string featureName)
        {
            return Task.FromResult(Enumerable.Empty<Type>());
        }

        public Task<ActivatorParameter> FindParameterAsync(string name, string featureName, string activatorType)
        {
            return Task.FromResult<ActivatorParameter>(null);
        }

        public Task<bool> PersistConfiguratioAsync(IEnumerable<FeatureConfigurer> features)
        {
            return Task.FromResult<bool>(false);
        }
    }
}
