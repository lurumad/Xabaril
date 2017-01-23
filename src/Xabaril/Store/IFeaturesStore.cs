using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Xabaril.Store
{
    public interface IFeaturesStore
    {
        Task<Feature> FindFeatureAsync(string featureName);

        Task<IEnumerable<Type>> FindFeatureActivatorsTypesAsync(string featureName);

        Task<ActivatorParameter> FindParameterAsync(string name, string featureName, string activatorType);

        Task<bool> PersistConfiguratioAsync(IEnumerable<FeatureConfigurer> features);
    }
}
