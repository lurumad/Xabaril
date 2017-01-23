using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xabaril.Core.Activators
{
    public sealed class RolloutUsernameActivator
        : IFeatureActivator, IDiscoverableActivatorParameters
    {
        const string ANONYMOUS_USER = "Anonymous";
        const int NUMBER_OF_PARTITIONS = 10;

        private readonly ILogger<XabarilModule> _logger;
        private readonly IRuntimeParameterAccessor _runtimeParameterAccessor;
        private readonly IUserProvider _userProvider;
        
        List<ActivatorParameterDescriptor> _descriptors = new List<ActivatorParameterDescriptor>()
        {
             new ActivatorParameterDescriptor() {Name = "percentage", ClrType=typeof(Double).Name , IsOptional = false,ActivatorName = typeof(RolloutUsernameActivator).Name},
        };

        public IEnumerable<ActivatorParameterDescriptor> Descriptors
        {
            get
            {
                return _descriptors;
            }
        }

        public RolloutUsernameActivator(ILogger<XabarilModule> logger,
            IRuntimeParameterAccessor runtimeParameterAccessor,
            IUserProvider userProvider)
        {
            _logger = logger;
            _runtimeParameterAccessor = runtimeParameterAccessor;
            _userProvider = userProvider;
        }

        public async Task<bool> IsActiveAsync(string featureName)
        {
            var percentage = await _runtimeParameterAccessor
                .GetValueAsync<double>(featureName, _descriptors[0]);

            var username = await _userProvider
                .GetUserNameAsync() ?? ANONYMOUS_USER;

            var assignedPartition = JenkinsPartitioner.ResolveToLogicalPartition(username, NUMBER_OF_PARTITIONS);

            return assignedPartition <= ((NUMBER_OF_PARTITIONS * percentage) / 100);
        }
    }
}
