using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionService _regionService;

        public RegionsController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegionDto>>> GetRegions()
        {
            var regions = await _regionService.GetAllRegionsAsync();
            return Ok(regions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RegionDto>> GetRegion(int id)
        {
            var region = await _regionService.GetRegionByIdAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            return Ok(region);
        }

        [HttpGet("by-name/{name}")]
        public async Task<ActionResult<RegionDto>> GetRegionByName(string name)
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
