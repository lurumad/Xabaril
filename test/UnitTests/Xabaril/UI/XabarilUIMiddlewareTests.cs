using FluentAssertions;
using global::Xabaril.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Xabaril.UI
{
    public class xabaril_ui_middleware_should
    {
        [Fact]
        public async Task get_content_on_valid_request()
        {
            using( var server = new TestServer(CreateWebHostBuilder()))
            {
                var response = await server.CreateClient()
                    .GetAsync(XabarilBaseAddress);

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task do_nothing_for_non_xabaril_request()
        {
            using (var server = new TestServer(CreateWebHostBuilder()))
            {
                var response = await server.CreateClient()
                    .GetAsync("/any/other/route/");

                response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            }
        }

        [Fact]
        public async Task do_nothing_for_post_request()
        {
            using (var server = new TestServer(CreateWebHostBuilder()))
            {
                var response = await server.CreateClient()
                  .PostAsync(XabarilBaseAddress, new StringContent(string.Empty));

                response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            }
        }

        [Fact]
        public async Task do_nothing_for_put_request()
        {
            using (var server = new TestServer(CreateWebHostBuilder()))
            {
                var response = await server.CreateClient()
                  .PutAsync(XabarilBaseAddress, new StringContent(string.Empty));

                response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            }
        }

        [Fact]
        public async Task do_nothing_for_delete_request()
        {
            using (var server = new TestServer(CreateWebHostBuilder()))
            {
                var response = await server.CreateClient()
                  .DeleteAsync(XabarilBaseAddress);

                response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            }
        }

        WebHostBuilder CreateWebHostBuilder()
        {
            var hostBuilder = new WebHostBuilder();
            hostBuilder.UseStartup<DefaultStartup>();


            return hostBuilder;
        }

        string XabarilBaseAddress => new XabarilUIOptions().IndexAddress;

        private class DefaultStartup
        {
            public void ConfigureServices(IServiceCollection services)
            {
               
            }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
            {
                app.UseMiddleware<XabarilUIMiddleware>(new XabarilUIOptions());
                app.Run(async (context) =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    await Task.FromResult<object>(null);
                });
            }
        }
    }
}
