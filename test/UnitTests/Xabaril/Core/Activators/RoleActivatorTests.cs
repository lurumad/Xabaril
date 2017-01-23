using FluentAssertions;
using global::Xabaril;
using global::Xabaril.Core;
using global::Xabaril.Core.Activators;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Xabaril.Core.Activators
{
    public class role_activator_should
    {
        [Fact]
        public async Task indicates_whether_activator_is_active()
        {
            var activator = new RoleActivatorBuilder()
                .WithRuntimeParameters(new Dictionary<string, object>()
                {
                    { "role", "SuperAdmin" }
                })
                .WithDefaultActiveRole("SuperAdmin")
                .Build();

            (await activator.IsActiveAsync("feature")).Should().Be(true);
        }

        [Fact]
        public async Task indicates_whether_activator_is_active_with_multiple_parameter_values()
        {
            var activator = new RoleActivatorBuilder()
                .WithRuntimeParameters(new Dictionary<string, object>()
                {
                    { "role", "sa;SuperAdmin;user" }
                })
                .WithDefaultActiveRole("SuperAdmin")
                .Build();

            (await activator.IsActiveAsync("feature")).Should().Be(true);
        }

        [Fact]
        public async Task indicates_whether_activator_is_not_active()
        {
            var activator = new RoleActivatorBuilder()
                .WithRuntimeParameters(new Dictionary<string, object>()
                {
                    { "role", "InvalidRole;AnotherNotValidRole" }
                })
                .WithDefaultActiveRole("SuperAdmin")
                .Build();

            (await activator.IsActiveAsync("feature")).Should().Be(false);
        }

        [Fact]
        public async Task indicates_whether_activator_is_not_active_for_non_configured_parameter()
        {
            var activator = new RoleActivatorBuilder()
                .WithRuntimeParameters(new Dictionary<string, object>()
                {
                    { "role", null }
                })
                .WithDefaultActiveRole("SuperAdmin")
                .Build();

            (await activator.IsActiveAsync("feature")).Should().Be(false);
        }

        [Fact]
        public async Task not_be_case_sensitive()
        {
            var activator = new RoleActivatorBuilder()
                .WithRuntimeParameters(new Dictionary<string, object>()
                {
                    { "role", "sUpErAdMin" }
                })
                .WithDefaultActiveRole("SuperAdmin")
                .Build();

            (await activator.IsActiveAsync("feature")).Should().Be(true);
        }


        private class RoleActivatorBuilder
        {
            IRoleProvider _roleProvider;

            Dictionary<string, object> _parameters = new Dictionary<string, object>();

            public RoleActivator Build()
            {
                var loggerFactory = new LoggerFactory();
                var logger = loggerFactory.CreateLogger<XabarilModule>();

                var runtimeParameterAccessor = RuntimeParameterAccessorBuilder.Build(_parameters);

                return new RoleActivator(logger, runtimeParameterAccessor,  _roleProvider);
            }

            public RoleActivatorBuilder WithRuntimeParameters(Dictionary<string, object> parameters)
            {
                _parameters = parameters;

                return this;
            }

            public RoleActivatorBuilder WithDefaultActiveRole(string role)
            {
                _roleProvider = new SimpleRoleProvider(role);

                return this;
            }
        }

        private class SimpleRoleProvider
           : IRoleProvider
        {

            string _currentRole;

            public SimpleRoleProvider(string currentRole)
            {
                _currentRole = currentRole;
            }
            public Task<string> GetRoleAsync()
            {
                return Task.FromResult<string>(_currentRole);
            }
        }
    }
}
