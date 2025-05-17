using Application.Common.Validation.Rules;
using FluentValidation;

namespace Application.Features.Profiles.CreateProfile;

public class CreateUserProfileRequestValidator : AbstractValidator<CreateUserProfileRequest>
{
    public CreateUserProfileRequestValidator()
    {
        RuleFor(r => r.UserId).IsGuid();
        RuleFor(r => r.Nickname).Nickname();
    }
}