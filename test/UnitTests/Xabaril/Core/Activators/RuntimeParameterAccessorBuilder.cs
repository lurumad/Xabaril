using global::Xabaril.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests.Xabaril.Core.Activators
{
    public static class RuntimeParameterAccessorBuilder
    {
        public static IRuntimeParameterAccessor Build(Dictionary<string,object> parameters)
        {
            return new FromDictionaryRuntimeParamterAccesor(parameters);
        }

        private class FromDictionaryRuntimeParamterAccesor
            : IRuntimeParameterAccessor
        {
            Dictionary<string, object> _paramters = new Dictionary<string, object>();

            public FromDictionaryRuntimeParamterAccesor(Dictionary<string, object> parameters)
            {
                _paramters = parameters;
            }

            public Task<TType> GetValueAsync<TType>(string featureName, ActivatorParameterDescriptor parameterDescriptor)
            {
                TType value;

                if (_paramters.ContainsKey(parameterDescriptor.Name))
                {
                    value = (TType)_paramters[parameterDescriptor.Name];
                }
                else
                {
                    value = default(TType);
                }

                return Task.FromResult<TType>(value);
            }
        }
    }
}
