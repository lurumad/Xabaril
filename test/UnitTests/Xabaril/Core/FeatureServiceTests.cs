using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xabaril;
using Xunit;

namespace UnitTests.Xabaril.Core
{
    public class feature_service_should
    {
        private const string FeatureName = "feature_test";

        [Fact]
        public void throws_an_argument_null_exception_if_the_feature_does_not_exists()
        {
            var featureService = new FeatureServiceBuilder()
                                    .WithInMemoryStore()
                                    .Build();

            Func<Task<bool>> act = async () => await featureService.IsEnabledAsync("feature_name_does_not_exists");

            act.ShouldThrow<ArgumentException>();
        }

        [Fact]
        public async Task indicate_disable_if_failure_mode_is_log_and_disable()
        {
            var featureService = new FeatureServiceBuilder()
                                    .WithInMemoryStore()
                                    .WithOptions(new XabarilOptions()
                                    {
                                        FailureMode = FailureMode.LogAndDisable
                                    }).Build();

            (await featureService.IsEnabledAsync("feature_name_does_not_exists")).Should().Be(false);

        }

        [Fact]
        public async Task indicates_whether_a_feature_is_enabled()
        {
            var featureService = new FeatureServiceBuilder()
                                    .WithInMemoryStore()
                                    .WithEnabledFeature(FeatureName)
                                    .Build();

            (await featureService.IsEnabledAsync(FeatureName)).Should().Be(true);
        }

        [Fact]
        public async Task indicates_whether_a_feature_is_disabled()
        {
            var featureService = new FeatureServiceBuilder()
                                    .WithInMemoryStore()
                                    .WithDisabledFeature(FeatureName)
                                    .Build();

            (await featureService.IsEnabledAsync(FeatureName)).Should().Be(false);
        }
    }
}
