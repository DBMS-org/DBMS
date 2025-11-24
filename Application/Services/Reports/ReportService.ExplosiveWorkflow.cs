using Application.DTOs.Reports;
using Domain.Entities.ProjectManagement;
using Domain.Entities.ExplosiveInventory.Enums;
using Microsoft.Extensions.Logging;

namespace Application.Services.Reports
{
    /// <summary>
    /// Explosive Workflow Report implementation for Phase 4
    /// Generates comprehensive approval and transfer request workflow analysis
    /// </summary>
    public partial class ReportService
    {
        public async Task<ExplosiveWorkflowReportDto> GetExplosiveWorkflowReportAsync(ReportFilterDto? filter = null)
        {
            try
            {
                _logger.LogInformation("Generating Explosive Workflow Report");

                var report = new ExplosiveWorkflowReportDto
                {
                    GeneratedAt = DateTime.UtcNow,
                    StartDate = filter?.StartDate,
                    EndDate = filter?.EndDate
                };

                // Get all approval requests (including approved, rejected, cancelled)
                var approvalRequests = (await _explosiveApprovalRequestRepository.GetAllAsync()).ToList();

                // Fetch all transfer requests
                var transferRequests = (await _transferRequestRepository.GetAllAsync()).ToList();

                // Apply date filters if provided
                if (filter?.StartDate.HasValue == true)
                {
                    approvalRequests = approvalRequests.Where(a => a.CreatedAt >= filter.StartDate.Value).ToList();
                    transferRequests = transferRequests.Where(t => t.RequestDate >= filter.StartDate.Value).ToList();
                }

                if (filter?.EndDate.HasValue == true)
                {
                    approvalRequests = approvalRequests.Where(a => a.CreatedAt <= filter.EndDate.Value).ToList();
                    transferRequests = transferRequests.Where(t => t.RequestDate <= filter.EndDate.Value).ToList();
                }

                // Calculate overall statistics
                report.Statistics = CalculateWorkflowStatistics(approvalRequests, transferRequests);

                // Map approval requests to DTOs
                report.ApprovalRequests = await MapApprovalRequestsAsync(approvalRequests);

                // Calculate approval analysis
                report.ApprovalAnalysis = CalculateApprovalAnalysis(approvalRequests, report.ApprovalRequests);

                // Map transfer requests to DTOs
                report.TransferRequests = await MapTransferRequestsAsync(transferRequests);

                // Calculate transfer analysis
                report.TransferAnalysis = CalculateTransferAnalysis(transferRequests, report.TransferRequests);

                // Calculate turnaround analysis
                report.TurnaroundAnalysis = CalculateTurnaroundAnalysis(report.ApprovalRequests, report.TransferRequests);

                // Calculate requester statistics
                report.RequesterStatistics = await CalculateRequesterStatisticsAsync(approvalRequests, transferRequests);

                _logger.LogInformation($"Explosive Workflow Report generated with {approvalRequests.Count} approval requests and {transferRequests.Count} transfer requests");

                return report;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating Explosive Workflow Report");
                throw;
            }
        }

        private WorkflowStatisticsDto CalculateWorkflowStatistics(
            List<Domain.Entities.ProjectManagement.ExplosiveApprovalRequest> approvalRequests,
            List<Domain.Entities.ExplosiveInventory.InventoryTransferRequest> transferRequests)
        {
            var stats = new WorkflowStatisticsDto();

            // Approval Request Stats
            stats.TotalApprovalRequests = approvalRequests.Count;
            stats.PendingApprovals = approvalRequests.Count(a => a.Status == ExplosiveApprovalStatus.Pending);
            stats.ApprovedRequests = approvalRequests.Count(a => a.Status == ExplosiveApprovalStatus.Approved);
            stats.RejectedRequests = approvalRequests.Count(a => a.Status == ExplosiveApprovalStatus.Rejected);
            stats.CancelledApprovals = approvalRequests.Count(a => a.Status == ExplosiveApprovalStatus.Cancelled);
            stats.ExpiredApprovals = approvalRequests.Count(a => a.Status == ExplosiveApprovalStatus.Expired);

            // Calculate approval rate (approved / (approved + rejected))
            var processedApprovals = stats.ApprovedRequests + stats.RejectedRequests;
            stats.ApprovalRate = processedApprovals > 0
                ? Math.Round((decimal)stats.ApprovedRequests / processedApprovals * 100, 2)
                : 0;

            // Transfer Request Stats
            stats.TotalTransferRequests = transferRequests.Count;
            stats.PendingTransfers = transferRequests.Count(t => t.Status == TransferRequestStatus.Pending);
            stats.ApprovedTransfers = transferRequests.Count(t => t.Status == TransferRequestStatus.Approved);
            stats.RejectedTransfers = transferRequests.Count(t => t.Status == TransferRequestStatus.Rejected);
            stats.InProgressTransfers = transferRequests.Count(t => t.Status == TransferRequestStatus.InProgress);
            stats.CompletedTransfers = transferRequests.Count(t => t.Status == TransferRequestStatus.Completed);
            stats.CancelledTransfers = transferRequests.Count(t => t.Status == TransferRequestStatus.Cancelled);

            stats.TransferCompletionRate = stats.TotalTransferRequests > 0
                ? Math.Round((decimal)stats.CompletedTransfers / stats.TotalTransferRequests * 100, 2)
                : 0;

            // Overall Stats
            stats.TotalWorkflowItems = stats.TotalApprovalRequests + stats.TotalTransferRequests;
            stats.TotalPending = stats.PendingApprovals + stats.PendingTransfers;
            stats.TotalCompleted = stats.ApprovedRequests + stats.CompletedTransfers;
            stats.OverallCompletionRate = stats.TotalWorkflowItems > 0
                ? Math.Round((decimal)stats.TotalCompleted / stats.TotalWorkflowItems * 100, 2)
                : 0;

            return stats;
        }

        private async Task<List<ApprovalRequestSummaryDto>> MapApprovalRequestsAsync(
            List<Domain.Entities.ProjectManagement.ExplosiveApprovalRequest> approvalRequests)
        {
            var summaries = new List<ApprovalRequestSummaryDto>();

            foreach (var request in approvalRequests)
            {
                var projectSite = request.ProjectSite;
                var requestedByUser = request.RequestedByUser;
                var processedByUser = request.ProcessedByUser;

                var turnaroundDays = request.ProcessedAt.HasValue
                    ? (int)(request.ProcessedAt.Value - request.CreatedAt).TotalDays
                    : (int?)null;

                summaries.Add(new ApprovalRequestSummaryDto
                {
                    Id = request.Id,
                    ProjectSiteName = projectSite?.Name ?? "N/A",
                    RegionName = "N/A", // ProjectSite doesn't have Region navigation
                    RequestedByUserName = requestedByUser?.Name ?? "Unknown",
                    ExpectedUsageDate = request.ExpectedUsageDate,
                    CreatedDate = request.CreatedAt,
                    Status = request.Status.ToString(),
                    Priority = request.Priority.ToString(),
                    ApprovalType = request.ApprovalType.ToString(),
                    ProcessedByUserName = processedByUser?.Name,
                    ProcessedAt = request.ProcessedAt,
                    Comments = request.Comments,
                    RejectionReason = request.RejectionReason,
                    SafetyChecklistCompleted = request.SafetyChecklistCompleted,
                    EnvironmentalAssessmentCompleted = request.EnvironmentalAssessmentCompleted,
                    BlastingDate = request.BlastingDate,
                    BlastTiming = request.BlastTiming,
                    TurnaroundDays = turnaroundDays
                });
            }

            return summaries;
        }

        private ApprovalAnalysisDto CalculateApprovalAnalysis(
            List<Domain.Entities.ProjectManagement.ExplosiveApprovalRequest> approvalRequests,
            List<ApprovalRequestSummaryDto> summaries)
        {
            var analysis = new ApprovalAnalysisDto
            {
                TotalRequests = approvalRequests.Count
            };

            // Group by status
            analysis.ByStatus = approvalRequests
                .GroupBy(a => a.Status.ToString())
                .ToDictionary(g => g.Key, g => g.Count());

            // Group by priority
            analysis.ByPriority = approvalRequests
                .GroupBy(a => a.Priority.ToString())
                .ToDictionary(g => g.Key, g => g.Count());

            // Group by approval type
            analysis.ByApprovalType = approvalRequests
                .GroupBy(a => a.ApprovalType.ToString())
                .ToDictionary(g => g.Key, g => g.Count());

            // Group by project site
            analysis.ByProjectSite = approvalRequests
                .Where(a => a.ProjectSite != null)
                .GroupBy(a => a.ProjectSite.Name)
                .ToDictionary(g => g.Key, g => g.Count());

            // Group by region - ProjectSite doesn't have Region navigation
            analysis.ByRegion = new Dictionary<string, int>();

            // Compliance stats
            analysis.SafetyChecklistCompletedCount = approvalRequests.Count(a => a.SafetyChecklistCompleted);
            analysis.EnvironmentalAssessmentCompletedCount = approvalRequests.Count(a => a.EnvironmentalAssessmentCompleted);

            var totalCompliance = analysis.SafetyChecklistCompletedCount + analysis.EnvironmentalAssessmentCompletedCount;
            var totalPossible = approvalRequests.Count * 2; // Both checklists
            analysis.ComplianceRate = totalPossible > 0
                ? Math.Round((decimal)totalCompliance / totalPossible * 100, 2)
                : 0;

            // Average turnaround
            var turnaroundDays = summaries.Where(s => s.TurnaroundDays.HasValue).Select(s => s.TurnaroundDays.Value).ToList();
            analysis.AverageTurnaroundDays = turnaroundDays.Any()
                ? Math.Round((decimal)turnaroundDays.Average(), 2)
                : 0;

            return analysis;
        }

        private async Task<List<WorkflowTransferRequestDto>> MapTransferRequestsAsync(
            List<Domain.Entities.ExplosiveInventory.InventoryTransferRequest> transferRequests)
        {
            var summaries = new List<WorkflowTransferRequestDto>();

            foreach (var request in transferRequests)
            {
                var centralInventory = request.CentralInventory;
                var destinationStore = request.DestinationStore;
                var requestedByUser = request.RequestedByUser;
                var approvedByUser = request.ApprovedByUser;
                var processedByUser = request.ProcessedByUser;

                var turnaroundDays = request.CompletedDate.HasValue
                    ? (int)(request.CompletedDate.Value - request.RequestDate).TotalDays
                    : request.ApprovedDate.HasValue
                        ? (int)(request.ApprovedDate.Value - request.RequestDate).TotalDays
                        : (int?)null;

                summaries.Add(new WorkflowTransferRequestDto
                {
                    Id = centralInventory?.Id ?? 0,
                    RequestNumber = request.RequestNumber,
                    ExplosiveTypeName = centralInventory?.ExplosiveType.ToString() ?? "Unknown",
                    DestinationStoreName = destinationStore?.StoreName ?? "N/A",
                    DestinationRegion = "N/A", // Store doesn't have Region navigation
                    RequestedQuantity = request.RequestedQuantity,
                    ApprovedQuantity = request.ApprovedQuantity,
                    Unit = request.Unit,
                    Status = request.Status.ToString(),
                    RequestDate = request.RequestDate,
                    ApprovedDate = request.ApprovedDate,
                    CompletedDate = request.CompletedDate,
                    RequiredByDate = request.RequiredByDate,
                    RequestedByUserName = requestedByUser?.Name ?? "Unknown",
                    ApprovedByUserName = approvedByUser?.Name,
                    ProcessedByUserName = processedByUser?.Name,
                    TruckNumber = request.TruckNumber,
                    DriverName = request.DriverName,
                    DispatchDate = request.DispatchDate,
                    RejectionReason = request.RejectionReason,
                    TurnaroundDays = turnaroundDays,
                    IsOverdue = request.IsOverdue(),
                    IsUrgent = request.IsUrgent()
                });
            }

            return summaries;
        }

        private TransferAnalysisDto CalculateTransferAnalysis(
            List<Domain.Entities.ExplosiveInventory.InventoryTransferRequest> transferRequests,
            List<WorkflowTransferRequestDto> summaries)
        {
            var analysis = new TransferAnalysisDto
            {
                TotalRequests = transferRequests.Count
            };

            // Group by status
            analysis.ByStatus = transferRequests
                .GroupBy(t => t.Status.ToString())
                .ToDictionary(g => g.Key, g => g.Count());

            // Group by explosive type
            analysis.ByExplosiveType = transferRequests
                .Where(t => t.CentralInventory?.ExplosiveType != null)
                .GroupBy(t => t.CentralInventory.ExplosiveType.ToString())
                .ToDictionary(g => g.Key, g => g.Count());

            // Group by destination store
            analysis.ByDestinationStore = transferRequests
                .Where(t => t.DestinationStore != null)
                .GroupBy(t => t.DestinationStore.StoreName)
                .ToDictionary(g => g.Key, g => g.Count());

            // Group by destination region - Store doesn't have Region navigation
            analysis.ByDestinationRegion = new Dictionary<string, int>();

            // Quantity analysis
            analysis.TotalRequestedQuantity = transferRequests.Sum(t => t.RequestedQuantity);
            analysis.TotalApprovedQuantity = transferRequests
                .Where(t => t.ApprovedQuantity.HasValue &&
                           (t.Status == TransferRequestStatus.Approved ||
                            t.Status == TransferRequestStatus.InProgress ||
                            t.Status == TransferRequestStatus.Completed))
                .Sum(t => t.ApprovedQuantity.Value);
            analysis.TotalTransferredQuantity = transferRequests
                .Where(t => t.Status == TransferRequestStatus.Completed && t.ApprovedQuantity.HasValue)
                .Sum(t => t.ApprovedQuantity.Value);

            // Overdue and urgent counts
            analysis.OverdueCount = summaries.Count(s => s.IsOverdue);
            analysis.UrgentCount = summaries.Count(s => s.IsUrgent);

            // Average turnaround
            var turnaroundDays = summaries.Where(s => s.TurnaroundDays.HasValue).Select(s => s.TurnaroundDays.Value).ToList();
            analysis.AverageTurnaroundDays = turnaroundDays.Any()
                ? Math.Round((decimal)turnaroundDays.Average(), 2)
                : 0;

            // Fulfillment rate
            analysis.QuantityFulfillmentRate = analysis.TotalRequestedQuantity > 0
                ? Math.Round(analysis.TotalApprovedQuantity / analysis.TotalRequestedQuantity * 100, 2)
                : 0;

            return analysis;
        }

        private TurnaroundAnalysisDto CalculateTurnaroundAnalysis(
            List<ApprovalRequestSummaryDto> approvalSummaries,
            List<WorkflowTransferRequestDto> transferSummaries)
        {
            var analysis = new TurnaroundAnalysisDto();

            // Approval turnaround
            var approvalTurnarounds = approvalSummaries
                .Where(a => a.TurnaroundDays.HasValue)
                .Select(a => (decimal)a.TurnaroundDays.Value)
                .ToList();

            if (approvalTurnarounds.Any())
            {
                analysis.AverageApprovalTurnaroundDays = Math.Round(approvalTurnarounds.Average(), 2);
                analysis.MinApprovalTurnaroundDays = approvalTurnarounds.Min();
                analysis.MaxApprovalTurnaroundDays = approvalTurnarounds.Max();
                analysis.ProcessedApprovalCount = approvalTurnarounds.Count;
            }

            // Transfer turnaround
            var transferTurnarounds = transferSummaries
                .Where(t => t.TurnaroundDays.HasValue)
                .Select(t => (decimal)t.TurnaroundDays.Value)
                .ToList();

            if (transferTurnarounds.Any())
            {
                analysis.AverageTransferTurnaroundDays = Math.Round(transferTurnarounds.Average(), 2);
                analysis.MinTransferTurnaroundDays = transferTurnarounds.Min();
                analysis.MaxTransferTurnaroundDays = transferTurnarounds.Max();
                analysis.ProcessedTransferCount = transferTurnarounds.Count;
            }

            // Overall turnaround
            var allTurnarounds = approvalTurnarounds.Concat(transferTurnarounds).ToList();
            if (allTurnarounds.Any())
            {
                analysis.OverallAverageTurnaroundDays = Math.Round(allTurnarounds.Average(), 2);
                analysis.TotalProcessedRequests = allTurnarounds.Count;
            }

            return analysis;
        }

        private async Task<List<RequesterStatisticsDto>> CalculateRequesterStatisticsAsync(
            List<Domain.Entities.ProjectManagement.ExplosiveApprovalRequest> approvalRequests,
            List<Domain.Entities.ExplosiveInventory.InventoryTransferRequest> transferRequests)
        {
            var stats = new List<RequesterStatisticsDto>();

            // Get all unique requesters
            var allUserIds = approvalRequests.Select(a => a.RequestedByUserId)
                .Concat(transferRequests.Select(t => t.RequestedByUserId))
                .Distinct()
                .ToList();

            var allUsers = (await _userRepository.GetAllAsync()).ToList();

            foreach (var userId in allUserIds)
            {
                var user = allUsers.FirstOrDefault(u => u.Id == userId);
                if (user == null) continue;

                var userApprovals = approvalRequests.Where(a => a.RequestedByUserId == userId).ToList();
                var userTransfers = transferRequests.Where(t => t.RequestedByUserId == userId).ToList();

                var approvedCount = userTransfers.Count(t => t.Status == TransferRequestStatus.Approved ||
                                                           t.Status == TransferRequestStatus.Completed);

                var rejectedCount = userTransfers.Count(t => t.Status == TransferRequestStatus.Rejected);

                var pendingCount = userApprovals.Count(a => a.Status == ExplosiveApprovalStatus.Pending) +
                                  userTransfers.Count(t => t.Status == TransferRequestStatus.Pending);

                var totalRequests = userApprovals.Count + userTransfers.Count;
                var processedRequests = approvedCount + rejectedCount;

                stats.Add(new RequesterStatisticsDto
                {
                    UserName = user.Name,
                    UserRole = user.Role ?? "Unknown",
                    TotalApprovalRequests = userApprovals.Count,
                    TotalTransferRequests = userTransfers.Count,
                    TotalRequests = totalRequests,
                    ApprovedCount = approvedCount,
                    RejectedCount = rejectedCount,
                    PendingCount = pendingCount,
                    ApprovalRate = processedRequests > 0
                        ? Math.Round((decimal)approvedCount / processedRequests * 100, 2)
                        : 0
                });
            }

            return stats.OrderByDescending(s => s.TotalRequests).ToList();
        }
    }
}
