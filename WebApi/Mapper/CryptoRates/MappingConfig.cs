using CryptoRateService.Domain.Entities;
using CryptoRateService.WebApi.ViewModels.V1.CryptoRates;
using Mapster;

namespace CryptoRateService.WebApi.Mapper.CryptoRates
{
    public class MappingConfig
    {
        public static void RegisterMappings()
        {
            TypeAdapterConfig<ExchangeRate, ExchangeRateOutVm>
                .NewConfig()
                .Map(dest => dest.CurrencyCode, src => src.CurrencyCode)
                .Map(dest => dest.Rate, src => src.Rate);
        }
    }
}
