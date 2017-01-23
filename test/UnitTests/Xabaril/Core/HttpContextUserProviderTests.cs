using FluentAssertions;
using global::Xabaril.Core;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Xabaril.Core
{
    public class http_context_user_provider_should
    {
        [Fact]
        public async Task get_null_if_user_is_not_authenticated()
        {
            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();

            var provider = new HttpContextUserProvider(httpContextAccessor);

            (await provider.GetUserNameAsync()).Should().BeNull();
        }

        [Fact]
        public async Task get_the_claim_name_of_authenticated_user()
        {
            var username = "some_user";

            var context = new DefaultHttpContext();
            context.User = GetIdentityWithName(username);

            var httpContextAccessor = new HttpContextAccessor();
            httpContextAccessor.HttpContext = context;

            var provider = new HttpContextUserProvider(httpContextAccessor);

            (await provider.GetUserNameAsync()).Should().Be(username);
        }

        private ClaimsPrincipal GetIdentityWithName(string user)
        {
            return new ClaimsPrincipal(
                new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,user)
                }));
        }
    }
}
