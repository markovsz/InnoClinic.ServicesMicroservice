using Api.Enums;
using Api.Extensions;
using Application.Abstractions;
using Domain.RequestParameters;
using FluentValidation;
using InnoClinic.SharedModels.DTOs.Services.Incoming;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _servicesService;
        private readonly IValidator<ServiceIncomingDto> _serviceIncomingDtoValidator;
        private readonly IValidator<AddServiceIncomingDto> _addServiceIncomingDtoValidator;

        public ServicesController(IServicesService servicesService, 
            IValidator<ServiceIncomingDto> serviceIncomingDtoValidator,
            IValidator<AddServiceIncomingDto> addServiceIncomingDtoValidator)
        {
            _servicesService = servicesService;
            _serviceIncomingDtoValidator = serviceIncomingDtoValidator;
            _addServiceIncomingDtoValidator = addServiceIncomingDtoValidator;
        }

        [Authorize(Roles = nameof(UserRole.Receptionist))]
        [HttpPost]
        public async Task<IActionResult> CreateServiceAsync([FromBody] AddServiceIncomingDto incomingDto)
        {
            var result = await _addServiceIncomingDtoValidator.ValidateAsync(incomingDto);
            result.HandleValidationResult();
            var id = await _servicesService.CreateAsync(incomingDto);
            return CreatedAtRoute("GetServiceById", new { id = id }, id);
        }

        [Authorize(Roles = $"{nameof(UserRole.Patient)},{nameof(UserRole.Receptionist)}")]
        [HttpGet]
        public async Task<IActionResult> GetServicesAsync([FromQuery] ServiceParameters parameters)
        {
            var services = await _servicesService.GetAsync(parameters);
            return Ok(services);
        }

        [Authorize(Roles = $"{nameof(UserRole.Patient)},{nameof(UserRole.Doctor)},{nameof(UserRole.Receptionist)}")]
        [HttpPost("ids")]
        public async Task<IActionResult> GetServicesByIdsAsync([FromBody] IEnumerable<Guid> ids)
        {
            var entities = await _servicesService.GetByIdsAsync(ids);
            return Ok(entities);
        }

        [Authorize(Roles = $"{nameof(UserRole.Patient)},{nameof(UserRole.Receptionist)}")]
        [HttpGet("service/{id}", Name = "GetServiceById")]
        public async Task<IActionResult> GetServiceByIdAsync(Guid id)
        {
            var service = await _servicesService.GetByIdAsync(id);
            return Ok(service);
        }

        [Authorize(Roles = $"{nameof(UserRole.Patient)},{nameof(UserRole.Receptionist)}")]
        [HttpGet("service/{id}/min")]
        public async Task<IActionResult> GetMinServiceByIdAsync(Guid id)
        {
            var service = await _servicesService.GetMinByIdAsync(id);
            return Ok(service);
        }

        [Authorize(Roles = nameof(UserRole.Receptionist))]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServiceAsync(Guid id, [FromBody] ServiceIncomingDto incomingDto)
        {
            var result = await _serviceIncomingDtoValidator.ValidateAsync(incomingDto);
            result.HandleValidationResult();
            await _servicesService.UpdateAsync(id, incomingDto);
            return NoContent();
        }

        [Authorize(Roles = nameof(UserRole.Receptionist))]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> ChangeServiceStatusAsync(Guid id, [FromBody] string status)
        {
            await _servicesService.ChangeStatusAsync(id, status);
            return NoContent();
        }
    }
}
