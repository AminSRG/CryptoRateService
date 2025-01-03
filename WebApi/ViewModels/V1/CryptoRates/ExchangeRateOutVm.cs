namespace CryptoRateService.WebApi.ViewModels.V1.CryptoRates
{
    public class ExchangeRateOutVm
    {
        public string CurrencyCode { get; set; } = null!;
        public double Rate { get; set; }
    }
}
