using Application.DTOs.Reports;
using Application.Interfaces.Reports;
using Application.Interfaces.MachineManagement;
using Application.Interfaces.MaintenanceOperations;
using Application.Interfaces.UserManagement;
using Application.Interfaces.ProjectManagement;
using Application.Interfaces.StoreManagement;
using Application.Interfaces.ExplosiveInventory;
using Application.Interfaces.Infrastructure.Repositories;
using Application.Interfaces.DrillingOperations;
using Application.Interfaces.BlastingOperations;
using Domain.Entities.MachineManagement;
using Domain.Entities.MaintenanceOperations;
using Domain.Entities.MaintenanceOperations.Enums;
using Domain.Entities.ProjectManagement;
using Domain.Entities.StoreManagement;
using Domain.Entities.StoreManagement.Enums;
using Domain.Entities.ExplosiveInventory.Enums;
using Domain.Entities.UserManagement;
using Domain.Entities.DrillingOperations;
using Domain.Entities.BlastingOperations;
using Microsoft.Extensions.Logging;

namespace Application.Services.Reports
{
    /// <summary>
    /// Main report service containing dependencies, constructor, and metadata
    /// </summary>
    public partial class ReportService : IReportService
    {
        private readonly IMachineRepository _machineRepository;
        private readonly IMaintenanceJobRepository _maintenanceJobRepository;
        private readonly IMaintenanceReportRepository _maintenanceReportRepository;
        private readonly IUsageLogRepository _usageLogRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IStoreInventoryRepository _storeInventoryRepository;
        private readonly IInventoryTransferRequestRepository _transferRequestRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IStoreTransactionRepository _storeTransactionRepository;
        private readonly IExplosiveApprovalRequestRepository _explosiveApprovalRequestRepository;
        private readonly IDrillHoleRepository _drillHoleRepository;
        private readonly IDrillPointRepository _drillPointRepository;
        private readonly IBlastConnectionRepository _blastConnectionRepository;
        private readonly IExplosiveCalculationResultRepository _explosiveCalculationResultRepository;
        private readonly ILogger<ReportService> _logger;

        public ReportService(
            IMachineRepository machineRepository,
            IMaintenanceJobRepository maintenanceJobRepository,
            IMaintenanceReportRepository maintenanceReportRepository,
            IUsageLogRepository usageLogRepository,
            IUserRepository userRepository,
            IProjectRepository projectRepository,
            IStoreInventoryRepository storeInventoryRepository,
            IInventoryTransferRequestRepository transferRequestRepository,
            IStoreRepository storeRepository,
            IStoreTransactionRepository storeTransactionRepository,
            IExplosiveApprovalRequestRepository explosiveApprovalRequestRepository,
            IDrillHoleRepository drillHoleRepository,
            IDrillPointRepository drillPointRepository,
            IBlastConnectionRepository blastConnectionRepository,
            IExplosiveCalculationResultRepository explosiveCalculationResultRepository,
            ILogger<ReportService> logger)
        {
            _machineRepository = machineRepository;
            _maintenanceJobRepository = maintenanceJobRepository;
            _maintenanceReportRepository = maintenanceReportRepository;
            _usageLogRepository = usageLogRepository;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _storeInventoryRepository = storeInventoryRepository;
            _transferRequestRepository = transferRequestRepository;
            _storeRepository = storeRepository;
            _storeTransactionRepository = storeTransactionRepository;
            _explosiveApprovalRequestRepository = explosiveApprovalRequestRepository;
            _drillHoleRepository = drillHoleRepository;
            _drillPointRepository = drillPointRepository;
            _blastConnectionRepository = blastConnectionRepository;
            _explosiveCalculationResultRepository = explosiveCalculationResultRepository;
            _logger = logger;
        }

        public async Task<List<ReportMetadataDto>> GetAvailableReportsAsync()
        {
            return new List<ReportMetadataDto>
            {
                // REMOVED: Fleet Management Report - Only 2 machines, MachineAssignments table empty
                // REMOVED: Inventory Status Report - StoreInventories table empty
                // REMOVED: Operational Efficiency Report - Insufficient data for meaningful metrics

                new ReportMetadataDto
                {
                    Id = "maintenance-performance",
                    Name = "Maintenance Performance Report",
                    Description = "Maintenance job statistics, completion rates, and engineer workload analysis",
                    Icon = "pi-wrench",
                    Category = "Maintenance",
                    IsAvailable = true
                },
                // REMOVED: Regional Performance Report - Not needed for current operations
                // new ReportMetadataDto
                // {
                //     Id = "regional-performance",
                //     Name = "Regional Performance Report",
                //     Description = "Comparative analysis of performance metrics across regions",
                //     Icon = "pi-map-marker",
                //     Category = "Analytics",
                //     IsAvailable = true
                // },
                // PHASE 3: Drilling Operations Report - Core business data
                new ReportMetadataDto
                {
                    Id = "drilling-operations",
                    Name = "Drilling Operations Report",
                    Description = "Comprehensive drilling statistics, hole data, blast connections, and project site progress (164 drill holes)",
                    Icon = "pi-bolt",
                    Category = "Operations",
                    IsAvailable = true
                },
                // PHASE 4: Explosive Workflow Report - Approval & Transfer tracking
                new ReportMetadataDto
                {
                    Id = "explosive-workflow",
                    Name = "Explosive Workflow Report",
                    Description = "Approval and transfer request tracking, turnaround analysis, and workflow statistics (12 requests)",
                    Icon = "pi-check-circle",
                    Category = "Operations",
                    IsAvailable = true
                },
                // PHASE 5: User & Access Management Report - Admin oversight
                new ReportMetadataDto
                {
                    Id = "user-access",
                    Name = "User & Access Management Report",
                    Description = "User statistics, role distribution, permission matrix, and access analysis (8 users, 7 roles)",
                    Icon = "pi-users",
                    Category = "Analytics",
                    IsAvailable = true
                }
            };
        }
    }
}
