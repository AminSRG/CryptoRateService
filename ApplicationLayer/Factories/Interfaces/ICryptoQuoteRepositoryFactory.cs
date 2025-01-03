using CryptoRateService.Integrations.Repositories.Interfaces;

namespace CryptoRateService.Application.Factories.Interfaces
{
    public interface ICryptoQuoteRepositoryFactory
    {
        ICryptoQuoteRepository CreateRepository();
    }
}
