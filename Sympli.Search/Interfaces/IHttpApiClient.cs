using System.Threading.Tasks;

namespace Sympli.Search.Interfaces
{
    public interface IHttpApiClient
    {
        Task<string> GetWebContent(string url);
    }
}
