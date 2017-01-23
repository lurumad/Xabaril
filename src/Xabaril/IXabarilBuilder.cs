using Microsoft.Extensions.DependencyInjection;

namespace Xabaril
{
    public interface IXabarilBuilder
    {
        IServiceCollection Services { get; }
    }
}
