using CryptoRateService.Application.Services.Interfaces;
using CryptoRateService.Domain.Entities;
using CryptoRateService.WebApi.ViewModels.V1.CryptoRates;
using FluentResults;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CryptoRateService.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CryptoRatesController(ICryptoRateService cryptoRateService) : ControllerBase
    {
        private readonly ICryptoRateService _cryptoRateService = cryptoRateService;


        /// <summary>
        /// Retrieves exchange rates for the specified cryptocurrency symbol.
        /// </summary>
        /// <param name="request">The request object containing the cryptocurrency symbol.</param>
        /// <returns>A list of exchange rates for the given symbol.</returns>
        /// <response code="200">Returns the exchange rates</response>
        /// <response code="400">If the symbol is null or empty</response>
        /// <response code="401"></response>
        /// <response code="500">If an unexpected error occurs</response>
        [HttpPost]
        [SwaggerOperation(Summary = "Get exchange rates for a cryptocurrency symbol")]
        [SwaggerResponse(200, "Successfully retrieved exchange rates", typeof(List<ExchangeRateOutVm>))]
        [SwaggerResponse(400, "Invalid symbol provided")]
        [SwaggerResponse(401)]
        [SwaggerResponse(500, "Internal server error")]
        public async Task<IActionResult> GetCryptoRates([FromBody] CryptoRatesInBodyVm request)
        {
            Result<List<ExchangeRate>> result = await _cryptoRateService.GetExchangeRatesAsync(request.Symbol);

            if (result.IsSuccess)
                return Ok(result.Value);

            return result.Errors.Any(e => e.Message.Contains("An error occurred"))
                ? StatusCode(500, result.Errors.First().Message)
                : BadRequest(result.Errors.First().Message);
        }
    }
}
