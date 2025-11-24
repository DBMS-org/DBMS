using System;
using System.Collections.Generic;

namespace API.Models;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Region { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string OmanPhone { get; set; } = null!;

    public string CountryPhone { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string? PasswordResetCode { get; set; }

    public DateTime? PasswordResetCodeExpiry { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<BlastSequence> BlastSequences { get; set; } = new List<BlastSequence>();

    public virtual ICollection<DrillPattern> DrillPatterns { get; set; } = new List<DrillPattern>();

    public virtual ICollection<ExplosiveApprovalRequest> ExplosiveApprovalRequestProcessedByUsers { get; set; } = new List<ExplosiveApprovalRequest>();

    public virtual ICollection<ExplosiveApprovalRequest> ExplosiveApprovalRequestRequestedByUsers { get; set; } = new List<ExplosiveApprovalRequest>();

    public virtual ICollection<ExplosiveCalculationResult> ExplosiveCalculationResults { get; set; } = new List<ExplosiveCalculationResult>();

    public virtual ICollection<InventoryTransferRequest> InventoryTransferRequestApprovedByUsers { get; set; } = new List<InventoryTransferRequest>();

    public virtual ICollection<InventoryTransferRequest> InventoryTransferRequestDispatchedByUsers { get; set; } = new List<InventoryTransferRequest>();

    public virtual ICollection<InventoryTransferRequest> InventoryTransferRequestProcessedByUsers { get; set; } = new List<InventoryTransferRequest>();

    public virtual ICollection<InventoryTransferRequest> InventoryTransferRequestRequestedByUsers { get; set; } = new List<InventoryTransferRequest>();

    public virtual ICollection<MachineAssignment> MachineAssignments { get; set; } = new List<MachineAssignment>();

    public virtual ICollection<MachineUsageLog> MachineUsageLogs { get; set; } = new List<MachineUsageLog>();

    public virtual ICollection<Machine> Machines { get; set; } = new List<Machine>();

    public virtual ICollection<MaintenanceJobAssignment> MaintenanceJobAssignments { get; set; } = new List<MaintenanceJobAssignment>();

    public virtual ICollection<MaintenanceJob> MaintenanceJobs { get; set; } = new List<MaintenanceJob>();

    public virtual ICollection<MaintenanceReport> MaintenanceReportMechanicalEngineers { get; set; } = new List<MaintenanceReport>();

    public virtual ICollection<MaintenanceReport> MaintenanceReportOperators { get; set; } = new List<MaintenanceReport>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<PasswordResetCode> PasswordResetCodes { get; set; } = new List<PasswordResetCode>();

    public virtual Project? Project { get; set; }

    public virtual ICollection<ProjectSite> ProjectSites { get; set; } = new List<ProjectSite>();

    public virtual ICollection<QualityCheckRecord> QualityCheckRecords { get; set; } = new List<QualityCheckRecord>();

    public virtual ICollection<SiteBlastingDatum> SiteBlastingData { get; set; } = new List<SiteBlastingDatum>();

    public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();

    public virtual ICollection<StockRequest> StockRequestApprovedBies { get; set; } = new List<StockRequest>();

    public virtual ICollection<StockRequest> StockRequestRejectedBies { get; set; } = new List<StockRequest>();

    public virtual ICollection<StockRequest> StockRequestRequesters { get; set; } = new List<StockRequest>();

    public virtual ICollection<Store> StoreManagerUsers { get; set; } = new List<Store>();

    public virtual ICollection<StoreTransaction> StoreTransactions { get; set; } = new List<StoreTransaction>();

    public virtual ICollection<Store> StoreUsers { get; set; } = new List<Store>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
