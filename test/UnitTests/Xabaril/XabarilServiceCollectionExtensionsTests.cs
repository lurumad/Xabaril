using FluentAssertions;
using global::Xabaril.Core;
using global::Xabaril.Core.Activators;
using global::Xabaril.Store;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace UnitTests
{
    public class xabaril_servicecollection_extensions_should
    {
        [Fact]
        public void configure_all_required_xabaril_sevices()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();

            serviceCollection.AddXabaril();

            var provider = serviceCollection.BuildServiceProvider();

            //runtime services

            (provider.GetService<IFeaturesService>()).Should().BeOfType<FeaturesService>();
            (provider.GetService<IFeaturesStore>()).Should().BeOfType<NoFeaturesStore>();
            (provider.GetService<IUserProvider>()).Should().BeOfType<HttpContextUserProvider>();
            (provider.GetService<IGeoLocationProvider>()).Should().BeOfType<NoGeoLocationProvider>();
            (provider.GetService<IRoleProvider>()).Should().BeOfType<HttpContextRoleProvider>();
            (provider.GetService<IRuntimeParameterAccessor>()).Should().BeOfType<StoreRuntimeParameterAccesor>();

            //default activators

            (provider.GetService<UTCActivator>()).Should().NotBeNull();
            (provider.GetService<LocationActivator>()).Should().NotBeNull();
            (provider.GetService<RolloutHeaderValueActivator>()).Should().NotBeNull();
            (provider.GetService<RolloutUsernameActivator>()).Should().NotBeNull();
            (provider.GetService<UserActivator>()).Should().NotBeNull();
            (provider.GetService<RoleActivator>()).Should().NotBeNull();
        }
    }
}
