using System;
using System.Collections.Generic;
using Xabaril.Core;

namespace Xabaril.Store
{
    public class FeatureConfigurer
    {
        string _featureName;

        public string FeatureName => _featureName;

        Dictionary<Type, Dictionary<string, object>> _configuration;

        public Dictionary<Type, Dictionary<string, object>> Configuration => _configuration;

        public FeatureConfigurer(string featureName)
        {
            if (featureName == null)
            {
                throw new ArgumentNullException("featureName");
            }

            _featureName = featureName;
            _configuration = new Dictionary<Type, Dictionary<string, object>>();
        }

        public FeatureConfigurer WithActivator<TFeatureActivator>(Action<Dictionary<string, object>> configureParameters)
            where TFeatureActivator : IFeatureActivator
        {
            var parameters = new Dictionary<string, object>();

            configureParameters(parameters);

            _configuration.Add(typeof(TFeatureActivator), parameters);

            return this;
        }

    }
}
