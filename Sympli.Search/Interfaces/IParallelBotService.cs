using Sympli.Core.Models;
using System.Threading.Tasks;

namespace Sympli.Search.Interfaces
{
    public interface IParallelBotService
    {
        Task<SearchResponseModel> Process(SearchRequestModel searchRequestModel);
    }
}