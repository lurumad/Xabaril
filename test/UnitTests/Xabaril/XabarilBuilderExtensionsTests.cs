using FluentAssertions;
using global::Xabaril.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Threading.Tasks;
using Xabaril;
using Xunit;

namespace UnitTests
{
    public class xabaril_builder_extensions_should
    {
        [Fact]
        public void configure_xabaril_options()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();

            serviceCollection.AddXabaril()
                .AddXabarilOptions(options =>
                {
                    options.FailureMode = FailureMode.LogAndDisable;
                });

            var provider = serviceCollection.BuildServiceProvider();

            (provider.GetRequiredService<IOptions<XabarilOptions>>())
                .Should().NotBeNull();

            (provider.GetRequiredService<IOptions<XabarilOptions>>()).Value.FailureMode
                .Should().Be(FailureMode.LogAndDisable);
        }

        [Fact]
        public void replace_default_geolocation_provider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();

            serviceCollection.AddXabaril()
                .AddGeoLocationProvider<ReplacedGeoLocationProvider>();

            var provider = serviceCollection.BuildServiceProvider();

            (provider.GetRequiredService<IGeoLocationProvider>())
                .Should().BeOfType<ReplacedGeoLocationProvider>();
        }

        [Fact]
        public void replace_default_user_provider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();

            serviceCollection.AddXabaril()
                .AddRoleProvider<ReplacedRoleProvider>();

            var provider = serviceCollection.BuildServiceProvider();

            (provider.GetRequiredService<IRoleProvider>())
                .Should().BeOfType<ReplacedRoleProvider>();
        }


        [Fact]
        public void configure_custom_activators_assembly()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();

            var builder = serviceCollection.AddXabaril()
                .AddCustomActivators(typeof(xabaril_builder_extensions_should).GetTypeInfo().Assembly);

            var services = builder.Services;

            var provider = serviceCollection.BuildServiceProvider();

            (provider.GetRequiredService<AlwaysOnActivator>()).Should().NotBeNull();
        }

        private class ReplacedGeoLocationProvider
        : IGeoLocationProvider
        {
            public Task<string> FindLocationAsync(string ipAddress)
            {
                return Task.FromResult<string>(null);
            }
        }

        private class ReplacedUserProvider
            : IUserProvider
        {
            public Task<string> GetUserNameAsync()
            {
                return Task.FromResult<string>(null);
            }
        }

        private class ReplacedRoleProvider
            : IRoleProvider
        {
            public Task<string> GetRoleAsync()
            {
                return Task.FromResult<string>(null);
            }
        }

        private class AlwaysOnActivator
            : IFeatureActivator
        {
            public Task<bool> IsActiveAsync(string featureName)
            {
                return Task.FromResult<bool>(true);
            }
        }
    }
}
