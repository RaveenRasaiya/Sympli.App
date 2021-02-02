﻿using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
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
        public BotProviderTest()
        {
            _googleLogger = new Mock<ILogger<GoogleBotService>>();
            _bingLogger = new Mock<ILogger<BingBotService>>();
            _botProvider = new Mock<IBotProvider>();
            _httpClient = new Mock<IHttpApiClient>();
        }

        [Theory]
        [InlineData("google")]
        public void BotProvider_ValidEngine_Google(string engine)
        {
            _botProvider.Setup(provider => provider.GetBotService(engine)).Returns(new GoogleBotService(_googleLogger.Object, _httpClient.Object));
            var _provider = _botProvider.Object.GetBotService(engine);
            _provider.Should().NotBeNull();
            _provider.Should().BeOfType<GoogleBotService>();
        }


        [Theory]
        [InlineData("bing")]
        public void BotProvider_ValidEngine_Bing(string engine)
        {
            _botProvider.Setup(provider => provider.GetBotService(engine)).Returns(new BingBotService(_bingLogger.Object, _httpClient.Object));
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