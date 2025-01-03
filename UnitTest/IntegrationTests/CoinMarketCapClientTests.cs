using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CryptoRateService.Integrations.ExternalClients;
using CryptoRateService.Integrations.Model;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace UnitTest.IntegrationTests
{
    public class CoinMarketCapClientTests
    {
        private readonly Mock<HttpMessageHandler> _mockHandler;
        private readonly CoinMarketCapClient _coinMarketCapClient;
        private readonly string _apiKey = "fake-api-key";
        private readonly string _baseUrl = "https://pro-api.coinmarketcap.com";
        private readonly string _cryptoQuotesEndpoint = "/v1/cryptocurrency/quotes/latest";

        public CoinMarketCapClientTests()
        {
            _mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var httpClient = new HttpClient(_mockHandler.Object);
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(c => c["CoinMarketCap:BaseUrl"]).Returns(_baseUrl);
            configuration.Setup(c => c["CoinMarketCap:CryptoQuotesEndpoint"]).Returns(_cryptoQuotesEndpoint);
            configuration.Setup(c => c["CoinMarketCap:ApiKey"]).Returns(_apiKey);

            _coinMarketCapClient = new CoinMarketCapClient(configuration.Object, httpClient);
        }

        [Fact]
        public async Task GetCryptoQuoteAsync_ShouldReturnValidResponse_WhenApiCallIsSuccessful()
        {
            var symbol = "BTC";
            var convert = "USD";
            var mockResponse = new CoinMarketCapResponse
            {
                Status = new StatusInfo
                {
                    ErrorCode = 0,
                    ErrorMessage = "",
                },
                Data = new Dictionary<string, CoinDetails>
                {
                    {
                        symbol, new CoinDetails
                        {
                            Quote = new Dictionary<string, CurrencyDetails>
                            {
                                { convert, new CurrencyDetails { Price = 40000.0 } }
                            }
                        }
                    }
                }
            };

            var jsonResponse = JsonConvert.SerializeObject(mockResponse);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            };

            _mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponseMessage)
                .Verifiable();

            var result = await _coinMarketCapClient.GetCryptoQuoteAsync(symbol, convert);

            Assert.NotNull(result);
            Assert.Equal(symbol, result.Data.Keys.First());
            Assert.Equal(40000.0, result.Data[symbol].Quote[convert].Price);
        }

        [Fact]
        public async Task GetCryptoQuoteAsync_ShouldThrowException_WhenApiCallFails()
        {
            var symbol = "BTC";
            var convert = "USD";

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Bad Request")
            };

            _mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponseMessage)
                .Verifiable();

            var exception = await Assert.ThrowsAsync<Exception>(() => _coinMarketCapClient.GetCryptoQuoteAsync(symbol, convert));
            Assert.Equal("Error: BadRequest, Bad Request", exception.Message);
        }

        [Fact]
        public async Task GetCryptoQuoteAsync_ShouldThrowException_WhenDataNotFound()
        {
            var symbol = "INVALID";
            var convert = "USD";
            var mockResponse = new CoinMarketCapResponse
            {
                Status = new StatusInfo
                {
                    ErrorCode = 1,
                    ErrorMessage = "Data not found",
                },
                Data = null
            };

            var jsonResponse = JsonConvert.SerializeObject(mockResponse);
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            };

            _mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(httpResponseMessage)
                .Verifiable();

            var exception = await Assert.ThrowsAsync<Exception>(() => _coinMarketCapClient.GetCryptoQuoteAsync(symbol, convert));
            Assert.Equal("Error Code: 1, Message: Data not found", exception.Message);
        }
    }
}
