using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Xabaril.Core.Activators
{
    public sealed class UTCActivator
        : IFeatureActivator, IDiscoverableActivatorParameters
    {
        private readonly ILogger<XabarilModule> _logger;
        private readonly IRuntimeParameterAccessor _runtimeParameterAccessor;

        List<ActivatorParameterDescriptor> _descriptors = new List<ActivatorParameterDescriptor>()
        {
             new ActivatorParameterDescriptor() {Name = "release-date", ClrType=typeof(String).Name , IsOptional = false,ActivatorName = typeof(UTCActivator).Name},
             new ActivatorParameterDescriptor() {Name = "format-date", ClrType=typeof(String).Name ,IsOptional = true,ActivatorName = typeof(UTCActivator).Name},
        };

        public IEnumerable<ActivatorParameterDescriptor> Descriptors => _descriptors;

        public UTCActivator(ILogger<XabarilModule> logger,
            IRuntimeParameterAccessor runtimeParameterAccessor)
        {
            _logger = logger;
            _runtimeParameterAccessor = runtimeParameterAccessor;
        }

        public async Task<bool> IsActiveAsync(string featureName)
        {
            bool active = false;
            DateTime configuredDate;

            var releaseDate = await _runtimeParameterAccessor
                .GetValueAsync<string>(featureName, _descriptors[0]);

            var format = await _runtimeParameterAccessor
                .GetValueAsync<string>(featureName, _descriptors[1]);

            if (releaseDate != null)
            {
                if (format != null)
                {
                    configuredDate = DateTime.ParseExact((string)releaseDate,
                        (string)format,
                        null,
                        DateTimeStyles.AssumeUniversal);
                }
                else
                {
                    _logger.LogInformation($"The value for format date is not specified on UTCActivator, the current culture format is applied");

                    configuredDate = DateTime.Parse((string)releaseDate, null,
                        DateTimeStyles.AssumeUniversal);
                }

                active = configuredDate <= DateTime.UtcNow;
            }
            else
            {
                _logger.LogWarning($"The UTCActivator can't parse {releaseDate} as valid date time");
            }

            return active;
        }
    }
}
