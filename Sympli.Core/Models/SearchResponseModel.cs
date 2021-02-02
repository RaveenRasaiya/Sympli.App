using System.Collections.Generic;

namespace Sympli.Core.Models
{
    public class SearchResponseModel
    {
        public SearchResponseModel()
        {
            Result = new List<SearchResult>();
        }
        public IList<SearchResult> Result { get; set; }
    }
}
