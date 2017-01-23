using System.Threading.Tasks;
using global::Xabaril.Core;

namespace UnitTests.Xabaril.Core
{
    internal class AlwaysDisabledFeatureActivator : IFeatureActivator
    {
        public Task<bool> IsActiveAsync(string featureName)
        {
            return Task.FromResult(false);
        }
    }
}