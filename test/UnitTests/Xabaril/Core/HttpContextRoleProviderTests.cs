using FluentAssertions;
using global::Xabaril.Core;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Xabaril.Core
{
    public class http_context_role_provider_should
    {
        [Fact]
        public async Task get_null_when_user_is_not_authenticated()
        {
            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();

            var provider = new HttpContextRoleProvider(httpContextAccessor);

            (await provider.GetRoleAsync()).Should().BeNull();
        }

        [Fact]
        public async Task get_null_when_claim_role_not_exist()
        {
            var context = new DefaultHttpContext();
            context.User = GetIdentityWithoutRole();

            var httpContextAccessor = new HttpContextAccessor();
            httpContextAccessor.HttpContext = context;

            var provider = new HttpContextRoleProvider(httpContextAccessor);

            (await provider.GetRoleAsync()).Should().BeNull();
        }

        [Fact]
        public async Task get_the_claim_role_value_of_authenticated_user()
        {
            string currentRole = "stuff";

            var context = new DefaultHttpContext();
            context.User = GetIdentityWithRole(role:currentRole);

            var httpContextAccessor = new HttpContextAccessor();
            httpContextAccessor.HttpContext = context;

            var provider = new HttpContextRoleProvider(httpContextAccessor);

            (await provider.GetRoleAsync()).Should().Be(currentRole);
        }

        private ClaimsPrincipal GetIdentityWithRole(string role)
        {
            return new ClaimsPrincipal(
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,"some_user"),
                    new Claim(ClaimTypes.Role,role),
                }));
        }

        private ClaimsPrincipal GetIdentityWithoutRole()
        {
            return new ClaimsPrincipal(
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,"some_user"),
                }));
        }
    }
}
