using Microsoft.Extensions.Options;
using Sympli.Core.Models;
using Sympli.Search.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sympli.Search.Services
{
    public class ParallelBotService : IParallelBotService
    {
        private readonly IBotProvider _botProvider;
        private readonly SearchSettings _settings;
        public ParallelBotService(IBotProvider botProvider, IOptions<SearchSettings> settings)
        {
            _settings = settings.Value;
            _botProvider = botProvider;

        }
        public async Task<SearchResponseModel> Process(SearchRequestModel searchRequestModel)
        {
            SearchResponseModel response = new SearchResponseModel();
            var tasks = new List<Task>(searchRequestModel.SearchEngines.Count());
            foreach (var engine in searchRequestModel.SearchEngines)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var _botService = _botProvider.GetBotService(engine);
                    if (_botService != null)
                    {
                        var positions = await _botService.GetPositions(searchRequestModel.TargetUrl, searchRequestModel.Keyword, _settings.NoOfPagesToScan);
                        response.Result ??= new List<SearchResult>();
                        response.Result.Add(new SearchResult
                        {
                            Engine = engine,
                            PagePositions = positions,
                        });
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());
            return response;
        }
    }
}
