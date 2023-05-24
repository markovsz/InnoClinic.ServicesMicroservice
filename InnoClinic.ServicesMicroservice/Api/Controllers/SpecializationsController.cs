using Api.Enums;
using Api.Extensions;
using Application.Abstractions;
using FluentValidation;
using InnoClinic.SharedModels.DTOs.Services.Incoming;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationsController : ControllerBase
    {
        private readonly ISpecializationsService _specializationsService;
        private readonly IValidator<SpecializationIncomingDto> _specializationIncomingDtoValidator;
         
        public SpecializationsController(ISpecializationsService specializationsService, IValidator<SpecializationIncomingDto> specializationIncomingDtoValidator) 
        {
            _specializationsService = specializationsService;
            _specializationIncomingDtoValidator = specializationIncomingDtoValidator;
        }

        [Authorize(Roles = nameof(UserRole.Receptionist))]
        [HttpPost]
        public async Task<IActionResult> CreateSpecializationAsync([FromBody] SpecializationIncomingDto incomingDto)
        {
            var result = await _specializationIncomingDtoValidator.ValidateAsync(incomingDto);
            result.HandleValidationResult();
            var id = await _specializationsService.CreateAsync(incomingDto);
            return CreatedAtRoute("GetSpecialization", new { id = id }, id);
        }

        [Authorize(Roles = $"{nameof(UserRole.Patient)},{nameof(UserRole.Doctor)},{nameof(UserRole.Receptionist)}")]
        [HttpPost("ids")]
        public async Task<IActionResult> GetSpecializationsByIdsAsync([FromBody] IEnumerable<Guid> ids)
        {
            var entities = await _specializationsService.GetByIdsAsync(ids);
            return Ok(entities);
        }

        [Authorize(Roles = nameof(UserRole.Receptionist))]
        [HttpGet("specialization/{id}", Name = "GetSpecialization")]
        public async Task<IActionResult> GetSpecializationByIdAsync(Guid id)
        {
            var entity = await _specializationsService.GetByIdAsync(id);
            return Ok(entity);
        }

        [Authorize(Roles = $"{nameof(UserRole.Patient)},{nameof(UserRole.Doctor)},{nameof(UserRole.Receptionist)}")]
        [HttpGet("specialization/{id}/min")]
        public async Task<IActionResult> GetMinSpecializationByIdAsync(Guid id)
        {
            var entity = await _specializationsService.GetMinByIdAsync(id);
            return Ok(entity);
        }

        [Authorize(Roles = $"{nameof(UserRole.Receptionist)}")]
        [HttpGet]
        public async Task<IActionResult> GetSpecializationsAsync()
        {
            var entities = await _specializationsService.GetAsync();
            return Ok(entities);
        }

        [Authorize(Roles = $"{nameof(UserRole.Receptionist)}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSpecializationAsync(Guid id, [FromBody] SpecializationIncomingDto incomingDto)
        {
            var result = await _specializationIncomingDtoValidator.ValidateAsync(incomingDto);
            result.HandleValidationResult();
            await _specializationsService.UpdateAsync(id, incomingDto);
            return NoContent();
        }

        [Authorize(Roles = $"{nameof(UserRole.Receptionist)}")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> ChangeSpecializationStatusAsync(Guid id, [FromBody] string status)
        {
            await _specializationsService.ChangeStatusAsync(id, status);
            return NoContent();
        }
    }
}
