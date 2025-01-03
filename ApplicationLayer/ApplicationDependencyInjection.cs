using CryptoRateService.Application.Factories.Interfaces;
using CryptoRateService.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoRateService.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICryptoQuoteRepositoryFactory, Factories.CryptoQuoteRepositoryFactory>();
            services.AddScoped<ICryptoRateService, Services.CryptoRateService>();
            return services;
        }
    }
}
