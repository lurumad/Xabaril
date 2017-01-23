using System.Threading.Tasks;

namespace Xabaril.Core
{
    public interface IRoleProvider
    {
        Task<string> GetRoleAsync();
    }
}
