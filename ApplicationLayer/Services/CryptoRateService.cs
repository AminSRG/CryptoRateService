using CryptoRateService.Application.Factories.Interfaces;
using CryptoRateService.Application.Services.Interfaces;
using CryptoRateService.Domain.Entities;
using CryptoRateService.Domain.Exceptions;
using CryptoRateService.Integrations.Repositories.Interfaces;
using FluentResults;

namespace CryptoRateService.Application.Services
{
    public class CryptoRateService(ICryptoQuoteRepositoryFactory repositoryFactory) : ICryptoRateService
    {
        private readonly ICryptoQuoteRepository _cryptoQuoteRepository = repositoryFactory.CreateRepository();

        public async Task<Result<List<ExchangeRate>>> GetExchangeRatesAsync(string symbol)
        {
            try
            {
                var currencies = new[] { "USD", "EUR", "BRL", "GBP", "AUD" };
                var quotes = await _cryptoQuoteRepository.GetCryptoQuotesAsync(symbol, currencies);

                var exchangeRates = quotes.Select(q => new ExchangeRate(q.Key, q.Value)).ToList();

                return Result.Ok(exchangeRates);
            }
            catch (SymbolNotFoundException ex)
            {
                return Result.Fail<List<ExchangeRate>>(ex.Message);
            }
            catch (CurrencyNotFoundException ex)
            {
                return Result.Fail<List<ExchangeRate>>(ex.Message);
            }
            catch (Exception ex)
            {
                return Result.Fail<List<ExchangeRate>>($"An error occurred while fetching exchange rates: {ex.Message}");
            }
        }
    }
}
