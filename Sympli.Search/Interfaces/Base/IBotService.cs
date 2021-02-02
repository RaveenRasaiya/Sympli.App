using System.Threading.Tasks;

namespace Sympli.Search.Interfaces
{
    public interface IBotService
    {
        Task<string> GetPositions(string url, string keywords, int noOfResults);
    }
}