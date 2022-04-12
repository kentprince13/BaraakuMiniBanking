using FluentValidation;
using MiniBanking.API.Models;
using MiniBanking.Domain.Enums;

namespace MiniBanking.API.Application.Validations;

public class WalletTopUpRequestValidator : AbstractValidator<WalletTopUpRequest>
{
    public WalletTopUpRequestValidator()
    {
        RuleFor(c => c.PaymentReference).NotEmpty()
            .WithMessage("PaymentReference must not be empty");
        RuleFor(c => c.Amount).GreaterThan(0)
            .WithMessage("Amount can not be less than zero");
        RuleFor(c => c.Status).IsEnumName(typeof(TransactionStatus))
            .WithMessage("Accepted Status Include Failed, Success, Pending, Created ");
    }
}