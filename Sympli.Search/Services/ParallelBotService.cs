using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Sympli.Core.Models;
using Sympli.Search.Helpers;
using Sympli.Search.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sympli.Search.Services
{
    public class ParallelBotService : IParallelBotService
    {
        private readonly IBotProvider _botProvider;
        private readonly IMemoryCache _memoryCache;
        private readonly SearchSettings _settings;

        public ParallelBotService(IBotProvider botProvider, IMemoryCache memoryCache, IOptions<SearchSettings> settings)
        {
            _settings = settings.Value;
            _botProvider = botProvider;
            _memoryCache = memoryCache;
        }
        public async Task<SearchResponseModel> Process(SearchRequestModel searchRequestModel)
        {
            SearchResponseModel response = new SearchResponseModel();
            var tasks = new List<Task>(searchRequestModel.SearchEngines.Count());
            string positions = "0";
            foreach (var engine in searchRequestModel.SearchEngines)
            {
                _memoryCache.TryGetValue(AppHelper.GetKey(engine, searchRequestModel.Keyword, searchRequestModel.TargetUrl), out positions);
                if (!string.IsNullOrEmpty(positions))
                {
                    response.Result.Add(new SearchResult
                    {
                        Engine = engine,
                        PagePositions = positions,
                    });
                }
                else
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        var _botService = _botProvider.GetBotService(engine);
                        positions = await _botService?.GetPositions(searchRequestModel.TargetUrl, searchRequestModel.Keyword, _settings.NoOfResultsToScan);
                        _memoryCache.Set(AppHelper.GetKey(engine, searchRequestModel.Keyword, searchRequestModel.TargetUrl), positions);
                        response.Result.Add(new SearchResult
                        {
                            Engine = engine,
                            PagePositions = positions,
                        });
                    }));
                }
            }
            if (tasks.Any())
            {
                Task.WaitAll(tasks.ToArray());
            }
            return response;
        }
    }
}
