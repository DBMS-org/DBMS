using Domain.Entities.ProjectManagement;
using Application.Interfaces.ProjectManagement;
using Application.Interfaces.Infrastructure;
using Application.Services.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Application.Services.ProjectManagement
{
    public class RegionApplicationService : IRegionService
    {
        private readonly IRegionRepository _repository;
        private readonly ILogger<RegionApplicationService> _logger;
        private readonly ICacheService _cacheService;

        public RegionApplicationService(
            IRegionRepository repository, 
            ILogger<RegionApplicationService> logger,
            ICacheService cacheService)
        {
            _repository = repository;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<Region>> GetAllRegionsAsync()
        {
            try
            {
                // Regions change infrequently, use longer TTL
                return await _cacheService.GetOrSetAsync(
                    CacheKeyExtensions.AllRegionsKey(),
                    async () =>
                    {
                        var regions = await _repository.GetAllActiveRegionsAsync().ConfigureAwait(false);
                        return regions.ToList();
                    },
                    TimeSpan.FromHours(2) // Longer TTL for relatively static data
                );
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
                return await _cacheService.GetOrSetAsync(
                    CacheKeyExtensions.RegionKey(id),
                    async () => await _repository.GetByIdAsync(id).ConfigureAwait(false),
                    TimeSpan.FromHours(4) // Long TTL for region lookups
                );
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