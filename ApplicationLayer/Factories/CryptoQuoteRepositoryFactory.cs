using CryptoRateService.Application.Factories.Interfaces;
using CryptoRateService.Integrations.ExternalClients;
using CryptoRateService.Integrations.Repositories;
using CryptoRateService.Integrations.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CryptoRateService.Application.Factories
{
    public class CryptoQuoteRepositoryFactory(IConfiguration configuration, HttpClient httpClient) : ICryptoQuoteRepositoryFactory
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly HttpClient _httpClient = httpClient;

        public ICryptoQuoteRepository CreateRepository()
        {
            return new CoinMarketCapRepository(new CoinMarketCapClient(_configuration, _httpClient));
        }
    }
}
