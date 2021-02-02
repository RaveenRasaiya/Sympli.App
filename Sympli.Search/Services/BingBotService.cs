using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sympli.Core.Models;
using Sympli.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sympli.Search.Services
{
    public class BingBotService : BotBaseService, IBingBotService
    {
        private readonly SearchSettings _searchSettings;
        public BingBotService(ILogger<BingBotService> logger, IOptions<SearchSettings> options, IHttpApiClient httpApiClient) : base(logger, httpApiClient)
        {
            _searchSettings = options.Value;
        }
        public override string EngineUrl
        {
            get
            {
                return _searchSettings.Bing.Url;
            }
        }

        public override string RowLookPattern
        {
            get
            {
                return _searchSettings.Bing.RowLookPattern;
            }
        }

        public override List<int> GetPositions(Uri uri, List<Match> matches)
        {
            return matches.Where(m => m.Groups[2].Value.Contains(uri.Host))
               .Select(m => matches.IndexOf(m) + 1)
               .ToList();
        }
    }
}