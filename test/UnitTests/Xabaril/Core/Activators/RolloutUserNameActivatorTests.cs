using FluentAssertions;
using global::Xabaril;
using global::Xabaril.Core;
using global::Xabaril.Core.Activators;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Xabaril.Core.Activators
{
    public class rollout_user_activator_should
    {
        [Fact]
        public async Task indicates_whether_activator_is_active()
        {
            var activator = new RolloutUserNameActivatorBuilder()
                .WithRuntimeParameters(new Dictionary<string, object>()
                {
                    {"percentage",50d }
                })
                .WithActiveUsers(new string[] { "user_selected_to_be_on_first_part" })
                .Build();

            int index = 0;

            while (index < 100)
            {
                (await activator.IsActiveAsync("feature")).Should().Be(true);

                ++index;
            }
        }

        [Fact]
        public async Task be_active_when_percentage_is_100()
        {
            var activator = new RolloutUserNameActivatorBuilder()
                .WithRuntimeParameters(new Dictionary<string, object>()
                {
                    {"percentage",100d }
                })
                .WithActiveUsers(new string[] { "user1" })
                .Build();

            int index = 0;

            while (index < 100)
            {
                (await activator.IsActiveAsync("feature")).Should().Be(true);

                ++index;
            }
        }

        [Fact]
        public async Task be_not_active_when_percentage_is_0()
        {
            var activator = new RolloutUserNameActivatorBuilder()
                .WithRuntimeParameters(new Dictionary<string, object>()
                {
                    {"percentage",0d }
                })
                .WithActiveUsers(new string[] { "user1" })
                .Build();

            int index = 0;

            while (index < 100)
            {
                (await activator.IsActiveAsync("feature")).Should().Be(false);

                ++index;
            }
        }

        private class RolloutUserNameActivatorBuilder
        {
            IUserProvider _userProvider;
            Dictionary<string, object> _parameters = new Dictionary<string, object>();

            public RolloutUsernameActivator Build()
            {
                var loggerFactory = new LoggerFactory();
                var logger = loggerFactory.CreateLogger<XabarilModule>();

                var runtimeParameterAccessor = RuntimeParameterAccessorBuilder.Build(_parameters);

                return new RolloutUsernameActivator(logger, runtimeParameterAccessor, _userProvider);
            }

            public RolloutUserNameActivatorBuilder WithRuntimeParameters(Dictionary<string, object> parameters)
            {
                _parameters = parameters;

                return this;
            }

            public RolloutUserNameActivatorBuilder WithActiveUsers(string[] users)
            {
                _userProvider = new SimpleUserProvider(users);

                return this;
            }
        }

        private class SimpleUserProvider
            : IUserProvider
        {
            string[] _users;

            Random random = new Random(234234);

            public SimpleUserProvider(string[] users)
            {
                _users = users;
            }

            public Task<string> GetUserNameAsync()
            {
                var index = random.Next() % _users.Length;

                return Task.FromResult<string>(_users[index]);
            }
        }
    }
}
