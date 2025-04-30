using Application.Common.Validation.Rules;
using FluentValidation;

namespace Application.Features.Authentication.Login;

internal class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(r => r.Login).NotNull().Login();
        RuleFor(r => r.Password).Password();
    }
}