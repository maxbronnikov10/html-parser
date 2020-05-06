
namespace Parser.Core.Habra
{
    class DefaultSettings : IParserSettings
    {
        public DefaultSettings(string baseUrl, string prefix, string selector, string className)
        {
            Selector = selector;
            ClassName = className;
            BaseUrl = baseUrl;
            Prefix = prefix;
        }

        public string BaseUrl { get; set; }

        public string Prefix { get; set; }

        public string Selector { get; set; }
        public string ClassName { get; set; }
    }
}
