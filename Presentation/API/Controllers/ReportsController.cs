using Application.DTOs.Reports;
using Application.DTOs.Shared;
using Application.Interfaces.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(
            IReportService reportService,
            ILogger<ReportsController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        /// <summary>
        /// Get list of all available reports
        /// </summary>
        [HttpGet("available")]
        [Authorize(Roles = "Admin,GeneralManager")]
        public async Task<ActionResult<ApiResponse<List<ReportMetadataDto>>>> GetAvailableReports()
        {
            try
            {
                var reports = await _reportService.GetAvailableReportsAsync();
                return Ok(new ApiResponse<List<ReportMetadataDto>>(reports, true, "Available reports retrieved successfully", 200));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available reports");
                return StatusCode(500, new ApiResponse<List<ReportMetadataDto>>(null!, false, "Failed to retrieve available reports", 500));
            }
        }

        /* REMOVED IN PHASE 1: Fleet Management Report endpoint
        /// <summary>
        /// Generate Fleet Management Report
        /// </summary>
        [HttpPost("fleet-management")]
        [Authorize(Roles = "Admin,GeneralManager,MachineManager")]
        public async Task<ActionResult<ApiResponse<FleetManagementReportDto>>> GetFleetManagementReport([FromBody] ReportFilterDto? filter = null)
        {
            try
            {
                _logger.LogInformation("Generating Fleet Management Report");
                var report = await _reportService.GetFleetManagementReportAsync(filter);
                return Ok(new ApiResponse<FleetManagementReportDto>(report, true, "Fleet Management Report generated successfully", 200));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating Fleet Management Report");
                return StatusCode(500, new ApiResponse<FleetManagementReportDto>(null!, false, "Failed to generate Fleet Management Report", 500));
            }
        }
        */

        /// <summary>
        /// Generate Maintenance Performance Report
        /// </summary>
        [HttpPost("maintenance-performance")]
        [Authorize(Roles = "Admin,GeneralManager,MachineManager")]
        public async Task<ActionResult<ApiResponse<MaintenancePerformanceReportDto>>> GetMaintenancePerformanceReport([FromBody] ReportFilterDto? filter = null)
        {
            try
            {
                _logger.LogInformation("Generating Maintenance Performance Report");
                var report = await _reportService.GetMaintenancePerformanceReportAsync(filter);
                return Ok(new ApiResponse<MaintenancePerformanceReportDto>(report, true, "Maintenance Performance Report generated successfully", 200));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating Maintenance Performance Report");
                return StatusCode(500, new ApiResponse<MaintenancePerformanceReportDto>(null!, false, "Failed to generate Maintenance Performance Report", 500));
            }
        }

        /* REMOVED IN PHASE 1: Inventory Status Report endpoint
        /// <summary>
        /// Generate Inventory Status Report
        /// </summary>
        [HttpPost("inventory-status")]
        [Authorize(Roles = "Admin,GeneralManager,StoreManager")]
        public async Task<ActionResult<ApiResponse<InventoryStatusReportDto>>> GetInventoryStatusReport([FromBody] ReportFilterDto? filter = null)
        {
            try
            {
                _logger.LogInformation("Generating Inventory Status Report");
                var report = await _reportService.GetInventoryStatusReportAsync(filter);
                return Ok(new ApiResponse<InventoryStatusReportDto>(report, true, "Inventory Status Report generated successfully", 200));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating Inventory Status Report");
                return StatusCode(500, new ApiResponse<InventoryStatusReportDto>(null!, false, "Failed to generate Inventory Status Report", 500));
            }
        }
        */

        /* REMOVED IN PHASE 1: Operational Efficiency Report endpoint
        /// <summary>
        /// Generate Operational Efficiency Report
        /// </summary>
        [HttpPost("operational-efficiency")]
        [Authorize(Roles = "Admin,GeneralManager")]
        public async Task<ActionResult<ApiResponse<OperationalEfficiencyReportDto>>> GetOperationalEfficiencyReport([FromBody] ReportFilterDto? filter = null)
        {
            try
            {
                _logger.LogInformation("Generating Operational Efficiency Report");
                var report = await _reportService.GetOperationalEfficiencyReportAsync(filter);
                return Ok(new ApiResponse<OperationalEfficiencyReportDto>(report, true, "Operational Efficiency Report generated successfully", 200));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating Operational Efficiency Report");
                return StatusCode(500, new ApiResponse<OperationalEfficiencyReportDto>(null!, false, "Failed to generate Operational Efficiency Report", 500));
            }
        }
        */

        /* REMOVED: Regional Performance Report - Not needed for current operations
        /// <summary>
        /// Generate Regional Performance Report
        /// </summary>
        [HttpPost("regional-performance")]
        [Authorize(Roles = "Admin,GeneralManager")]
        public async Task<ActionResult<ApiResponse<RegionalPerformanceReportDto>>> GetRegionalPerformanceReport([FromBody] ReportFilterDto? filter = null)
        {
            try
            {
                _logger.LogInformation("Generating Regional Performance Report");
                var report = await _reportService.GetRegionalPerformanceReportAsync(filter);
                return Ok(new ApiResponse<RegionalPerformanceReportDto>(report, true, "Regional Performance Report generated successfully", 200));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating Regional Performance Report");
                return StatusCode(500, new ApiResponse<RegionalPerformanceReportDto>(null!, false, "Failed to generate Regional Performance Report", 500));
            }
        }
        */

        /// <summary>
        /// Generate Drilling Operations Report - PHASE 3
        /// </summary>
        [HttpPost("drilling-operations")]
        [Authorize(Roles = "Admin,GeneralManager,BlastingEngineer")]
        public async Task<ActionResult<ApiResponse<DrillingOperationsReportDto>>> GetDrillingOperationsReport([FromBody] ReportFilterDto? filter = null)
        {
            try
            {
                _logger.LogInformation("Generating Drilling Operations Report");
                var report = await _reportService.GetDrillingOperationsReportAsync(filter);
                return Ok(new ApiResponse<DrillingOperationsReportDto>(report, true, "Drilling Operations Report generated successfully", 200));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating Drilling Operations Report");
                return StatusCode(500, new ApiResponse<DrillingOperationsReportDto>(null!, false, "Failed to generate Drilling Operations Report", 500));
            }
        }

        /// <summary>
        /// Generate Explosive Workflow Report - PHASE 4
        /// </summary>
        [HttpPost("explosive-workflow")]
        [Authorize(Roles = "Admin,GeneralManager,StoreManager")]
        public async Task<ActionResult<ApiResponse<ExplosiveWorkflowReportDto>>> GetExplosiveWorkflowReport([FromBody] ReportFilterDto? filter = null)
        {
            try
            {
                _logger.LogInformation("Generating Explosive Workflow Report");
                var report = await _reportService.GetExplosiveWorkflowReportAsync(filter);
                return Ok(new ApiResponse<ExplosiveWorkflowReportDto>(report, true, "Explosive Workflow Report generated successfully", 200));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating Explosive Workflow Report");
                return StatusCode(500, new ApiResponse<ExplosiveWorkflowReportDto>(null!, false, "Failed to generate Explosive Workflow Report", 500));
            }
        }

        /// <summary>
        /// Generate User & Access Management Report - PHASE 5
        /// </summary>
        [HttpPost("user-access")]
        [Authorize(Roles = "Admin,GeneralManager")]
        public async Task<ActionResult<ApiResponse<UserAccessReportDto>>> GetUserAccessReport([FromBody] ReportFilterDto? filter = null)
        {
            try
            {
                _logger.LogInformation("Generating User & Access Management Report");
                var report = await _reportService.GetUserAccessReportAsync(filter);
                return Ok(new ApiResponse<UserAccessReportDto>(report, true, "User & Access Management Report generated successfully", 200));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating User & Access Management Report");
                return StatusCode(500, new ApiResponse<UserAccessReportDto>(null!, false, "Failed to generate User & Access Management Report", 500));
            }
        }

        /// <summary>
        /// Generate Financial Overview Report
        /// </summary>
        [HttpPost("financial-overview")]
        [Authorize(Roles = "Admin,GeneralManager")]
        public async Task<ActionResult<ApiResponse<FinancialOverviewReportDto>>> GetFinancialOverviewReport([FromBody] ReportFilterDto? filter = null)
        {
            try
            {
                _logger.LogInformation("Generating Financial Overview Report");
                var report = await _reportService.GetFinancialOverviewReportAsync(filter);
                return Ok(new ApiResponse<FinancialOverviewReportDto>(report, true, "Financial Overview Report generated successfully", 200));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating Financial Overview Report");
                return StatusCode(500, new ApiResponse<FinancialOverviewReportDto>(null!, false, "Failed to generate Financial Overview Report", 500));
            }
        }

        /// <summary>
        /// Generate Custom Report
        /// </summary>
        [HttpPost("custom")]
        [Authorize(Roles = "Admin,GeneralManager")]
        public async Task<ActionResult<ApiResponse<CustomReportResponseDto>>> GenerateCustomReport([FromBody] CustomReportRequestDto request)
        {
            try
            {
                _logger.LogInformation("Generating Custom Report: {ReportName}", request.ReportName);
                var report = await _reportService.GenerateCustomReportAsync(request);
                return Ok(new ApiResponse<CustomReportResponseDto>(report, true, "Custom Report generated successfully", 200));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating Custom Report");
                return StatusCode(500, new ApiResponse<CustomReportResponseDto>(null!, false, "Failed to generate Custom Report", 500));
            }
        }

        /// <summary>
        /// Export report to PDF
        /// </summary>
        [HttpPost("export/pdf")]
        [Authorize(Roles = "Admin,GeneralManager")]
        public async Task<ActionResult> ExportReportToPdf([FromBody] ExportReportRequestDto request)
        {
            try
            {
                _logger.LogInformation("Exporting report to PDF: {ReportType}", request.ReportType);

                var filter = new ReportFilterDto
                {
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    RegionId = request.RegionId
                };

                var pdfBytes = await _reportService.ExportReportToPdfAsync(request.ReportType, filter);

                return File(pdfBytes, "application/pdf", $"{request.ReportType}_{DateTime.UtcNow:yyyyMMdd}.pdf");
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, new ApiResponse<object>(null!, false, "PDF export feature is not yet implemented", 501));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting report to PDF");
                return StatusCode(500, new ApiResponse<object>(null!, false, "Failed to export report to PDF", 500));
            }
        }

        /// <summary>
        /// Export report to Excel
        /// </summary>
        [HttpPost("export/excel")]
        [Authorize(Roles = "Admin,GeneralManager")]
        public async Task<ActionResult> ExportReportToExcel([FromBody] ExportReportRequestDto request)
        {
            try
            {
                _logger.LogInformation("Exporting report to Excel: {ReportType}", request.ReportType);

                var filter = new ReportFilterDto
                {
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    RegionId = request.RegionId
                };

                var excelBytes = await _reportService.ExportReportToExcelAsync(request.ReportType, filter);

                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"{request.ReportType}_{DateTime.UtcNow:yyyyMMdd}.xlsx");
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, new ApiResponse<object>(null!, false, "Excel export feature is not yet implemented", 501));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting report to Excel");
                return StatusCode(500, new ApiResponse<object>(null!, false, "Failed to export report to Excel", 500));
            }
        }
    }
}
