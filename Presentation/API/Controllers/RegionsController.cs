using Application.Interfaces.ProjectManagement;
using Domain.Entities.ProjectManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegionsController : BaseApiController
    {
        private readonly IRegionService _regionService;

        public RegionsController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        [HttpGet]
        [Authorize] // Allow all authenticated users to read regions
        public async Task<IActionResult> GetRegions()
        {
            var regions = await _regionService.GetAllRegionsAsync();
            return Ok(regions);
        }

        [HttpGet("{id}")]
        [Authorize] // Allow all authenticated users to read region by ID
        public async Task<IActionResult> GetRegion(int id)
        {
            var region = await _regionService.GetRegionByIdAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            return Ok(region);
        }

        [HttpGet("by-name/{name}")]
        [Authorize] // Allow all authenticated users to read region by name
        public async Task<IActionResult> GetRegionByName(string name)
        {
            var region = await _regionService.GetRegionByNameAsync(name);
            if (region == null)
            {
                return NotFound();
            }
            return Ok(region);
        }
    }
} 