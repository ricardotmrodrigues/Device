using FluentValidation;

namespace Application.Devices.Commands.PartialUpdateDevice;

public class PartialUpdateDeviceCommandValidator : AbstractValidator<PartialUpdateDeviceCommand>
{
    public PartialUpdateDeviceCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");

        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.")
            .When(x => x.Name is not null);

        RuleFor(x => x.Brand)
            .MaximumLength(100).WithMessage("Brand must not exceed 100 characters.")
            .When(x => x.Brand is not null);

        RuleFor(x => x.State)
            .IsInEnum().WithMessage("Invalid device state.")
            .When(x => x.State is not null);
    }
}
