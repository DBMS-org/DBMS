using Application.DTOs;

namespace Application.Interfaces
{
    public interface IMachineService
    {
        // Machine CRUD operations
        Task<List<MachineDto>> GetAllMachinesAsync();
        Task<MachineDto?> GetMachineByIdAsync(int id);
        Task<MachineDto> CreateMachineAsync(CreateMachineRequest request);
        Task<MachineDto> UpdateMachineAsync(UpdateMachineRequest request);
        Task<bool> DeleteMachineAsync(int id);
        
        // Machine queries
        Task<List<MachineDto>> GetMachinesByProjectAsync(int projectId);
        Task<List<MachineDto>> GetMachinesByOperatorAsync(int operatorId);
        Task<List<MachineDto>> GetMachinesByRegionAsync(int regionId);
        Task<List<MachineDto>> GetAvailableMachinesAsync();
        Task<List<MachineDto>> GetMachinesInUseAsync();
        Task<List<MachineDto>> GetMachinesRequiringMaintenanceAsync();
        
        // Machine assignment
        Task<bool> AssignMachineToProjectAsync(int machineId, int projectId);
        Task<bool> AssignMachineToOperatorAsync(int machineId, int operatorId);
        Task<bool> UnassignMachineFromProjectAsync(int machineId);
        Task<bool> UnassignMachineFromOperatorAsync(int machineId);
        
        // Machine status management
        Task<bool> SetMachineStatusAsync(int machineId, string status);
        Task<bool> UpdateMachineLocationAsync(int machineId, string location);
        Task<bool> ScheduleMaintenanceAsync(int machineId, DateTime maintenanceDate);
        Task<bool> CompleteMaintenance(int machineId);
        
        // Machine validation
        Task<bool> ValidateMachineExistsAsync(int machineId);
        Task<bool> IsMachineAvailableAsync(int machineId);
        Task<bool> IsMachineAssignedToProjectAsync(int machineId, int projectId);
    }
} 