using CryptoRateService.Application.Services.Interfaces;
using CryptoRateService.Domain.Entities;
using CryptoRateService.WebApi.Controllers;
using CryptoRateService.WebApi.ViewModels.V1.CryptoRates;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.WebApi
{
    public class CryptoRatesControllerTests
    {
        private readonly Mock<ICryptoRateService> _mockCryptoRateService;
        private readonly CryptoRatesController _controller;

        public CryptoRatesControllerTests()
        {
            _mockCryptoRateService = new Mock<ICryptoRateService>();
            _controller = new CryptoRatesController(_mockCryptoRateService.Object);
        }

        [Fact]
        public async Task GetCryptoRates_ShouldReturnOk_WhenServiceReturnsValidData()
        {
            // Arrange
            var request = new CryptoRatesInBodyVm { Symbol = "BTC" };
            var exchangeRates = new List<ExchangeRate>
        {
            new ExchangeRate("USD", 40000.0),
            new ExchangeRate("EUR", 35000.0)
        };

            _mockCryptoRateService
                .Setup(service => service.GetExchangeRatesAsync(request.Symbol))
                .ReturnsAsync(Result.Ok(exchangeRates));

            // Act
            var result = await _controller.GetCryptoRates(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<List<ExchangeRate>>(okResult.Value);
            Assert.Equal(exchangeRates, data);
        }

        [Fact]
        public async Task GetCryptoRates_ShouldReturnBadRequest_WhenServiceReturnsFailure()
        {
            // Arrange
            var request = new CryptoRatesInBodyVm { Symbol = "BTC" };
            var errorMessage = "Symbol not found";

            _mockCryptoRateService
                .Setup(service => service.GetExchangeRatesAsync(request.Symbol))
                .ReturnsAsync(Result.Fail<List<ExchangeRate>>(errorMessage));

            // Act
            var result = await _controller.GetCryptoRates(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(errorMessage, badRequestResult.Value);
        }

        [Fact]
        public async Task GetCryptoRates_ShouldReturnInternalServerError_WhenUnhandledErrorOccurs()
        {
            // Arrange
            var request = new CryptoRatesInBodyVm { Symbol = "BTC" };
            var errorMessage = "An error occurred while fetching exchange rates";

            _mockCryptoRateService
                .Setup(service => service.GetExchangeRatesAsync(request.Symbol))
                .ReturnsAsync(Result.Fail<List<ExchangeRate>>(errorMessage));

            // Act
            var result = await _controller.GetCryptoRates(request);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal(errorMessage, statusCodeResult.Value);
        }
    }
}