using CryptoRateService.Domain.Exceptions;
using CryptoRateService.Integrations.ExternalClients;
using CryptoRateService.Integrations.Repositories.Interfaces;
using System;

namespace CryptoRateService.Integrations.Repositories
{
    public class CoinMarketCapRepository(CoinMarketCapClient coinMarketCapClient) : ICryptoQuoteRepository
    {
        private readonly CoinMarketCapClient _coinMarketCapClient = coinMarketCapClient;
        
        public async Task<Dictionary<string, double>> GetCryptoQuotesAsync(string symbol, IEnumerable<string> currencies)
        {
            var quotes = new Dictionary<string, double>();

            foreach (var currency in currencies)
            {
                var quoteResponse = await _coinMarketCapClient.GetCryptoQuoteAsync(symbol, currency);

                if (quoteResponse?.Data?.ContainsKey(symbol) != true)
                    throw new SymbolNotFoundException(symbol);

                var symbolData = quoteResponse.Data[symbol];

                if (symbolData?.Quote?.ContainsKey(currency) != true)
                    throw new CurrencyNotFoundException(symbol: symbol, currency: currency);

                var currencyDetails = symbolData.Quote[currency];

                quotes[currency] = currencyDetails.Price;
            }

            return quotes;
        }
    }

}
