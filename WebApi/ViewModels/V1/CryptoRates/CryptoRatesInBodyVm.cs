using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace CryptoRateService.WebApi.ViewModels.V1.CryptoRates
{
    public class CryptoRatesInBodyVm
    {
        [Required]
        public string Symbol { get; set; }
    }

    public class CryptoRatesRequestValidator : AbstractValidator<CryptoRatesInBodyVm>
    {
        public CryptoRatesRequestValidator()
        {
            RuleFor(x => x.Symbol)
                .NotNull()
                .WithMessage("Symbol is required.")
                .NotEmpty()
                .WithMessage("Symbol cannot be empty.")
                .Matches(@"^[A-Za-z0-9]+$")
                .WithMessage("Symbol can only contain alphanumeric characters.");
        }
    }
}
