using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Xabaril.Core
{
    public class HttpContextRoleProvider
        : IRoleProvider
    {
        private readonly IHttpContextAccessor _httpContextAccesor;

        public HttpContextRoleProvider(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor == null)
            {
                throw new ArgumentNullException(nameof(httpContextAccessor));
            }

            _httpContextAccesor = httpContextAccessor;
        }

        public Task<string> GetRoleAsync()
        {
            var httpContext = _httpContextAccesor.HttpContext;

            var claims = httpContext?.User?.Claims;

            if (claims != null)
            {
                var roleClaim =  claims.Where(c => c.Type == ClaimTypes.Role)
                    .FirstOrDefault();

                if (roleClaim != null)
                {
                    return Task.FromResult<string>(roleClaim.Value);
                }
            }

            return Task.FromResult<string>(null);
        }
    }
}
