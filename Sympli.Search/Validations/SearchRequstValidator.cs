using Sympli.Core.Models;
using Sympli.Search.Interfaces;
using System.Linq;

namespace Sympli.Search.Services
{
    public class SearchRequstValidator : ISearchRequstValidator
    {
        public string IsValid(SearchRequestModel request)
        {
            if (request == null)
            {
                return $"request can't empty";
            }
            if (string.IsNullOrEmpty(request.Keyword))
            {
                return $"{nameof(request.TargetUrl)} can't be empty";
            }
            if (string.IsNullOrEmpty(request.TargetUrl))
            {
                return $"{nameof(request.TargetUrl)} can't be empty";
            }
            if (request.SearchEngines == null || !request.SearchEngines.Any())
            {
                return $"{nameof(request.SearchEngines)} can't be empty";
            }
            return string.Empty;
        }
    }
}