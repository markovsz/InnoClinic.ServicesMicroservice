using Application.DTOs.Incoming;
using Domain.Enums;
using FluentValidation;

namespace Application.Validators;

public class ServiceIncomingDtoValidator : AbstractValidator<ServiceIncomingDto>
{
	public ServiceIncomingDtoValidator()
	{
		RuleFor(e => e.Name).NotEmpty();
		RuleFor(e => e.Price).GreaterThan(0.0m);
		RuleFor(e => e.Status).IsEnumName(typeof(Status));
	}
}
