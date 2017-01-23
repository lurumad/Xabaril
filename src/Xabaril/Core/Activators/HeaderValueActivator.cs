using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xabaril.Core.Activators
{
    public sealed class HeaderValueActivator
        : IFeatureActivator, IDiscoverableActivatorParameters
    {
        private readonly ILogger<XabarilModule> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRuntimeParameterAccessor _runtimeParameterAccesor;

        public HeaderValueActivator(ILogger<XabarilModule> logger,
            IRuntimeParameterAccessor runtimeParameterAccessor,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _runtimeParameterAccesor = runtimeParameterAccessor;
        }

        List<ActivatorParameterDescriptor> _descriptors = new List<ActivatorParameterDescriptor>()
        {
             new ActivatorParameterDescriptor() {Name = "header-name", ClrType=typeof(String).Name , IsOptional = false,ActivatorName = typeof(RolloutUsernameActivator).Name},
             new ActivatorParameterDescriptor() {Name = "header-value", ClrType=typeof(String).Name , IsOptional = false,ActivatorName = typeof(RolloutUsernameActivator).Name},
        };

        public IEnumerable<ActivatorParameterDescriptor> Descriptors
        {
            get
            {
                return _descriptors;
            }
        }

        public async Task<bool> IsActiveAsync(string featureName)
        {
            var active = false;

            var httpContext = _httpContextAccessor.HttpContext;

            var headerName = await _runtimeParameterAccesor
                .GetValueAsync<string>(featureName, _descriptors[0]);

            if (headerName != null)
            {
                var headerValue = await _runtimeParameterAccesor
                    .GetValueAsync<string>(featureName, _descriptors[1]);

                if (headerValue != null)
                {
                    var headerValues = httpContext.Request
                        .Headers[headerName];

                    if (headerValues.Count > 0)
                    {
                        return headerValues.Any(s => s.Equals(headerValue, StringComparison.CurrentCultureIgnoreCase));
                    }
                }
                else
                {
                    _logger.LogWarning($"The header value {_descriptors[1].Name} for feature {featureName} on HeaderValueActivator is not configured correctly.");
                }
            }
            else
            {
                _logger.LogWarning($"The header name {_descriptors[0].Name} for feature {featureName} on HeaderValueActivator is not configured correctly.");
            }

            return active;
        }
    }
}
