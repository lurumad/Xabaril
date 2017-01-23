using System.Threading.Tasks;

namespace Xabaril.Core
{
    public interface IFeaturesService
    {
        Task<bool> IsEnabledAsync(string featureName);

        Task<bool> IsEnabledAsync<TFeature>() where TFeature : class;
    }
}
