using FluentAssertions;
using global::Xabaril;
using global::Xabaril.Core;
using global::Xabaril.Store;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Xabaril.Core
{
    public class store_runtime_parameter_accessor_should
    {
        [Fact]
        public async Task return_converted_value_as_ttype()
        {
            var runtimeParamterAccessor = new StoreRuntimeParameterAccesorBuilder()
                .WithParameters(new Dictionary<string, ActivatorParameter>()
                {
                    { "double",new ActivatorParameter() { Name = "double", Value = "22.2", ActivatorType = "", FeatureName = "" } },
                    { "int",new ActivatorParameter() { Name = "int", Value = "222", ActivatorType = "", FeatureName = "" } },
                    { "decimal",new ActivatorParameter() { Name = "decimal", Value = "22.2", ActivatorType = "", FeatureName = "" } },
                    { "string",new ActivatorParameter() { Name = "string", Value = "some", ActivatorType = "", FeatureName = "" } },
                    { "long",new ActivatorParameter() { Name = "long", Value = "2233332", ActivatorType = "", FeatureName = "" } },
                }).Build();

            (await runtimeParamterAccessor.GetValueAsync<double>("", new ActivatorParameterDescriptor() { Name = "double" }))
                .Should().Be(22.2d);

            (await runtimeParamterAccessor.GetValueAsync<int>("", new ActivatorParameterDescriptor() { Name = "int" }))
                .Should().Be(222);

            (await runtimeParamterAccessor.GetValueAsync<decimal>("", new ActivatorParameterDescriptor() { Name = "decimal" }))
                .Should().Be(22.2M);

            (await runtimeParamterAccessor.GetValueAsync<string>("", new ActivatorParameterDescriptor() { Name = "string" }))
                .Should().Be("some");

            (await runtimeParamterAccessor.GetValueAsync<long>("", new ActivatorParameterDescriptor() { Name = "long" }))
                .Should().Be(2233332);
        }

        [Fact]
        public async Task get_default_configured_value_if_conversion_fail()
        {
            var runtimeParamterAccessor = new StoreRuntimeParameterAccesorBuilder()
                .Build();

            (await runtimeParamterAccessor.GetValueAsync<double>("", new ActivatorParameterDescriptor() { Name = "non_valid_double" }))
                .Should().Be(default(Double));

        }

        private class StoreRuntimeParameterAccesorBuilder
        {
            Dictionary<string, ActivatorParameter> _parameters;

            public StoreRuntimeParameterAccesor Build()
            {
                var loggerFactory = new LoggerFactory();

                var logger = loggerFactory.CreateLogger<XabarilModule>();

                return new StoreRuntimeParameterAccesor(logger, new DictionaryFeatureStore(_parameters ?? new Dictionary<string, ActivatorParameter>()));
            }

            public StoreRuntimeParameterAccesorBuilder WithParameters(Dictionary<string, ActivatorParameter> parameters)
            {
                _parameters = parameters;

                return this;
            }

        }

        private class DictionaryFeatureStore : IFeaturesStore
        {
            Dictionary<string, ActivatorParameter> _data;

            public DictionaryFeatureStore(Dictionary<string, ActivatorParameter> data)
            {
                _data = data;
            }

            public Task<IEnumerable<Type>> FindFeatureActivatorsTypesAsync(string featureName)
            {
                return Task.FromResult(Enumerable.Empty<Type>());
            }

            public Task<Feature> FindFeatureAsync(string featureName)
            {
                return Task.FromResult<Feature>(new Feature() { Name = featureName, CreatedOn = DateTime.UtcNow });
            }

            public Task<ActivatorParameter> FindParameterAsync(string name, string featureName, string activatorType)
            {
                ActivatorParameter value;

                _data.TryGetValue(name, out value);

                return Task.FromResult<ActivatorParameter>(value);
            }

            public Task<bool> PersistConfiguratioAsync(IEnumerable<FeatureConfigurer> features)
            {
                return Task.FromResult<bool>(true);
            }
        }
    }
}
