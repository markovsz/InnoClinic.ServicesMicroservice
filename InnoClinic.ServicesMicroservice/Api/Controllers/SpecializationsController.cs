using Api.Extensions;
using Application.Abstractions;
using Application.DTOs.Incoming;
using Application.Validators;
using FluentValidation;
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

        [HttpPost]
        public async Task<IActionResult> CreateSpecializationAsync([FromBody] SpecializationIncomingDto incomingDto)
        {
            var result = await _specializationIncomingDtoValidator.ValidateAsync(incomingDto);
            result.HandleValidationResult();
            var id = await _specializationsService.CreateAsync(incomingDto);
            return CreatedAtRoute("GetSpecialization", new { id = id }, id);
        }

        [HttpGet("specialization/{id}", Name = "GetSpecialization")]
        public async Task<IActionResult> GetSpecializationByIdAsync(Guid id)
        {
            var entity = await _specializationsService.GetByIdAsync(id);
            return Ok(entity);
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecializationsAsync()
        {
            var entities = await _specializationsService.GetAsync();
            return Ok(entities);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSpecializationAsync(Guid id, [FromBody] SpecializationIncomingDto incomingDto)
        {
            var result = await _specializationIncomingDtoValidator.ValidateAsync(incomingDto);
            result.HandleValidationResult();
            await _specializationsService.UpdateAsync(id, incomingDto);
            return NoContent();
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> ChangeSpecializationStatusAsync(Guid id, [FromBody] string status)
        {
            await _specializationsService.ChangeStatusAsync(id, status);
            return NoContent();
        }
    }
}
