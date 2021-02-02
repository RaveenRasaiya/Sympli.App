using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Sympli.Core.Models;
using Sympli.Search.Interfaces;
using Sympli.Search.Services;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sympli.Tests
{
    public class BotServiceTest
    {
        private readonly IOptions<SearchSettings> _options;
        private SearchSettings _settings;


        private readonly Mock<IHttpApiClient> _httpApiClient;
        private readonly Mock<IBotProvider> _botProvider;
        private readonly Mock<ILogger<GoogleBotService>> _googleLogger;
        private readonly Mock<ILogger<BingBotService>> _bingLogger;
        public BotServiceTest()
        {
            InitConfig();
            _options = Options.Create(_settings);
            _botProvider = new Mock<IBotProvider>();
            _googleLogger = new Mock<ILogger<GoogleBotService>>();
            _bingLogger = new Mock<ILogger<BingBotService>>();
            _httpApiClient = new Mock<IHttpApiClient>();
        }

        private void InitConfig()
        {
            _settings = new SearchSettings
            {
                NoOfResultsToScan = 10,
                Bing = new SearchEngine
                {
                    Url = "https://www.bing.com/search?count={0}&q={1}",
                    RowLookPattern = "<li class=\"b_algo\" data-bm=\"[0-9]+\"><h2><a href=\""
                },
                Google = new SearchEngine
                {
                    Url = "https://www.google.com/search?num={0}&q={1}",
                    RowLookPattern = "<div class=\"kCrYT\"><a href=\"/url\\?q="
                }
            };
        }

        [Theory]
        [InlineData("engine", "e-settlements", "www.sympli.com.au")]
        public async Task BotService_Google_Search_NoContent(string engine, string keywords, string targetUrl)
        {
            _botProvider.Setup(provider => provider.GetBotService(engine)).Returns(new GoogleBotService(_googleLogger.Object, _options, _httpApiClient.Object));
            var _searchService = _botProvider.Object.GetBotService(engine);
            var positions = await _searchService.GetPositions(targetUrl, keywords, _settings.NoOfResultsToScan);
            positions.Should().Be("0");
        }

        [Theory]
        [InlineData("google", "e-settlements", "www.sympli.com.au", "1, 2")]
        public async Task BotService_Google_Search_WithContent(string engine, string keywords, string targetUrl, string expectedResult)
        {
            _httpApiClient.Setup(client => client.GetWebContent(It.IsAny<string>())).ReturnsAsync(GetMockGoogleContent());
            _botProvider.Setup(provider => provider.GetBotService(engine)).Returns(new GoogleBotService(_googleLogger.Object, _options, _httpApiClient.Object));
            var _searchService = _botProvider.Object.GetBotService(engine);
            var positions = await _searchService.GetPositions(targetUrl, keywords, _settings.NoOfResultsToScan);
            positions.Should().NotBeNullOrEmpty();
            positions.Should().Be(expectedResult);
        }
        [Theory]
        [InlineData("bing", "e-settlements", "www.sympli.com.au")]
        public async Task BotService_Bing_Search_NoContent(string engine, string keywords, string targetUrl)
        {
            _botProvider.Setup(provider => provider.GetBotService(engine)).Returns(new BingBotService(_bingLogger.Object, _options, _httpApiClient.Object));
            var _searchService = _botProvider.Object.GetBotService(engine);
            var positions = await _searchService.GetPositions(targetUrl, keywords, _settings.NoOfResultsToScan);
            positions.Should().Be("0");
        }

        [Theory]
        [InlineData("bing", "e-settlements", "www.sympli.com.au", "1, 3")]
        public async Task BotService_Bing_Search_WithContent(string engine, string keywords, string targetUrl, string expectedResult)
        {
            _httpApiClient.Setup(client => client.GetWebContent(It.IsAny<string>())).ReturnsAsync(GetMockBingContent());
            _botProvider.Setup(provider => provider.GetBotService(engine)).Returns(new BingBotService(_bingLogger.Object, _options, _httpApiClient.Object));
            var _searchService = _botProvider.Object.GetBotService(engine);
            var positions = await _searchService.GetPositions(targetUrl, keywords, _settings.NoOfResultsToScan);
            positions.Should().NotBeNullOrEmpty();
            positions.Should().Be(expectedResult);
        }

        public string GetMockBingContent()
        {
            var builder = new StringBuilder();
            builder.Append("<li class=\"b_algo\" data-bm=\"25\"><h2><a href=\"https://www.sympli.com.au/\" h=\"ID=SERP,5391.1\" class=\"\">e-Settlement Services | Sympli - Making e-Settlements Simple</a></h2></li>");
            builder.Append("<li class=\"b_algo\" data-bm=\"20\"><h2><a href=\"https://www.otherdoman.com.au/e-settlement-services/\" h=\"ID=SERP,5391.1\" class=\"\">Nice domain</a></h2></li>");
            builder.Append("<li class=\"b_algo\" data-bm=\"40\"><h2><a href=\"https://www.sympli.com.au/\" h=\"ID=SERP,5391.1\" class=\"\">e-Settlement Services | Sympli - Making e-Settlements Simple</a></h2></li>");
            builder.Append("<li class=\"b_algo\" data-bm=\"39\"><h2><a href=\"https://www.worldopn.com.au/e-settlement-services/\" h=\"ID=SERP,5391.1\" class=\"\">Open world</a></h2></li>");
            var content = builder.ToString().Trim().Replace("\t", "");
            return content;
        }
        public string GetMockGoogleContent()
        {
            var builder = new StringBuilder();
            builder.Append("< div class=\"g\">");
            builder.Append("	<div class=\"tF2Cxc\" data-hveid=\"CCYQAA\" data-ved=\"2ahUKEwiFjtzqpcruAhXEuZ4KHSwFBEMQFSgAMA16BAgmEAA\">");
            builder.Append("		<div class=\"kCrYT\">");
            builder.Append("			<a href=\"/url?q=https://www.sympli.com.au\">");
            builder.Append("				<br>");
            builder.Append("					<h3 class=\"LC20lb DKV0Md\">");
            builder.Append("						<span>4 Things You Should Know About Electronic Settlements</span>");
            builder.Append("					</h3>");
            builder.Append("					<div class=\"TbwUpd NJjxre\">");
            builder.Append("						<cite class=\"iUh30 Zu0yb qLRx3b tjvcx\">www.firsttitle.com.au");
            builder.Append("							");
            builder.Append("							<span class=\"dyjrff qzEoUe\">");
            builder.Append("								<span> › blogs › 4-things-you-should-kn...</span>");
            builder.Append("							</span>");
            builder.Append("						</cite>");
            builder.Append("					</div>");
            builder.Append("				</a>");
            builder.Append("			</div>");
            builder.Append("			<div class=\"IsZvec\">");
            builder.Append("			</div>");
            builder.Append("		</div>");
            builder.Append("	</div>");
            builder.Append("< div class=\"g\">");
            builder.Append("<div class=\"kCrYT\"></div>");
            builder.Append("			<a href=\"/url?q=https://www.otherwebsite.com.au\"/>");
            builder.Append("	</div>");
            builder.Append("< div class=\"g\">");
            builder.Append("	<div class=\"tF2Cxc\" data-hveid=\"CCYQAA\" data-ved=\"2ahUKEwiFjtzqpcruAhXEuZ4KHSwFBEMQFSgAMA16BAgmEAA\">");
            builder.Append("		<div class=\"kCrYT\">");
            builder.Append("			<a href=\"/url?q=https://www.sympli.com.au\">");
            builder.Append("				<br>");
            builder.Append("					<h3 class=\"LC20lb DKV0Md\">");
            builder.Append("						<span>4 Tsdsad Electronic Settlements</span>");
            builder.Append("					</h3>");
            builder.Append("					<div class=\"TbwUpd NJjxre\">");
            builder.Append("						<cite class=\"iUh30 Zu0yb qLRx3b tjvcx\">www.sympli.com.au");
            builder.Append("							");
            builder.Append("							<span class=\"dyjrff qzEoUe\">");
            builder.Append("								<span> › blogs › 4-things-you-should-kn...</span>");
            builder.Append("							</span>");
            builder.Append("						</cite>");
            builder.Append("					</div>");
            builder.Append("				</a>");
            builder.Append("			</div>");
            builder.Append("			<div class=\"IsZvec\">");
            builder.Append("			</div>");
            builder.Append("		</div>");
            builder.Append("	</div>");

            var content = builder.ToString().Trim().Replace("\t", "");
            return content;
        }
    }
}
