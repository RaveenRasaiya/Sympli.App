namespace Sympli.Search.Helpers
{
    public static class AppHelper
    {
        public static string GetKey(string engine, string keyword, string targetUrl)
        {
            return $"{engine.Trim()}_{keyword.Trim()}_{targetUrl.Trim()}";
        }
    }
}
