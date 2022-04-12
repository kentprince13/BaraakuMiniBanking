using FluentValidation;
using MiniBanking.API.Models;

namespace MiniBanking.API.Application.Validations;

public class UserRequestValidator : AbstractValidator<UserRequestModel>
{
    public UserRequestValidator()
    {
        RuleFor(c => c.Email).NotEmpty().WithMessage("Email must not be empty");
        RuleFor(c => c.FirstName).NotEmpty().WithMessage("FirstName must not be empty");
        RuleFor(c => c.PhoneNumber).NotEmpty().WithMessage("PhoneNumber must not be empty");
    }
}