using Cqrs.Domain.Helpers;
using FluentValidation;

namespace Cqrs.Domain.Commands.UpdatePerson;

public class UpdatePersonCommandValidator : AbstractValidator<UpdatePersonCommand>
{
    public UpdatePersonCommandValidator()
    {
        RuleFor(input => input.Name)
            .NotEmpty().WithMessage("The field {PropertyName} is not valid");

        RuleFor(input => input.Cpf)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("The field {PropertyName} is not valid")
            .Must(StringHelper.IsCpf).WithMessage("The value {PropertyValue} is not valid for {PropertyName}");

        RuleFor(input => input.DateBirth)
            .NotEmpty().WithMessage("The field {PropertyName} is mandatory");

        RuleFor(input => input.Email)
            .EmailAddress().WithMessage("The field {PropertyName} is not valid")
            .When(x => !string.IsNullOrWhiteSpace(x.Email));
    }
}
