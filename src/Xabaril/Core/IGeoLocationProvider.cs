using System.Threading.Tasks;

namespace Xabaril.Core
{
    public interface IGeoLocationProvider
    {
        Task<string> FindLocationAsync(string ipAddress);
    }
}
