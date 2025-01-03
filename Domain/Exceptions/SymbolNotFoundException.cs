namespace CryptoRateService.Domain.Exceptions
{
    public class SymbolNotFoundException(string symbol) : Exception($"Symbol '{symbol}' not found!.")
    {
    }
}
