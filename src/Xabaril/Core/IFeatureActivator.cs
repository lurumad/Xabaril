using System.Threading.Tasks;

namespace Xabaril.Core
{
    public interface IFeatureActivator
    {
        Task<bool> IsActiveAsync(string featureName);
    }
}
