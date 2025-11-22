using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.MachineManagement;
using Application.DTOs.Shared;

namespace Application.Interfaces.MachineManagement
{
    public interface IMachineServiceConfigService
    {
        // CONFIG - Called by: Machine Manager
        Task<Result<bool>> UpdateServiceConfigAsync(int machineId, UpdateMachineServiceConfigRequest request);

        // STATUS - Called by: All roles
        Task<Result<MachineServiceStatusDto>> GetMachineServiceStatusAsync(int machineId);

        // ALERTS - Called by: Machine Manager, Mechanical Engineer
        Task<Result<IEnumerable<ServiceDueAlertDto>>> GetServiceDueAlertsAsync();
        Task<Result<IEnumerable<ServiceDueAlertDto>>> GetServiceDueAlertsByRegionAsync(int regionId);
    }
}
