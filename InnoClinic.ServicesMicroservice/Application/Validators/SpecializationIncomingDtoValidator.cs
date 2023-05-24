using Domain.Enums;
using FluentValidation;
using InnoClinic.SharedModels.DTOs.Services.Incoming;

namespace Application.Validators;

public class SpecializationIncomingDtoValidator : AbstractValidator<SpecializationIncomingDto>
{
	public SpecializationIncomingDtoValidator()
	{
		RuleFor(e => e.Name).NotEmpty();
		RuleFor(e => e.Status).IsEnumName(typeof(Status));
	}
}
