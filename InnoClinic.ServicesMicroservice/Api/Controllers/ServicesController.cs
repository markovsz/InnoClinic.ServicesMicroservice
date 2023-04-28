using Api.Extensions;
using Application.Abstractions;
using Domain.RequestParameters;
using FluentValidation;
using InnoClinic.SharedModels.DTOs.Services.Incoming;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _servicesService;
        private readonly IValidator<ServiceIncomingDto> _serviceIncomingDtoValidator;

        public ServicesController(IServicesService servicesService, IValidator<ServiceIncomingDto> serviceIncomingDtoValidator)
        {
            _servicesService = servicesService;
            _serviceIncomingDtoValidator = serviceIncomingDtoValidator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateServiceAsync([FromBody] ServiceIncomingDto incomingDto)
        {
            var result = await _serviceIncomingDtoValidator.ValidateAsync(incomingDto);
            result.HandleValidationResult();
            var id = await _servicesService.CreateAsync(incomingDto);
            return CreatedAtRoute("GetServiceById", new { id = id }, id);
        }

        [HttpGet]
        public async Task<IActionResult> GetServicesAsync([FromQuery] ServiceParameters parameters)
        {
            var services = await _servicesService.GetAsync(parameters);
            return Ok(services);
        }

        [HttpGet("service/{id}", Name = "GetServiceById")]
        public async Task<IActionResult> GetServiceByIdAsync(Guid id)
        {
            var service = await _servicesService.GetByIdAsync(id);
            return Ok(service);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateServiceAsync(Guid id, [FromBody] ServiceIncomingDto incomingDto)
        {
            var result = await _serviceIncomingDtoValidator.ValidateAsync(incomingDto);
            result.HandleValidationResult();
            await _servicesService.UpdateAsync(id, incomingDto);
            return NoContent();
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> ChangeServiceStatusAsync(Guid id, [FromBody] string status)
        {
            await _servicesService.ChangeStatusAsync(id, status);
            return NoContent();
        }
    }
}
