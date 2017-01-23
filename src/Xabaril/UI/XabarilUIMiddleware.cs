using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Xabaril.UI
{
    public class XabarilUIMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly XabarilUIOptions _options;
        private readonly TemplateMatcher _requestPathMatcher;
        private readonly Assembly _xabarilAssembly;

        public XabarilUIMiddleware(RequestDelegate next, XabarilUIOptions options)
        {
            _next = next;
            _options = options;
            _requestPathMatcher = new TemplateMatcher(
                TemplateParser.Parse(options.IndexAddress),
                new RouteValueDictionary());
            _xabarilAssembly = GetType().GetTypeInfo().Assembly;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!IsUIXabarilRequest(context.Request))
            {
                await _next.Invoke(context);

                return;
            }

            using (var stream = _xabarilAssembly.GetManifestResourceStream("Xabaril.UI.Assets.Index.html"))
            {
                var response = context.Response;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.ContentType = "text/html";

                using (var contentStream = GenerateContent(stream, _options.IndexConfig.AsBookmarksDictionary()))
                {
                    contentStream.CopyTo(response.Body);
                }  
            }
        }

        private Stream GenerateContent(Stream template, IDictionary<string, string> bookmarks)
        {
            using (var templateReader = new StreamReader(template))
            {
                var templateContent = templateReader.ReadToEnd();

                foreach (var bookmark in bookmarks)
                {
                    templateContent.Replace(bookmark.Key, bookmark.Value);
                }

                return new MemoryStream(Encoding.UTF8.GetBytes(templateContent));
            }
        }

        private bool IsUIXabarilRequest(HttpRequest request)
        {
            return request.Method == HttpMethods.Get
                &&
                _requestPathMatcher.TryMatch(request.Path, new RouteValueDictionary());
        }
    }
}
