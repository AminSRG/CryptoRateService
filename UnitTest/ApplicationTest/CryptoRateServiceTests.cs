using CryptoRateService.Application.Factories.Interfaces;
using CryptoRateService.Domain.Exceptions;
using CryptoRateService.Integrations.Repositories.Interfaces;
using Moq;

namespace UnitTest.ApplicationTest
{
    public class CryptoRateServiceTests
    {
        private readonly Mock<ICryptoQuoteRepositoryFactory> _repositoryFactoryMock;
        private readonly Mock<ICryptoQuoteRepository> _cryptoQuoteRepositoryMock;
        private readonly CryptoRateService.Application.Services.CryptoRateService _cryptoRateService;

        public CryptoRateServiceTests()
        {
            _repositoryFactoryMock = new Mock<ICryptoQuoteRepositoryFactory>();
            _cryptoQuoteRepositoryMock = new Mock<ICryptoQuoteRepository>();

            _repositoryFactoryMock.Setup(factory => factory.CreateRepository())
                .Returns(_cryptoQuoteRepositoryMock.Object);

            _cryptoRateService = new CryptoRateService.Application.Services.CryptoRateService(_repositoryFactoryMock.Object);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsExchangeRates_WhenValidSymbolIsProvided()
        {
            var symbol = "BTC";
            var expectedRates = new Dictionary<string, double>
            {
                { "USD", 45000 },
                { "EUR", 38000 }
            };

            _cryptoQuoteRepositoryMock.Setup(repo => repo.GetCryptoQuotesAsync(symbol, It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(expectedRates);

            var result = await _cryptoRateService.GetExchangeRatesAsync(symbol);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Count);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsFail_WhenSymbolNotFound()
        {
            var symbol = "INVALID_SYMBOL";
            _cryptoQuoteRepositoryMock.Setup(repo => repo.GetCryptoQuotesAsync(symbol, It.IsAny<IEnumerable<string>>()))
                .ThrowsAsync(new SymbolNotFoundException(symbol));

            var result = await _cryptoRateService.GetExchangeRatesAsync(symbol);

            Assert.False(result.IsSuccess);
            Assert.True(result.Errors.Where(e => e.Message == "Symbol 'INVALID_SYMBOL' not found!.").Any());
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsFail_WhenCurrencyNotFound()
        {
            var symbol = "BTC";
            var currencies = new[] { "USD", "EUR", "BRL", "GBP", "AUD" };
            _cryptoQuoteRepositoryMock.Setup(repo => repo.GetCryptoQuotesAsync(symbol, currencies))
                .ThrowsAsync(new CurrencyNotFoundException(symbol, "USD"));

            var result = await _cryptoRateService.GetExchangeRatesAsync(symbol);

            Assert.False(result.IsSuccess);
            Assert.True(result.Errors.Where(e => e.Message == "Currency 'USD' not found for symbol: BTC.").Any());
        }
    }
}
