using Application.Common.Validation.Rules;
using FluentValidation;

namespace Application.Features.Authentication.Register;

internal class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(r => r.Username).Username();
        RuleFor(r => r.Login).Login();
        RuleFor(r => r.Password).Password();
    }
}