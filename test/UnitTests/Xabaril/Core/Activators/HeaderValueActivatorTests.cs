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
    public class header_value_activator_should
    {
        [Fact]
        public async Task indicates_whether_activator_is_active()
        {
            var activator = new HeaderValueActivatorBuilder()
                .WithRuntimeParameters(new Dictionary<string, object>()
                {
                    { "header-name", "existing_header" },
                    { "header-value", "valid_value" },
                })
                .WithDefaultHttpContextHeaders()
                .Build();

            (await activator.IsActiveAsync("feature")).Should().Be(true);
        }

        [Fact]
        public async Task works_with_headers_with_multiple_values()
        {
            var activator = new HeaderValueActivatorBuilder()
                .WithRuntimeParameters(new Dictionary<string, object>()
                {
                    { "header-name", "multiple_existing_header" },
                    { "header-value", "valid_value2" },
                })
                .WithDefaultHttpContextHeaders()
                .Build();

            (await activator.IsActiveAsync("feature")).Should().Be(true);
        }

        [Fact]
        public async Task indicates_whether_activator_is_not_active_on_headers_with_invalid_value()
        {
            var activator = new HeaderValueActivatorBuilder()
                 .WithRuntimeParameters(new Dictionary<string, object>()
                 {
                    { "header-name", "existing_header" },
                    { "header-value", "not_valid_value" },
                 })
                 .WithDefaultHttpContextHeaders()
                 .Build();

            (await activator.IsActiveAsync("feature")).Should().Be(false);
        }

        [Fact]
        public async Task indicates_whether_activator_is_not_active_on_headers_with_multiple_invalid_values()
        {
            var activator = new HeaderValueActivatorBuilder()
               .WithRuntimeParameters(new Dictionary<string, object>()
               {
                    { "header-name", "multiple_existing_header" },
                    { "header-value", "not_valid_value" },
               })
               .WithDefaultHttpContextHeaders()
               .Build();

            (await activator.IsActiveAsync("feature")).Should().Be(false);
        }

        [Fact]
        public async Task indicates_wheter_activator_is_not_active_on_http_context_without_header()
        {
            var activator = new HeaderValueActivatorBuilder()
               .WithRuntimeParameters(new Dictionary<string, object>()
               {
                    { "header-name", "non_existing_header" },
                    { "header-value", "not_valid_value" },
               })
               .WithDefaultHttpContextHeaders()
               .Build();

            (await activator.IsActiveAsync("feature")).Should().Be(false);
        }

        private class HeaderValueActivatorBuilder
        {
            Dictionary<string, object> _parameters = new Dictionary<string, object>();
            Dictionary<string, string[]> _headers = new Dictionary<string, string[]>();

            public HeaderValueActivator Build()
            {
                var loggerFactory = new LoggerFactory();
                var logger = loggerFactory.CreateLogger<XabarilModule>();

                var runtimeParameterAccessor = RuntimeParameterAccessorBuilder.Build(_parameters);

                var httpContextAccesor = new HttpContextAccessor();
                httpContextAccesor.HttpContext = new DefaultHttpContext();

                foreach (var key in _headers.Keys)
                {
                    httpContextAccesor.HttpContext.Request.Headers.Add(key, _headers[key]);
                }

                return new HeaderValueActivator(logger, runtimeParameterAccessor, httpContextAccesor);

            }
            public HeaderValueActivatorBuilder WithRuntimeParameters(Dictionary<string, object> parameters)
            {
                _parameters = parameters;

                return this;
            }

            public HeaderValueActivatorBuilder WithDefaultHttpContextHeaders()
            {
                _headers = new Dictionary<string, string[]>()
                {
                    {"existing_header",new string[] { "valid_value" } },
                    {"another_header",new string[] { "another_diferent_string" } },
                    {"multiple_existing_header",new string[] { "valid_value1", "valid_value2" } }
                }; ;

                return this;
            }

            public HeaderValueActivatorBuilder WithHttpContextHeaders(Dictionary<string, string[]> headers)
            {
                _headers = headers;

                return this;
            }
        }
    }
}
