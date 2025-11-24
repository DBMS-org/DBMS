using Application.DTOs.Reports;

namespace Application.Interfaces.Reports
{
    public interface IReportService
    {
        // Get available reports metadata
        Task<List<ReportMetadataDto>> GetAvailableReportsAsync();

        // REMOVED IN PHASE 1: Fleet Management Report - Insufficient data (only 2 machines)
        // Task<FleetManagementReportDto> GetFleetManagementReportAsync(ReportFilterDto? filter = null);

        // Maintenance Performance Report
        Task<MaintenancePerformanceReportDto> GetMaintenancePerformanceReportAsync(ReportFilterDto? filter = null);

        // REMOVED IN PHASE 1: Inventory Status Report - StoreInventories table empty
        // Task<InventoryStatusReportDto> GetInventoryStatusReportAsync(ReportFilterDto? filter = null);

        // REMOVED IN PHASE 1: Operational Efficiency Report - Insufficient data
        // Task<OperationalEfficiencyReportDto> GetOperationalEfficiencyReportAsync(ReportFilterDto? filter = null);

        // Regional Performance Report
        Task<RegionalPerformanceReportDto> GetRegionalPerformanceReportAsync(ReportFilterDto? filter = null);

        // PHASE 3: Drilling Operations Report - Core business data (164 drill holes)
        Task<DrillingOperationsReportDto> GetDrillingOperationsReportAsync(ReportFilterDto? filter = null);

        // PHASE 4: Explosive Workflow Report - Approval & Transfer tracking (12 requests)
        Task<ExplosiveWorkflowReportDto> GetExplosiveWorkflowReportAsync(ReportFilterDto? filter = null);

        // PHASE 5: User & Access Management Report - Admin oversight (8 users, 7 roles)
        Task<UserAccessReportDto> GetUserAccessReportAsync(ReportFilterDto? filter = null);

        // Financial Overview Report
        Task<FinancialOverviewReportDto> GetFinancialOverviewReportAsync(ReportFilterDto? filter = null);

        // Custom Report
        Task<CustomReportResponseDto> GenerateCustomReportAsync(CustomReportRequestDto request);

        // Export functionality
        Task<byte[]> ExportReportToPdfAsync(string reportType, ReportFilterDto? filter = null);
        Task<byte[]> ExportReportToExcelAsync(string reportType, ReportFilterDto? filter = null);
    }
}
