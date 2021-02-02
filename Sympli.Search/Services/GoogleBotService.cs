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
    public class GoogleBotService : BotBaseService, IGoogleBotService
    {
        private readonly SearchSettings _searchSettings;
        public GoogleBotService(ILogger<GoogleBotService> logger, IOptions<SearchSettings> options, IHttpApiClient httpApiClient) : base(logger, httpApiClient)
        {
            _searchSettings = options.Value;
        }

        public override string EngineUrl
        {
            get
            {
                return _searchSettings.Google.Url;
            }
        }

        public override string RowLookPattern
        {
            get
            {
                return _searchSettings.Google.RowLookPattern;
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