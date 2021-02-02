using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Sympli.Core.Models;
using Sympli.Search.Interfaces;
using Sympli.Search.Services;
using Xunit;

namespace Sympli.Tests
{
    public class BotProviderTest
    {
        private readonly Mock<IBotProvider> _botProvider;
        private readonly Mock<ILogger<GoogleBotService>> _googleLogger;
        private readonly Mock<ILogger<BingBotService>> _bingLogger;
        private readonly Mock<IHttpApiClient> _httpClient;
        private readonly IOptions<SearchSettings> _options;
        private SearchSettings _settings;
        public BotProviderTest()
        {
            _googleLogger   = new Mock<ILogger<GoogleBotService>>();
            _bingLogger     = new Mock<ILogger<BingBotService>>();
            _botProvider    = new Mock<IBotProvider>();
            _httpClient     = new Mock<IHttpApiClient>();
            InitConfig();
            _options = Options.Create(_settings);
        }

        private void InitConfig()
        {
            _settings = new SearchSettings
            {
                NoOfResultsToScan = 100,
                Bing = new SearchEngine
                {

                },
                Google = new SearchEngine
                {

                }
            };
        }

        [Theory]
        [InlineData("google")]
        public void BotProvider_ValidEngine_Google(string engine)
        {
            _botProvider.Setup(provider => provider.GetBotService(engine)).Returns(new GoogleBotService(_googleLogger.Object, _options, _httpClient.Object));
            var _provider = _botProvider.Object.GetBotService(engine);
            _provider.Should().NotBeNull();
            _provider.Should().BeOfType<GoogleBotService>();
        }


        [Theory]
        [InlineData("bing")]
        public void BotProvider_ValidEngine_Bing(string engine)
        {
            _botProvider.Setup(provider => provider.GetBotService(engine)).Returns(new BingBotService(_bingLogger.Object, _options, _httpClient.Object));
            var _provider = _botProvider.Object.GetBotService(engine);
            _provider.Should().NotBeNull();
            _provider.Should().BeOfType<BingBotService>();
        }

        [Theory]
        [InlineData("")]
        public void BotProvider_ValidEngine_Notvalid(string engine)
        {
            _botProvider.Setup(provider => provider.GetBotService(engine)).Returns((IBotService)null);
            var _provider = _botProvider.Object.GetBotService(engine);
            _provider.Should().BeNull();
        }
    }
}
