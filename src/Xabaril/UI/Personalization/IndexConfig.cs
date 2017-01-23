using System.Collections.Generic;
using System.Text;

namespace Xabaril.UI.Personalization
{
    public class IndexConfig
    {
        const string CSS_BOOKMARK = "%(StylesheetsHtml)";

        public IList<StylesheetDescriptor> Stylesheets  = new List<StylesheetDescriptor>();
      
        public IDictionary<string, string> AsBookmarksDictionary()
        {
            return new Dictionary<string, string>
            {
                { CSS_BOOKMARK, GetStylesheetsHtml() }
            };
        }

        private string GetStylesheetsHtml()
        {
            var builder = new StringBuilder();

            foreach (var css in Stylesheets)
            {
                builder.AppendLine($"<link href='{css.Href}' rel='stylesheet' media='{css.Media}' type='text/css' />");
            }

            return builder.ToString();
        }
    }
}
