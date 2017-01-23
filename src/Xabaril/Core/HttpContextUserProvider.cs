using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Xabaril.Core
{
    public class HttpContextUserProvider
        : IUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccesor;

        public HttpContextUserProvider(IHttpContextAccessor httpContextAccesor)
        {
            _httpContextAccesor = httpContextAccesor;
        }

        public Task<string> GetUserNameAsync()
        {
            var httpContext = _httpContextAccesor.HttpContext;

            return Task.FromResult<string>(
                httpContext?.User?.Identity.Name);
        }
    }
}
