using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Repositories
{
    public class DrillHoleRepository : IDrillHoleRepository
    {
        private readonly List<DrillHole> _drillHoles = new();
        private readonly object _lock = new();

        public Task<IEnumerable<DrillHole>> GetAllAsync()
        {
            lock (_lock)
            {
                return Task.FromResult(_drillHoles.AsEnumerable());
            }
        }

        public Task<DrillHole?> GetByIdAsync(string id)
        {
            lock (_lock)
            {
                var drillHole = _drillHoles.FirstOrDefault(d => d.Id == id);
                return Task.FromResult(drillHole);
            }
        }

        public Task<DrillHole> AddAsync(DrillHole drillHole)
        {
            lock (_lock)
            {
                _drillHoles.Add(drillHole);
                return Task.FromResult(drillHole);
            }
        }

        public Task UpdateAsync(DrillHole drillHole)
        {
            lock (_lock)
            {
                var existingIndex = _drillHoles.FindIndex(d => d.Id == drillHole.Id);
                if (existingIndex >= 0)
                {
                    _drillHoles[existingIndex] = drillHole;
                }
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(string id)
        {
            lock (_lock)
            {
                var drillHole = _drillHoles.FirstOrDefault(d => d.Id == id);
                if (drillHole != null)
                {
                    _drillHoles.Remove(drillHole);
                }
            }
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(string id)
        {
            lock (_lock)
            {
                var exists = _drillHoles.Any(d => d.Id == id);
                return Task.FromResult(exists);
            }
        }

        public Task AddRangeAsync(IEnumerable<DrillHole> drillHoles)
        {
            lock (_lock)
            {
                _drillHoles.AddRange(drillHoles);
            }
            return Task.CompletedTask;
        }

        public Task ClearAllAsync()
        {
            lock (_lock)
            {
                _drillHoles.Clear();
            }
            return Task.CompletedTask;
        }
    }
} 