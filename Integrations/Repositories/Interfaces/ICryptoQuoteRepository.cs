namespace CryptoRateService.Integrations.Repositories.Interfaces
{
    public interface ICryptoQuoteRepository
    {
        Task<Dictionary<string, double>> GetCryptoQuotesAsync(string symbol, IEnumerable<string> currencies);
    }
}
