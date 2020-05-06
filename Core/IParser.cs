using AngleSharp.Dom.Html;
using System.Collections;

namespace Parser.Core
{
    interface IParser
    {
        IEnumerable Parse(IHtmlDocument document, IParserSettings parserSettings);
    }
}
