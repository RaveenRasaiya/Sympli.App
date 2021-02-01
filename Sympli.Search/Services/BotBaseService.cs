using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Sympli.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Sympli.Search.Services
{
    public abstract class BotBaseService : IBotService
    {
        public abstract string EngineUri { get; }
        public abstract string SearchPattern { get; }
        public abstract List<int> GetPositions(Uri uri, List<Match> matches);
        protected readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;
        public BotBaseService(ILogger<BotBaseService> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<string> GetPositions(string url, string keywords, int noOfPagesToScan)
        {
            ValidateParameter(url, keywords, noOfPagesToScan);

            var fullyQualifiedUrl = url.ToLower().StartsWith("http") ? url : $"http://{url}";
            var positions = await GetPageContent(keywords, new Uri(fullyQualifiedUrl), noOfPagesToScan);

            if (!positions.Any())
            {
                return "0";
            }
            return string.Join(", ", positions);
        }

        private void ValidateParameter(string url, string keyWords, int noOfPagesToScan)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (string.IsNullOrWhiteSpace(keyWords))
            {
                throw new ArgumentNullException(nameof(keyWords));
            }

            if (noOfPagesToScan <= 0)
            {
                throw new ArgumentException($"Number of pages to scan should be greate than zero");
            }
        }

        private async Task<List<int>> GetPageContent(string keywords, Uri uri, int noOfPagesToScan)
        {
            var searchUrl = string.Format(EngineUri, noOfPagesToScan, HttpUtility.UrlEncode(keywords));
            var searchResultRegex = $"({SearchPattern})(http+[a-zA-Z0-9--?=/]*)";

            try
            {
                string content = string.Empty;
                if (!_memoryCache.TryGetValue(searchUrl.ToUpper(), out content))
                {
                    var httpClient = HttpClientFactory.Create();

                    content = await httpClient.GetStringAsync(searchUrl);

                    _memoryCache.Set(searchUrl.ToUpper(), content);
                }

                var matches = Regex.Matches(content, searchResultRegex);

                return GetPositions(uri, matches.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null;
        }
    }
}
