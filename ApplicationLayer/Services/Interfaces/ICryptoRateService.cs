using CryptoRateService.Domain.Entities;
using FluentResults;

namespace CryptoRateService.Application.Services.Interfaces
{
    public interface ICryptoRateService
    {
        Task<Result<List<ExchangeRate>>> GetExchangeRatesAsync(string symbol);
    }
}
