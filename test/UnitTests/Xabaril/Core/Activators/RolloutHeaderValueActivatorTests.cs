using FluentAssertions;
using global::Xabaril;
using global::Xabaril.Core.Activators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Xabaril.Core.Activators
{
    public class rollout_header_value_activator_should
    {
        [Fact]
        public async Task indicates_whether_activator_is_active_when_percentage_is_100()
        {
            var activator = new RolloutHeaderValueActivatorBuilder()
                .WithRuntimeParameters(new Dictionary<string, object>()
                {
                    { "header-name", "existing_header" },
                    { "percentage", 100d },
                })
                .WithHeaderValue("existing_header", "some_value")
                .Build();

            int index = 0;

            while (index < 100)
            {
                (await activator.IsActiveAsync("feature")).Should().Be(true);

                ++index;
            }
        }

        [Fact]
        public async Task indicates_whether_activator_is_not_active_if_value_is_not_on_partition()
        {
            var activator = new RolloutHeaderValueActivatorBuilder()
                .WithRuntimeParameters(new Dictionary<string, object>()
                {
                    { "header-name", "existing_header" },
                    { "percentage", 50d },
                })
                .WithHeaderValue("existing_header", "some_value")
                .Build();

            int index = 0;

            while (index < 100)
            {
                (await activator.IsActiveAsync("feature")).Should().Be(false);

                ++index;
            }
        }

        [Fact]
        public async Task indicates_whether_activator_is_not_active_if_percentage_is_zero()
        {
            var activator = new RolloutHeaderValueActivatorBuilder()
                .WithRuntimeParameters(new Dictionary<string, object>()
                {
                    { "header-name", "existing_header" },
                    { "percentage", 0d },
                })
                .WithHeaderValue("existing_header", "some_value")
                .Build();

            int index = 0;

            while (index < 100)
            {
                (await activator.IsActiveAsync("feature")).Should().Be(false);

                ++index;
            }
        }

        private class RolloutHeaderValueActivatorBuilder
        {
            Dictionary<string, object> _parameters = new Dictionary<string, object>();
            string _header;
            string _value;

            public RolloutHeaderValueActivator Build()
            {
                var loggerFactory = new LoggerFactory();
                var logger = loggerFactory.CreateLogger<XabarilModule>();

                var runtimeParameterAccessor = RuntimeParameterAccessorBuilder.Build(_parameters);

                var httpContextAccesor = new HttpContextAccessor();
                httpContextAccesor.HttpContext = new DefaultHttpContext();

                httpContextAccesor.HttpContext.Request.Headers.Add(_header, _value);

                return new RolloutHeaderValueActivator(logger, runtimeParameterAccessor, httpContextAccesor);
            }

            public RolloutHeaderValueActivatorBuilder WithRuntimeParameters(Dictionary<string, object> parameters)
            {
                _parameters = parameters;

                return this;
            }

            public RolloutHeaderValueActivatorBuilder WithHeaderValue(string header, string value)
            {
                _header = header;
                _value = value;

                return this;
            }
        }
    }
}
