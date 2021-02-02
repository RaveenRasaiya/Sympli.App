using Sympli.Search.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sympli.Search.Providers
{
    public class HttpApiClient : IHttpApiClient
    {
        public async Task<string> GetWebContent(string url)
        {
            try
            {
                var httpClient = HttpClientFactory.Create();

                return await httpClient.GetStringAsync(url);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
