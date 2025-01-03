namespace CryptoRateService.Domain.Entities
{
    public class ExchangeRate
    {
        public string CurrencyCode { get; private set; }
        public double Rate { get; private set; }

        public ExchangeRate(string currencyCode, double rate)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
                throw new ArgumentException("Currency code cannot be null or empty.");

            if (rate <= 0)
                throw new ArgumentException("Exchange rate must be greater than zero.");

            CurrencyCode = currencyCode.ToUpperInvariant();
            Rate = rate;
        }
    }
}
