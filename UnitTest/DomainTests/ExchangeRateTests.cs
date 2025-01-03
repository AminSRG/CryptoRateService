using CryptoRateService.Domain.Entities;
using FluentAssertions;

namespace UnitTest.DomainTests
{
    public class ExchangeRateTests
    {
        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenCurrencyCodeIsNullOrWhiteSpace()
        {
            string invalidCurrencyCode = string.Empty;
            double rate = 100.5;

            Action act = () => new ExchangeRate(invalidCurrencyCode, rate);

            act.Should().Throw<ArgumentException>()
                .WithMessage("Currency code cannot be null or empty.");
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenRateIsZeroOrNegative()
        {
            string validCurrencyCode = "USD";
            double invalidRate = -1;

            Action act = () => new ExchangeRate(validCurrencyCode, invalidRate);

            act.Should().Throw<ArgumentException>()
                .WithMessage("Exchange rate must be greater than zero.");
        }

        [Fact]
        public void Constructor_ShouldCreateExchangeRate_WhenValidArgumentsAreProvided()
        {
            string validCurrencyCode = "USD";
            double validRate = 100.5;

            var exchangeRate = new ExchangeRate(validCurrencyCode, validRate);

            exchangeRate.CurrencyCode.Should().Be("USD");
            exchangeRate.Rate.Should().Be(100.5);
        }
    }
}
