using Application.Abstractions;
using Application.DTOs.Incoming;
using Domain.RequestParameters;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServicesService _servicesService;

        public ServicesController(IServicesService servicesService)
        {
            _servicesService = servicesService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateServiceAsync(ServiceIncomingDto service)
        {
            var id = await _servicesService.CreateAsync(service);
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
        public async Task<IActionResult> UpdateServiceAsync(Guid id, ServiceIncomingDto incomingDto)
        {
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
