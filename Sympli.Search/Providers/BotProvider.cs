using Sympli.Search.Interfaces;

namespace Sympli.Search.Providers
{
    public class BotProvider : IBotProvider
    {
        private readonly IGoogleBotService _googleBotService;
        private readonly IBingBotService _bingBotService;

        public BotProvider(IGoogleBotService googleBotService, IBingBotService bingBotService)
        {
            _googleBotService = googleBotService;
            _bingBotService = bingBotService;
        }
        public IBotService GetBotService(string searchEngine)
        {
            switch (searchEngine.ToLower())
            {
                case "google":
                    return _googleBotService;

                case "bing":
                    return _bingBotService;
                default:
                    return null;
            }
        }
    }
}
