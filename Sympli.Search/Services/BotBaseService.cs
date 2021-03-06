﻿using Microsoft.Extensions.Logging;
using Sympli.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Sympli.Search.Services
{
    public abstract class BotBaseService : IBotService
    {
        public abstract string EngineUrl { get; }
        public abstract string RowLookPattern { get; }
        public abstract List<int> GetPositions(Uri uri, List<Match> matches);
        protected readonly ILogger _logger;
        private readonly IHttpApiClient _httpApiClient;
        public BotBaseService(ILogger<BotBaseService> logger, IHttpApiClient httpApiClient)
        {
            _logger = logger;
            _httpApiClient = httpApiClient;
        }

        public async Task<string> GetPositions(string url, string keywords, int noOfResults)
        {
            ValidInput(url, keywords, noOfResults);

            var fullyQualifiedUrl = url.ToLower().StartsWith("http") ? url : $"http://{url}";

            var positions = await GetPageContent(keywords, new Uri(fullyQualifiedUrl), noOfResults);

            if (positions != null && positions.Any())
            {
                return string.Join(", ", positions);
            }
            return "0";
        }

        private void ValidInput(string url, string keywords, int noOfResults)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (string.IsNullOrEmpty(keywords))
            {
                throw new ArgumentNullException(nameof(keywords));
            }

            if (noOfResults <= 0)
            {
                throw new ArgumentException($"Number of results to scan should be greate than zero");
            }
        }

        private async Task<IEnumerable<int>> GetPageContent(string keywords, Uri uri, int noOfResults)
        {
            try
            {
                var searchUrl = string.Format(EngineUrl, noOfResults, HttpUtility.UrlEncode(keywords));

                var searchResultRegex = $"({RowLookPattern})(http+[a-zA-Z0-9--?=/]*)";

                string content = await _httpApiClient.GetWebContent(searchUrl);

                var matches = Regex.Matches(content, searchResultRegex,RegexOptions.Multiline);

                return GetPositions(uri, matches.ToList());

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
    }
}
