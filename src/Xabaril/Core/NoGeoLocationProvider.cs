using System.Threading.Tasks;

namespace Xabaril.Core
{
    public class NoGeoLocationProvider
        : IGeoLocationProvider
    {
        public Task<string> FindLocationAsync(string ipAddress)
        {
            return Task.FromResult<string>(null);
        }
    }
}
