using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xabaril.Core.Activators
{
    public sealed class RoleActivator
        : IFeatureActivator, IDiscoverableActivatorParameters
    {
        private readonly ILogger<XabarilModule> _logger;
        private readonly IRuntimeParameterAccessor _runtimeParameterAccessor;
        private readonly IRoleProvider _roleProvider;
      
        List<ActivatorParameterDescriptor> _descriptors = new List<ActivatorParameterDescriptor>()
        {
             new ActivatorParameterDescriptor() {Name = "role", ClrType=typeof(String).Name , IsOptional = false,ActivatorName = typeof(UTCActivator).Name},
        };

        public IEnumerable<ActivatorParameterDescriptor> Descriptors => _descriptors;

        public RoleActivator(ILogger<XabarilModule> logger,
            IRuntimeParameterAccessor runtimeParameterAccessor,
            IRoleProvider roleProvider)
        {
            _logger = logger;
            _roleProvider = roleProvider;
            _runtimeParameterAccessor = runtimeParameterAccessor;
        }

        public async Task<bool> IsActiveAsync(string featureName)
        {
            var active = false;

            var roles = await _runtimeParameterAccessor
                .GetValueAsync<string>(featureName, _descriptors[0]);

            if (!String.IsNullOrWhiteSpace(roles))
            {
                var splittedRoles = roles.Split(';');

                var currentRole = await _roleProvider.GetRoleAsync();

                return splittedRoles.Any(s => String.Equals(s, currentRole, StringComparison.CurrentCultureIgnoreCase));
            }
            else
            {
                _logger.LogWarning($"The parameter ROLE on feature {featureName} is not configured properly.");
            }

            return active;
        }
    }
}
