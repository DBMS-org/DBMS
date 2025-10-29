using Application.DTOs.MaintenanceOperations;
using Application.DTOs.Shared;

namespace Application.Interfaces.MaintenanceOperations
{
    public interface IMaintenanceReportService
    {
        Task<Result<MaintenanceReportDto>> SubmitReportAsync(SubmitMaintenanceReportRequest request);
        Task<Result<MaintenanceReportDto>> GetReportByIdAsync(int id);
        Task<Result<MaintenanceReportDto>> GetReportByTicketIdAsync(string ticketId);
        Task<Result<IEnumerable<MaintenanceReportDto>>> GetOperatorReportsAsync(int operatorId);
        Task<Result<OperatorMachineDto>> GetOperatorMachineAsync(int operatorId);
        Task<Result<MaintenanceReportSummaryDto>> GetOperatorSummaryAsync(int operatorId);
        Task<Result<MaintenanceReportDto>> UpdateReportStatusAsync(int id, UpdateReportStatusRequest request);
        Task<Result<MaintenanceReportDto>> CloseReportAsync(int id, int operatorId);
        Task<Result<MaintenanceReportDto>> ReopenReportAsync(int id, string reason);
    }
}
