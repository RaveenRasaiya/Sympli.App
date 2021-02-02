namespace Sympli.Core.Models
{
    public class SearchSettings
    {
        public int NoOfResultsToScan { get; set; }
        public SearchEngine Google { get; set; }
        public SearchEngine Bing { get; set; }
    }
}
