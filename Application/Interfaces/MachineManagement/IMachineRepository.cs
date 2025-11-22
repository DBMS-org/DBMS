using Domain.Entities.MachineManagement;

namespace Application.Interfaces.MachineManagement
{
    public interface IMachineRepository
    {
        Task<Machine?> GetByIdAsync(int id);
        Task<Machine?> GetByOperatorIdAsync(int operatorId);
        Task<bool> UpdateAsync(Machine machine);
        Task<IEnumerable<Machine>> GetByOperatorIdsAsync(IEnumerable<int> operatorIds);
        Task<IEnumerable<Machine>> GetAllAsync();
        Task<IEnumerable<Machine>> GetByRegionAsync(int regionId);
    }
}
