using System.Threading.Tasks;

namespace Xabaril.Core
{
    public interface IRuntimeParameterAccessor
    {
        Task<TType> GetValueAsync<TType>(string featureName,ActivatorParameterDescriptor parameterDescriptor);
    }
}
