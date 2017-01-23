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
    public class user_activator_should
    {
        [Fact]
        public async Task indicates_whether_activator_is_active()
        {
            var activator = new UserActivatorBuilder()
                .WithRuntimeParameters(new Dictionary<string, object>()
                {
                    { "user", "dotnetuser" }
                })
                .WithActiveUser("dotnetuser")
                .Build();

            (await activator.IsActiveAsync("feature")).Should().Be(true);
        }

        [Fact]
        public async Task indicates_whether_activator_is_not_active()
        {
            var activator = new UserActivatorBuilder()
                .WithRuntimeParameters(new Dictionary<string, object>()
                {
                    { "user", "dotnetuser" }
                })
                .WithActiveUser("anonimous")
                .Build();

            (await activator.IsActiveAsync("feature")).Should().Be(false);
        }

        [Fact]
        public async Task not_be_case_sensitive()
        {
            var activator = new UserActivatorBuilder()
                .WithRuntimeParameters(new Dictionary<string, object>()
                {
                    { "user", "dotnetuser" }
                })
                .WithActiveUser("DoTnEtUsEr")
                .Build();

            (await activator.IsActiveAsync("feature")).Should().Be(true);
        }


        [Fact]
        public async Task indicates_not_active_if_parameter_is_not_configured()
        {
            var activator = new UserActivatorBuilder()
                .WithRuntimeParameters(new Dictionary<string, object>()
                {
                    { "user", null }
                })
                .WithActiveUser("DoTnEtUsEr")
                .Build();

            (await activator.IsActiveAsync("feature")).Should().Be(false);
        }

        [Fact]
        public async Task indicates_active_if_parameter_is_on_a_list()
        {
            var activator = new UserActivatorBuilder()
                .WithRuntimeParameters(new Dictionary<string, object>()
                {
                    { "user", "anhotheruser;dotnetuser;user" }
                })
                .WithActiveUser("dotnetuser")
                .Build();

            (await activator.IsActiveAsync("feature")).Should().Be(true);
        }

        [Fact]
        public async Task indicates_not_active_if_parameter_is_not_on_a_list()
        {
            var activator = new UserActivatorBuilder()
                .WithRuntimeParameters(new Dictionary<string, object>()
                {
                    { "user", "anhotheruser;dotnetuser;user" }
                })
                .WithActiveUser("why?")
                .Build();

            (await activator.IsActiveAsync("feature")).Should().Be(false);
        }


        private class UserActivatorBuilder
        {
            IUserProvider _userProvider;
            Dictionary<string, object> _parameters = new Dictionary<string, object>();

            public UserActivator Build()
            {
                var loggerFactory = new LoggerFactory();
                var logger = loggerFactory.CreateLogger<XabarilModule>();

                var runtimeParameterAccessor = RuntimeParameterAccessorBuilder.Build(_parameters);

                return new UserActivator(logger, runtimeParameterAccessor, _userProvider);
            }

            public UserActivatorBuilder WithRuntimeParameters(Dictionary<string, object> parameters)
            {
                _parameters = parameters;

                return this;
            }

            public UserActivatorBuilder WithActiveUser(string user)
            {
                _userProvider = new SimpleUserProvider(user);

                return this;
            }
        }

        private class SimpleUserProvider
           : IUserProvider
        {
            string _user;

            public SimpleUserProvider(string user)
            {
                _user = user;
            }

            public Task<string> GetUserNameAsync()
            {
                return Task.FromResult<string>(_user);
            }
        }
    }
}
