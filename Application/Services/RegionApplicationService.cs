using Domain.Entities;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class RegionApplicationService : IRegionService
    {
        private readonly IRegionRepository _repository;
        private readonly ILogger<RegionApplicationService> _logger;

        public RegionApplicationService(IRegionRepository repository, ILogger<RegionApplicationService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<Region>> GetAllRegionsAsync()
        {
            try
            {
                return await _repository.GetAllActiveRegionsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all regions");
                throw;
            }
        }

        public async Task<Region?> GetRegionByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting region {RegionId}", id);
                throw;
            }
        }

        public async Task<Region?> GetRegionByNameAsync(string name)
        {
            try
            {
                return await _repository.GetByNameAsync(name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting region by name {RegionName}", name);
                throw;
            }
        }
    }
} 