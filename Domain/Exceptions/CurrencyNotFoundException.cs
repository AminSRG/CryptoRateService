namespace CryptoRateService.Domain.Exceptions
{
    public class CurrencyNotFoundException(string symbol, string currency) : Exception($"Currency '{currency}' not found for symbol: {symbol}.")
    {
    }
}
