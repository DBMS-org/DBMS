using Application.DTOs.Shared;
using Application.Interfaces.BlastingOperations;
using Domain.Entities.BlastingOperations;
using Microsoft.Extensions.Logging;

namespace Application.Services.BlastingOperations
{
    public class BlastConnectionApplicationService : IBlastConnectionService
    {
        private readonly IBlastConnectionRepository _blastConnectionRepository;
        private readonly ILogger<BlastConnectionApplicationService> _logger;

        public BlastConnectionApplicationService(
            IBlastConnectionRepository blastConnectionRepository,
            ILogger<BlastConnectionApplicationService> logger)
        {
            _blastConnectionRepository = blastConnectionRepository;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<BlastConnection>>> GetConnectionsByProjectAndSiteAsync(int projectId, int siteId)
        {
            try
            {
                _logger.LogInformation("Retrieving blast connections for project {ProjectId} and site {SiteId}", projectId, siteId);
                
                var connections = await _blastConnectionRepository.GetByProjectAndSiteAsync(projectId, siteId);
                
                _logger.LogInformation("Retrieved {Count} blast connections for project {ProjectId} and site {SiteId}", 
                    connections.Count(), projectId, siteId);
                
                return Result.Success(connections);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving blast connections for project {ProjectId} and site {SiteId}", projectId, siteId);
                return Result.Failure<IEnumerable<BlastConnection>>("Failed to retrieve blast connections");
            }
        }

        public async Task<Result<BlastConnection>> GetConnectionByIdAsync(string id, int projectId, int siteId)
        {
            try
            {
                _logger.LogInformation("Retrieving blast connection {Id} for project {ProjectId} and site {SiteId}", id, projectId, siteId);
                
                var connection = await _blastConnectionRepository.GetByIdAsync(id, projectId, siteId);
                
                if (connection == null)
                {
                    _logger.LogWarning("Blast connection {Id} not found for project {ProjectId} and site {SiteId}", id, projectId, siteId);
                    return Result.Failure<BlastConnection>("Blast connection not found");
                }
                
                return Result.Success(connection);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving blast connection {Id} for project {ProjectId} and site {SiteId}", id, projectId, siteId);
                return Result.Failure<BlastConnection>("Failed to retrieve blast connection");
            }
        }

        public async Task<Result<IEnumerable<BlastConnection>>> GetConnectionsBySequenceAsync(int projectId, int siteId, int sequence)
        {
            try
            {
                _logger.LogInformation("Retrieving blast connections for sequence {Sequence} in project {ProjectId} and site {SiteId}", 
                    sequence, projectId, siteId);
                
                var connections = await _blastConnectionRepository.GetBySequenceAsync(projectId, siteId, sequence);
                
                _logger.LogInformation("Retrieved {Count} blast connections for sequence {Sequence}", connections.Count(), sequence);
                
                return Result.Success(connections);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving blast connections for sequence {Sequence} in project {ProjectId} and site {SiteId}", 
                    sequence, projectId, siteId);
                return Result.Failure<IEnumerable<BlastConnection>>("Failed to retrieve blast connections by sequence");
            }
        }

        public async Task<Result<BlastConnection>> CreateConnectionAsync(BlastConnection blastConnection)
        {
            try
            {
                _logger.LogInformation("Creating blast connection {Id} between points {Point1} and {Point2}", 
                    blastConnection.Id, blastConnection.Point1DrillPointId, blastConnection.Point2DrillPointId);
                
                // Validate that the connection doesn't already exist
                var exists = await _blastConnectionRepository.ExistsAsync(blastConnection.Id, blastConnection.ProjectId, blastConnection.SiteId);
                if (exists)
                {
                    _logger.LogWarning("Blast connection {Id} already exists for project {ProjectId} and site {SiteId}", 
                        blastConnection.Id, blastConnection.ProjectId, blastConnection.SiteId);
                    return Result.Failure<BlastConnection>("Blast connection already exists");
                }
                
                // Ensure ID is set
                if (string.IsNullOrEmpty(blastConnection.Id))
                {
                    blastConnection.Id = Guid.NewGuid().ToString();
                }
                
                var createdConnection = await _blastConnectionRepository.CreateAsync(blastConnection);
                
                _logger.LogInformation("Successfully created blast connection {Id}", blastConnection.Id);
                
                return Result.Success(createdConnection);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating blast connection {Id}", blastConnection.Id);
                return Result.Failure<BlastConnection>("Failed to create blast connection");
            }
        }

        public async Task<Result<BlastConnection>> UpdateConnectionAsync(BlastConnection blastConnection)
        {
            try
            {
                _logger.LogInformation("Updating blast connection {Id}", blastConnection.Id);
                
                // Verify connection exists
                var exists = await _blastConnectionRepository.ExistsAsync(blastConnection.Id, blastConnection.ProjectId, blastConnection.SiteId);
                if (!exists)
                {
                    _logger.LogWarning("Blast connection {Id} not found for update", blastConnection.Id);
                    return Result.Failure<BlastConnection>("Blast connection not found");
                }
                
                var updatedConnection = await _blastConnectionRepository.UpdateAsync(blastConnection);
                
                _logger.LogInformation("Successfully updated blast connection {Id}", blastConnection.Id);
                
                return Result.Success(updatedConnection);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating blast connection {Id}", blastConnection.Id);
                return Result.Failure<BlastConnection>("Failed to update blast connection");
            }
        }

        public async Task<Result> DeleteConnectionAsync(string id, int projectId, int siteId)
        {
            try
            {
                _logger.LogInformation("Deleting blast connection {Id} for project {ProjectId} and site {SiteId}", id, projectId, siteId);
                
                var exists = await _blastConnectionRepository.ExistsAsync(id, projectId, siteId);
                if (!exists)
                {
                    _logger.LogWarning("Blast connection {Id} not found for deletion", id);
                    return Result.Failure("Blast connection not found");
                }
                
                await _blastConnectionRepository.DeleteAsync(id, projectId, siteId);
                
                _logger.LogInformation("Successfully deleted blast connection {Id}", id);
                
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting blast connection {Id}", id);
                return Result.Failure("Failed to delete blast connection");
            }
        }

        public async Task<Result<bool>> ConnectionExistsAsync(string id, int projectId, int siteId)
        {
            try
            {
                var exists = await _blastConnectionRepository.ExistsAsync(id, projectId, siteId);
                return Result.Success(exists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if blast connection {Id} exists", id);
                return Result.Failure<bool>("Failed to check blast connection existence");
            }
        }
    }
} 