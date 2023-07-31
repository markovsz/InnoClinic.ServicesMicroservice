using Api.Enums;
using Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [Authorize(Roles = $"{nameof(UserRole.Patient)},{nameof(UserRole.Receptionist)}")]
        [HttpGet("category/{name}")]
        public async Task<IActionResult> GetServiceByIdAsync(string name)
        {
            var category = await _categoriesService.GetByNameAsync(name);
            return Ok(category);
        }
    }
}
