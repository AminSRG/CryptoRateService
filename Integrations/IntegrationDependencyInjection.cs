using CryptoRateService.Integrations.ExternalClients;
using CryptoRateService.Integrations.Repositories.Interfaces;
using CryptoRateService.Integrations.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoRateService.Integrations
{
    public static class IntegrationDependencyInjection
    {
        public static IServiceCollection AddIntegrationDependencies(this IServiceCollection services)
        {
            services.AddSingleton<CoinMarketCapClient>();
            services.AddScoped<ICryptoQuoteRepository, CoinMarketCapRepository>();
            return services;
        }
    }
}
