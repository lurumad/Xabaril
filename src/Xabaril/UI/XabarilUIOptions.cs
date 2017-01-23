using Xabaril.UI.Personalization;

namespace Xabaril.UI
{
    public class XabarilUIOptions
    {
        const string XABARIL_UI_DEFAULT_ROUTE = "xabaril/ui";

        public string BaseAddress { get; set; } = XABARIL_UI_DEFAULT_ROUTE;

        public string IndexAddress => $"{BaseAddress.Trim('/')}/index.html";

        internal IndexConfig IndexConfig { get; private set; } = new IndexConfig();

        public void InjectStylesheet(string path, string media = "screen")
        {
            IndexConfig.Stylesheets.Add(new StylesheetDescriptor { Href = path, Media = media });
        }
    }
}
