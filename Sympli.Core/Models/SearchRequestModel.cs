using System.Collections.Generic;

namespace Sympli.Core.Models
{
    public class SearchRequestModel
    {
        public string Keyword { get; set; }

        public string TargetUrl { get; set; }

        public IEnumerable<string> SearchEngines { get; set; }
    }
}
