using System.Threading.Tasks;

namespace Xabaril.Core
{
    public interface IUserProvider
    {
        Task<string> GetUserNameAsync();
    }
}
