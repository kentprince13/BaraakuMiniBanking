using FluentValidation;
using MiniBanking.API.Models;

namespace MiniBanking.API.Application.Validations;

public class FundTransferRequestValidator : AbstractValidator<FundTransferRequest>
{
    public FundTransferRequestValidator()
    {
        RuleFor(c => c.PaymentReference).NotEmpty().WithMessage("PaymentReference must not be empty");
        RuleFor(c => c.Amount).GreaterThan(0).WithMessage("Amount can not be less than zero");
        RuleFor(c => c.BankCode).Length(3,6).WithMessage("BankCode can not be less than 3 digit and cannot be more than 6 didgit");
        RuleFor(c => c.DestinationAccount).NotEmpty().WithMessage("DestinationAccount must not be empty");
        //(c => c.SourceAccount).NotEmpty().WithMessage("SourceAccount must not be empty");
        RuleFor(c => c.DestinationAccountName).NotEmpty().WithMessage("DestinationAccountName must not be empty");
    }
}