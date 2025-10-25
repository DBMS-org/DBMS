using Domain.Entities.MachineManagement;

namespace Application.Interfaces.MachineManagement
{
    public interface IAccessoryRepository
    {
        Task<IEnumerable<Accessory>> GetAllAsync();
        Task<Accessory?> GetByIdAsync(int id);
        Task<Accessory?> GetByIdWithHistoryAsync(int id);
        Task<bool> ExistsByPartNumberAsync(string partNumber, int? excludeId = null);
        Task<IEnumerable<Accessory>> SearchAsync(
            string? searchTerm = null,
            AccessoryCategory? category = null,
            string? supplier = null,
            string? status = null);
        Task<Accessory> AddAsync(Accessory accessory);
        Task UpdateAsync(Accessory accessory);
        Task DeleteAsync(Accessory accessory);
        Task<int> CountAsync();
        Task<int> CountLowStockAsync();
        Task<int> CountOutOfStockAsync();
        Task<int> CountAvailableAsync();
    }
}
