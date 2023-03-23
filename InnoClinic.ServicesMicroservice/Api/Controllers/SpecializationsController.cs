using Application.Abstractions;
using Application.DTOs.Incoming;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationsController : ControllerBase
    {
        private ISpecializationsService _specializationsService;

        public SpecializationsController(ISpecializationsService specializationsService) 
        {
            _specializationsService = specializationsService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSpecializationAsync([FromBody] SpecializationIncomingDto incomingDto)
        {
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
