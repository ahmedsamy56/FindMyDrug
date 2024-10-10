using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMyDrug.Utilities
{
    public static class GetSrc
    {

        public static string ForLocation(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var iframeNode = doc.DocumentNode.SelectSingleNode("//iframe");
            return iframeNode?.GetAttributeValue("src", null);
        }
    }
}
