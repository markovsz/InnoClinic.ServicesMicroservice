using Domain.Enums;
using FluentValidation;
using InnoClinic.SharedModels.DTOs.Services.Incoming;

namespace Application.Validators;

public class UpdateSpecializationIncomingDtoValidator : AbstractValidator<UpdateSpecializationIncomingDto>
{
	public UpdateSpecializationIncomingDtoValidator()
	{
		RuleFor(e => e.Name).NotEmpty();
		RuleFor(e => e.Status).IsEnumName(typeof(Status));
	}
}
