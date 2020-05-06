using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Parser.Core
{
    class ParserWorker
    {
        readonly CancellationTokenSource source;
        readonly CancellationToken token;
        IParser parser;
        IParserSettings parserSettings;

        HtmlLoader loader;

        bool isActive;

        #region Properties

        public IParser Parser
        {
            get
            {
                return parser;
            }
            set
            {
                parser = value;
            }
        }

        public IParserSettings Settings
        {
            get
            {
                return parserSettings;
            }
            set
            {
                parserSettings = value;
                loader = new HtmlLoader(value);
            }
        }

        public bool IsActive
        {
            get
            {
                return isActive;
            }
        }

        #endregion

        public event Action<object, IEnumerable> OnNewData;
        public event Action<object> OnCompleted;

        public ParserWorker(IParser parser, IParserSettings parserSettings) 
        {
            this.parser = parser;
            this.parserSettings = parserSettings;
            source = new CancellationTokenSource();
            token = source.Token;
            loader = new HtmlLoader(parserSettings);
        }

        public void Start()
        {
            isActive = true;
            Worker(token);
        }

        public void Abort()
        {
            isActive = false;
            source.Cancel();
        }

        private async void Worker(CancellationToken token)
        {
            if (token.IsCancellationRequested) return;

            var source = await loader.GetSource();
            var domParser = new HtmlParser();

            var document = await domParser.ParseAsync(source);

            var result = parser.Parse(document, parserSettings);

            OnNewData?.Invoke(this, result);

            OnCompleted?.Invoke(this);
            isActive = false;
        }
    }
}
