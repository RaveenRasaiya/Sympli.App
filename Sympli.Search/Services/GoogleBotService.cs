using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Sympli.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sympli.Search.Services
{
    public class GoogleBotService : BotBaseService, IGoogleBotService
    {
        public GoogleBotService(ILogger<GoogleBotService> logger, IMemoryCache memoryCache) : base(logger, memoryCache)
        {

        }

        public override string EngineUri
        {
            get
            {
                return "https://www.google.com/search?num={0}&q={1}";
            }
        }

        public override string SearchPattern
        {
            get
            {
                return "<div class=\"kCrYT\"><a href=\"/url\\?q=";
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