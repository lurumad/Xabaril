using Microsoft.Extensions.DependencyInjection;
using System;

namespace Xabaril
{
    internal sealed class XabarilBuilder
        : IXabarilBuilder
    {
        public IServiceCollection Services
        {
            get;
        }


        public XabarilBuilder(IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            Services = serviceCollection;
        }
    }
}
