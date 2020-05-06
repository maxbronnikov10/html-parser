using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using AngleSharp.Dom.Html;

namespace Parser.Core.Habra
{
    class DefaultParser : IParser
    {
        public IEnumerable Parse(IHtmlDocument document, IParserSettings parserSettings)
        {
            var list  = new List<string>();
            var items = document.QuerySelectorAll(parserSettings.Selector)
                .Where(item => item.ClassName == (string.IsNullOrEmpty(parserSettings.ClassName) ? item.ClassName : parserSettings.ClassName));

            foreach(var item in items)
            {
                yield return item.TextContent;
            }
        }
    }
}
