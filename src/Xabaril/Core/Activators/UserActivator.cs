using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Xabaril.Core.Activators
{
    public sealed class UserActivator
        : IFeatureActivator, IDiscoverableActivatorParameters
    {
        private readonly ILogger<XabarilModule> _logger;
        private readonly IRuntimeParameterAccessor _runtimeParameterAccessor;
        private readonly IUserProvider _userProvider;

        List<ActivatorParameterDescriptor> _descriptors = new List<ActivatorParameterDescriptor>()
        {
             new ActivatorParameterDescriptor() {Name = "user", ClrType=typeof(String).Name , IsOptional = false,ActivatorName = typeof(UTCActivator).Name},
        };

        public IEnumerable<ActivatorParameterDescriptor> Descriptors => _descriptors;

        public UserActivator(ILogger<XabarilModule> logger,
            IRuntimeParameterAccessor runtimeParameterAccessor,
            IUserProvider userProvider)
        {
            _logger = logger;
            _runtimeParameterAccessor = runtimeParameterAccessor;
            _userProvider = userProvider;
        }

        public async Task<bool> IsActiveAsync(string featureName)
        {
            var active = false;

            var users = await _runtimeParameterAccessor
                .GetValueAsync<string>(featureName, _descriptors[0]);

            if (!String.IsNullOrWhiteSpace(users))
            {
                var splitedUsers = users.Split(';');

                var currentUser = await _userProvider
                    .GetUserNameAsync();

                return splitedUsers.Any(s => String.Equals(s, currentUser, StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                _logger.LogWarning($"The parameter USER on feature {featureName} is not configured properly.");
            }

            return active;
        }
    }
}
