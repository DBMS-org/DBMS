using System;
using System.Collections.Generic;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public partial class DbMsContext : DbContext
{
    public DbMsContext()
    {
    }

    public DbMsContext(DbContextOptions<DbMsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Accessory> Accessories { get; set; }

    public virtual DbSet<AccessoryStockAdjustment> AccessoryStockAdjustments { get; set; }

    public virtual DbSet<AnfotechnicalProperty> AnfotechnicalProperties { get; set; }

    public virtual DbSet<BlastConnection> BlastConnections { get; set; }

    public virtual DbSet<BlastSequence> BlastSequences { get; set; }

    public virtual DbSet<CentralWarehouseInventory> CentralWarehouseInventories { get; set; }

    public virtual DbSet<DetonatorInfo> DetonatorInfos { get; set; }

    public virtual DbSet<DrillHole> DrillHoles { get; set; }

    public virtual DbSet<DrillPattern> DrillPatterns { get; set; }

    public virtual DbSet<DrillPoint> DrillPoints { get; set; }

    public virtual DbSet<EmulsionTechnicalProperty> EmulsionTechnicalProperties { get; set; }

    public virtual DbSet<ExplosiveApprovalRequest> ExplosiveApprovalRequests { get; set; }

    public virtual DbSet<ExplosiveCalculationResult> ExplosiveCalculationResults { get; set; }

    public virtual DbSet<InventoryTransferRequest> InventoryTransferRequests { get; set; }

    public virtual DbSet<Machine> Machines { get; set; }

    public virtual DbSet<MachineAssignment> MachineAssignments { get; set; }

    public virtual DbSet<MachineAssignmentRequest> MachineAssignmentRequests { get; set; }

    public virtual DbSet<MachineUsageLog> MachineUsageLogs { get; set; }

    public virtual DbSet<MaintenanceJob> MaintenanceJobs { get; set; }

    public virtual DbSet<MaintenanceJobAssignment> MaintenanceJobAssignments { get; set; }

    public virtual DbSet<MaintenanceReport> MaintenanceReports { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<PasswordResetCode> PasswordResetCodes { get; set; }

    public virtual DbSet<PatternSetting> PatternSettings { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectSite> ProjectSites { get; set; }

    public virtual DbSet<QualityCheckRecord> QualityCheckRecords { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<SiteBlastingDatum> SiteBlastingData { get; set; }

    public virtual DbSet<StockMovement> StockMovements { get; set; }

    public virtual DbSet<StockRequest> StockRequests { get; set; }

    public virtual DbSet<StockRequestItem> StockRequestItems { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<StoreInventory> StoreInventories { get; set; }

    public virtual DbSet<StoreInventoryRecord> StoreInventoryRecords { get; set; }

    public virtual DbSet<StoreTransaction> StoreTransactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Accessory>(entity =>
        {
            entity.HasIndex(e => e.PartNumber, "IX_Accessories_PartNumber").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PartNumber).HasMaxLength(100);
            entity.Property(e => e.Supplier).HasMaxLength(200);
        });

        modelBuilder.Entity<AccessoryStockAdjustment>(entity =>
        {
            entity.HasIndex(e => e.AccessoryId, "IX_AccessoryStockAdjustments_AccessoryId");

            entity.HasIndex(e => e.AdjustedDate, "IX_AccessoryStockAdjustments_AdjustedDate");

            entity.Property(e => e.AdjustedBy).HasMaxLength(100);
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(d => d.Accessory).WithMany(p => p.AccessoryStockAdjustments).HasForeignKey(d => d.AccessoryId);
        });

        modelBuilder.Entity<AnfotechnicalProperty>(entity =>
        {
            entity.ToTable("ANFOTechnicalProperties");

            entity.HasIndex(e => e.CentralWarehouseInventoryId, "IX_ANFOTechnicalProperties_CentralWarehouseInventoryId").IsUnique();

            entity.Property(e => e.Density).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.FuelOilContent).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.MoistureContent).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.Notes).HasMaxLength(2000);
            entity.Property(e => e.PrillSize).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.StorageHumidity).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.StorageTemperature).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.WaterResistance)
                .HasMaxLength(50)
                .HasDefaultValue("None");

            entity.HasOne(d => d.CentralWarehouseInventory).WithOne(p => p.AnfotechnicalProperty).HasForeignKey<AnfotechnicalProperty>(d => d.CentralWarehouseInventoryId);
        });

        modelBuilder.Entity<BlastConnection>(entity =>
        {
            entity.HasIndex(e => new { e.Point1DrillPointId, e.ProjectId, e.SiteId }, "IX_BlastConnections_Point1DrillPointId_ProjectId_SiteId");

            entity.HasIndex(e => new { e.Point2DrillPointId, e.ProjectId, e.SiteId }, "IX_BlastConnections_Point2DrillPointId_ProjectId_SiteId");

            entity.HasIndex(e => new { e.ProjectId, e.SiteId }, "IX_BlastConnections_ProjectId_SiteId");

            entity.HasIndex(e => e.Sequence, "IX_BlastConnections_Sequence");

            entity.HasIndex(e => e.SiteId, "IX_BlastConnections_SiteId");

            entity.Property(e => e.Point1DrillPointId).HasDefaultValue("");
            entity.Property(e => e.Point2DrillPointId).HasDefaultValue("");

            entity.HasOne(d => d.Project).WithMany(p => p.BlastConnections)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Site).WithMany(p => p.BlastConnections)
                .HasForeignKey(d => d.SiteId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.DrillPoint).WithMany(p => p.BlastConnectionDrillPoints)
                .HasForeignKey(d => new { d.Point1DrillPointId, d.ProjectId, d.SiteId })
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.DrillPointNavigation).WithMany(p => p.BlastConnectionDrillPointNavigations)
                .HasForeignKey(d => new { d.Point2DrillPointId, d.ProjectId, d.SiteId })
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<BlastSequence>(entity =>
        {
            entity.HasIndex(e => e.CreatedByUserId, "IX_BlastSequences_CreatedByUserId");

            entity.HasIndex(e => e.DrillPatternId, "IX_BlastSequences_DrillPatternId");

            entity.HasIndex(e => e.IsActive, "IX_BlastSequences_IsActive");

            entity.HasIndex(e => new { e.ProjectId, e.SiteId }, "IX_BlastSequences_ProjectId_SiteId");

            entity.HasIndex(e => e.SiteId, "IX_BlastSequences_SiteId");

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.BlastSequences)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.DrillPattern).WithMany(p => p.BlastSequences).HasForeignKey(d => d.DrillPatternId);

            entity.HasOne(d => d.Project).WithMany(p => p.BlastSequences)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Site).WithMany(p => p.BlastSequences)
                .HasForeignKey(d => d.SiteId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<CentralWarehouseInventory>(entity =>
        {
            entity.HasIndex(e => e.BatchId, "IX_CentralWarehouseInventories_BatchId").IsUnique();

            entity.HasIndex(e => e.CentralWarehouseStoreId, "IX_CentralWarehouseInventories_CentralWarehouseStoreId");

            entity.HasIndex(e => e.CreatedAt, "IX_CentralWarehouseInventories_CreatedAt");

            entity.HasIndex(e => e.ExpiryDate, "IX_CentralWarehouseInventories_ExpiryDate");

            entity.HasIndex(e => e.ExplosiveType, "IX_CentralWarehouseInventories_ExplosiveType");

            entity.HasIndex(e => e.Status, "IX_CentralWarehouseInventories_Status");

            entity.HasIndex(e => e.Supplier, "IX_CentralWarehouseInventories_Supplier");

            entity.Property(e => e.AllocatedQuantity).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.AnfotechnicalPropertiesId).HasColumnName("ANFOTechnicalPropertiesId");
            entity.Property(e => e.BatchId).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ManufacturerBatchNumber).HasMaxLength(100);
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.StorageLocation).HasMaxLength(100);
            entity.Property(e => e.Supplier).HasMaxLength(200);
            entity.Property(e => e.Unit).HasMaxLength(10);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.CentralWarehouseStore).WithMany(p => p.CentralWarehouseInventories)
                .HasForeignKey(d => d.CentralWarehouseStoreId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<DetonatorInfo>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.ProjectId, e.SiteId });

            entity.HasIndex(e => e.DrillPointId, "IX_DetonatorInfos_DrillPointId");

            entity.HasIndex(e => new { e.DrillPointId, e.ProjectId, e.SiteId }, "IX_DetonatorInfos_DrillPointId_ProjectId_SiteId");

            entity.HasIndex(e => new { e.ProjectId, e.SiteId }, "IX_DetonatorInfos_ProjectId_SiteId");

            entity.HasIndex(e => e.SiteId, "IX_DetonatorInfos_SiteId");

            entity.Property(e => e.DrillPointId).HasDefaultValue("");

            entity.HasOne(d => d.Project).WithMany(p => p.DetonatorInfos)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Site).WithMany(p => p.DetonatorInfos).HasForeignKey(d => d.SiteId);

            entity.HasOne(d => d.DrillPoint).WithMany(p => p.DetonatorInfos)
                .HasForeignKey(d => new { d.DrillPointId, d.ProjectId, d.SiteId })
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<DrillHole>(entity =>
        {
            entity.Property(e => e.Depth).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Easting).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Elevation).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Length).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Northing).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<DrillPattern>(entity =>
        {
            entity.HasIndex(e => e.CreatedById, "IX_DrillPatterns_CreatedById");

            entity.HasIndex(e => e.IsActive, "IX_DrillPatterns_IsActive");

            entity.HasIndex(e => new { e.ProjectId, e.SiteId }, "IX_DrillPatterns_ProjectId_SiteId");

            entity.HasIndex(e => e.SiteId, "IX_DrillPatterns_SiteId");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.DrillPatterns).HasForeignKey(d => d.CreatedById);

            entity.HasOne(d => d.Project).WithMany(p => p.DrillPatterns)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Site).WithMany(p => p.DrillPatterns).HasForeignKey(d => d.SiteId);
        });

        modelBuilder.Entity<DrillPoint>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.ProjectId, e.SiteId });

            entity.HasIndex(e => new { e.ProjectId, e.SiteId }, "IX_DrillPoints_ProjectId_SiteId");

            entity.HasIndex(e => e.SiteId, "IX_DrillPoints_SiteId");

            entity.HasIndex(e => new { e.X, e.Y, e.ProjectId, e.SiteId }, "IX_DrillPoints_X_Y_ProjectId_SiteId");

            entity.Property(e => e.Anfo).HasColumnName("ANFO");
            entity.Property(e => e.Diameter).HasDefaultValue(0.89f);
            entity.Property(e => e.Stemming).HasDefaultValue(3f);

            entity.HasOne(d => d.Project).WithMany(p => p.DrillPoints)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Site).WithMany(p => p.DrillPoints)
                .HasForeignKey(d => d.SiteId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<EmulsionTechnicalProperty>(entity =>
        {
            entity.HasIndex(e => e.CentralWarehouseInventoryId, "IX_EmulsionTechnicalProperties_CentralWarehouseInventoryId").IsUnique();

            entity.Property(e => e.ApplicationTemperature).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.DensitySensitized).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.DensityUnsensitized).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.Notes).HasMaxLength(2000);
            entity.Property(e => e.PH)
                .HasColumnType("decimal(3, 1)")
                .HasColumnName("pH");
            entity.Property(e => e.SensitizerContent).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.StorageTemperature).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.WaterContent).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.WaterResistance)
                .HasMaxLength(50)
                .HasDefaultValue("Excellent");

            entity.HasOne(d => d.CentralWarehouseInventory).WithOne(p => p.EmulsionTechnicalProperty).HasForeignKey<EmulsionTechnicalProperty>(d => d.CentralWarehouseInventoryId);
        });

        modelBuilder.Entity<ExplosiveApprovalRequest>(entity =>
        {
            entity.HasIndex(e => e.ExpectedUsageDate, "IX_ExplosiveApprovalRequests_ExpectedUsageDate");

            entity.HasIndex(e => e.ProcessedByUserId, "IX_ExplosiveApprovalRequests_ProcessedByUserId");

            entity.HasIndex(e => e.ProjectSiteId, "IX_ExplosiveApprovalRequests_ProjectSiteId");

            entity.HasIndex(e => new { e.ProjectSiteId, e.Status }, "IX_ExplosiveApprovalRequests_ProjectSite_Status");

            entity.HasIndex(e => e.RequestedByUserId, "IX_ExplosiveApprovalRequests_RequestedByUserId");

            entity.HasIndex(e => e.Status, "IX_ExplosiveApprovalRequests_Status");

            entity.Property(e => e.Comments).HasMaxLength(1000);
            entity.Property(e => e.EstimatedDurationHours).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RejectionReason).HasMaxLength(500);

            entity.HasOne(d => d.ProcessedByUser).WithMany(p => p.ExplosiveApprovalRequestProcessedByUsers).HasForeignKey(d => d.ProcessedByUserId);

            entity.HasOne(d => d.ProjectSite).WithMany(p => p.ExplosiveApprovalRequests).HasForeignKey(d => d.ProjectSiteId);

            entity.HasOne(d => d.RequestedByUser).WithMany(p => p.ExplosiveApprovalRequestRequestedByUsers)
                .HasForeignKey(d => d.RequestedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<ExplosiveCalculationResult>(entity =>
        {
            entity.HasIndex(e => e.CalculationId, "IX_ExplosiveCalculationResults_CalculationId").IsUnique();

            entity.HasIndex(e => e.OwningUserId, "IX_ExplosiveCalculationResults_OwningUserId");

            entity.HasIndex(e => e.PatternSettingsId, "IX_ExplosiveCalculationResults_PatternSettingsId");

            entity.HasIndex(e => new { e.ProjectId, e.SiteId, e.CreatedAt }, "IX_ExplosiveCalculationResults_ProjectSite_CreatedAt");

            entity.HasIndex(e => new { e.ProjectId, e.SiteId }, "IX_ExplosiveCalculationResults_ProjectSite_Unique").IsUnique();

            entity.HasIndex(e => e.SiteId, "IX_ExplosiveCalculationResults_SiteId");

            entity.HasOne(d => d.OwningUser).WithMany(p => p.ExplosiveCalculationResults)
                .HasForeignKey(d => d.OwningUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.PatternSettings).WithMany(p => p.ExplosiveCalculationResults)
                .HasForeignKey(d => d.PatternSettingsId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.Project).WithMany(p => p.ExplosiveCalculationResults)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Site).WithMany(p => p.ExplosiveCalculationResults)
                .HasForeignKey(d => d.SiteId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<InventoryTransferRequest>(entity =>
        {
            entity.HasIndex(e => e.ApprovedByUserId, "IX_InventoryTransferRequests_ApprovedByUserId");

            entity.HasIndex(e => e.CentralWarehouseInventoryId, "IX_InventoryTransferRequests_CentralWarehouseInventoryId");

            entity.HasIndex(e => e.CompletedTransactionId, "IX_InventoryTransferRequests_CompletedTransactionId");

            entity.HasIndex(e => e.DestinationStoreId, "IX_InventoryTransferRequests_DestinationStoreId");

            entity.HasIndex(e => e.DispatchDate, "IX_InventoryTransferRequests_DispatchDate");

            entity.HasIndex(e => e.DispatchedByUserId, "IX_InventoryTransferRequests_DispatchedByUserId");

            entity.HasIndex(e => e.ProcessedByUserId, "IX_InventoryTransferRequests_ProcessedByUserId");

            entity.HasIndex(e => e.RequestDate, "IX_InventoryTransferRequests_RequestDate");

            entity.HasIndex(e => e.RequestNumber, "IX_InventoryTransferRequests_RequestNumber").IsUnique();

            entity.HasIndex(e => e.RequestedByUserId, "IX_InventoryTransferRequests_RequestedByUserId");

            entity.HasIndex(e => e.RequiredByDate, "IX_InventoryTransferRequests_RequiredByDate");

            entity.HasIndex(e => e.Status, "IX_InventoryTransferRequests_Status");

            entity.Property(e => e.ApprovalNotes).HasMaxLength(1000);
            entity.Property(e => e.ApprovedQuantity).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DispatchNotes).HasMaxLength(1000);
            entity.Property(e => e.DriverContactNumber).HasMaxLength(20);
            entity.Property(e => e.DriverName).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.RejectionReason).HasMaxLength(1000);
            entity.Property(e => e.RequestNotes).HasMaxLength(1000);
            entity.Property(e => e.RequestNumber).HasMaxLength(50);
            entity.Property(e => e.RequestedQuantity).HasColumnType("decimal(18, 3)");
            entity.Property(e => e.TruckNumber).HasMaxLength(50);
            entity.Property(e => e.Unit).HasMaxLength(10);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.ApprovedByUser).WithMany(p => p.InventoryTransferRequestApprovedByUsers).HasForeignKey(d => d.ApprovedByUserId);

            entity.HasOne(d => d.CentralWarehouseInventory).WithMany(p => p.InventoryTransferRequests)
                .HasForeignKey(d => d.CentralWarehouseInventoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.CompletedTransaction).WithMany(p => p.InventoryTransferRequests).HasForeignKey(d => d.CompletedTransactionId);

            entity.HasOne(d => d.DestinationStore).WithMany(p => p.InventoryTransferRequests)
                .HasForeignKey(d => d.DestinationStoreId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.DispatchedByUser).WithMany(p => p.InventoryTransferRequestDispatchedByUsers).HasForeignKey(d => d.DispatchedByUserId);

            entity.HasOne(d => d.ProcessedByUser).WithMany(p => p.InventoryTransferRequestProcessedByUsers).HasForeignKey(d => d.ProcessedByUserId);

            entity.HasOne(d => d.RequestedByUser).WithMany(p => p.InventoryTransferRequestRequestedByUsers)
                .HasForeignKey(d => d.RequestedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Machine>(entity =>
        {
            entity.HasIndex(e => e.OperatorId, "IX_Machines_OperatorId");

            entity.HasIndex(e => e.ProjectId, "IX_Machines_ProjectId");

            entity.HasIndex(e => e.RegionId, "IX_Machines_RegionId");

            entity.HasIndex(e => e.Status, "IX_Machines_Status");

            entity.Property(e => e.CurrentDrifterServiceHours).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CurrentEngineServiceHours).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DrifterServiceInterval).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EngineServiceInterval).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Operator).WithMany(p => p.Machines)
                .HasForeignKey(d => d.OperatorId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.Project).WithMany(p => p.Machines)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.Region).WithMany(p => p.Machines)
                .HasForeignKey(d => d.RegionId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<MachineAssignment>(entity =>
        {
            entity.HasIndex(e => e.AssignedDate, "IX_MachineAssignments_AssignedDate");

            entity.HasIndex(e => e.MachineId, "IX_MachineAssignments_MachineId");

            entity.HasIndex(e => e.OperatorId, "IX_MachineAssignments_OperatorId");

            entity.HasIndex(e => e.ProjectId, "IX_MachineAssignments_ProjectId");

            entity.HasIndex(e => e.Status, "IX_MachineAssignments_Status");

            entity.Property(e => e.AssignedBy).HasMaxLength(100);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Machine).WithMany(p => p.MachineAssignments)
                .HasForeignKey(d => d.MachineId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Operator).WithMany(p => p.MachineAssignments)
                .HasForeignKey(d => d.OperatorId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Project).WithMany(p => p.MachineAssignments)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<MachineAssignmentRequest>(entity =>
        {
            entity.HasIndex(e => e.ProjectId, "IX_MachineAssignmentRequests_ProjectId");

            entity.HasIndex(e => e.RequestedDate, "IX_MachineAssignmentRequests_RequestedDate");

            entity.HasIndex(e => e.Status, "IX_MachineAssignmentRequests_Status");

            entity.HasIndex(e => e.Urgency, "IX_MachineAssignmentRequests_Urgency");

            entity.Property(e => e.ApprovedBy).HasMaxLength(100);
            entity.Property(e => e.Comments).HasMaxLength(500);
            entity.Property(e => e.DetailsOrExplanation).HasMaxLength(1000);
            entity.Property(e => e.ExpectedUsageDuration).HasMaxLength(100);
            entity.Property(e => e.MachineType).HasMaxLength(50);
            entity.Property(e => e.RequestedBy).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(30);
            entity.Property(e => e.Urgency).HasMaxLength(20);

            entity.HasOne(d => d.Project).WithMany(p => p.MachineAssignmentRequests)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<MachineUsageLog>(entity =>
        {
            entity.HasIndex(e => e.LogDate, "IX_MachineUsageLogs_LogDate");

            entity.HasIndex(e => e.MachineId, "IX_MachineUsageLogs_MachineId");

            entity.HasIndex(e => new { e.MachineId, e.LogDate }, "IX_MachineUsageLogs_MachineId_LogDate");

            entity.HasIndex(e => e.OperatorId, "IX_MachineUsageLogs_OperatorId");

            entity.HasIndex(e => e.Status, "IX_MachineUsageLogs_Status");

            entity.Property(e => e.BreakdownDescription).HasMaxLength(1000);
            entity.Property(e => e.DowntimeHours).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DrifterHourEnd).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DrifterHourStart).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DrifterHoursDelta).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EngineHourEnd).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EngineHourStart).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EngineHoursDelta).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FuelConsumed).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IdleHours).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Remarks).HasMaxLength(2000);
            entity.Property(e => e.SiteEngineer).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.WorkingHours).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Machine).WithMany(p => p.MachineUsageLogs)
                .HasForeignKey(d => d.MachineId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Operator).WithMany(p => p.MachineUsageLogs)
                .HasForeignKey(d => d.OperatorId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<MaintenanceJob>(entity =>
        {
            entity.HasIndex(e => e.CreatedBy, "IX_MaintenanceJobs_CreatedBy");

            entity.HasIndex(e => e.MachineId, "IX_MaintenanceJobs_MachineId");

            entity.HasIndex(e => e.MaintenanceReportId, "IX_MaintenanceJobs_MaintenanceReportId")
                .IsUnique()
                .HasFilter("([MaintenanceReportId] IS NOT NULL)");

            entity.HasIndex(e => e.ProjectId, "IX_MaintenanceJobs_ProjectId");

            entity.HasIndex(e => e.ScheduledDate, "IX_MaintenanceJobs_ScheduledDate");

            entity.HasIndex(e => e.Status, "IX_MaintenanceJobs_Status");

            entity.HasIndex(e => e.Type, "IX_MaintenanceJobs_Type");

            entity.Property(e => e.ActualHours).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.EstimatedHours).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Reason).HasMaxLength(2000);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.Type).HasMaxLength(20);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MaintenanceJobs)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Machine).WithMany(p => p.MaintenanceJobs)
                .HasForeignKey(d => d.MachineId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.MaintenanceReport).WithOne(p => p.MaintenanceJob)
                .HasForeignKey<MaintenanceJob>(d => d.MaintenanceReportId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.Project).WithMany(p => p.MaintenanceJobs)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<MaintenanceJobAssignment>(entity =>
        {
            entity.HasIndex(e => e.MaintenanceJobId, "IX_MaintenanceJobAssignments_MaintenanceJobId");

            entity.HasIndex(e => new { e.MaintenanceJobId, e.MechanicalEngineerId }, "IX_MaintenanceJobAssignments_MaintenanceJobId_MechanicalEngineerId").IsUnique();

            entity.HasIndex(e => e.MechanicalEngineerId, "IX_MaintenanceJobAssignments_MechanicalEngineerId");

            entity.HasOne(d => d.MaintenanceJob).WithMany(p => p.MaintenanceJobAssignments).HasForeignKey(d => d.MaintenanceJobId);

            entity.HasOne(d => d.MechanicalEngineer).WithMany(p => p.MaintenanceJobAssignments)
                .HasForeignKey(d => d.MechanicalEngineerId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<MaintenanceReport>(entity =>
        {
            entity.HasIndex(e => e.MachineId, "IX_MaintenanceReports_MachineId");

            entity.HasIndex(e => e.MechanicalEngineerId, "IX_MaintenanceReports_MechanicalEngineerId");

            entity.HasIndex(e => e.OperatorId, "IX_MaintenanceReports_OperatorId");

            entity.HasIndex(e => e.ReportedAt, "IX_MaintenanceReports_ReportedAt");

            entity.HasIndex(e => e.Severity, "IX_MaintenanceReports_Severity");

            entity.HasIndex(e => e.Status, "IX_MaintenanceReports_Status");

            entity.HasIndex(e => e.TicketId, "IX_MaintenanceReports_TicketId").IsUnique();

            entity.Property(e => e.AffectedPart).HasMaxLength(50);
            entity.Property(e => e.CustomDescription).HasMaxLength(2000);
            entity.Property(e => e.ErrorCodes).HasMaxLength(500);
            entity.Property(e => e.EstimatedResponseTime).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(500);
            entity.Property(e => e.MachineModel).HasMaxLength(200);
            entity.Property(e => e.MachineName).HasMaxLength(200);
            entity.Property(e => e.MechanicalEngineerName).HasMaxLength(200);
            entity.Property(e => e.ProblemCategory).HasMaxLength(50);
            entity.Property(e => e.SerialNumber).HasMaxLength(100);
            entity.Property(e => e.Severity).HasMaxLength(20);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.TicketId).HasMaxLength(50);

            entity.HasOne(d => d.Machine).WithMany(p => p.MaintenanceReports)
                .HasForeignKey(d => d.MachineId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.MechanicalEngineer).WithMany(p => p.MaintenanceReportMechanicalEngineers)
                .HasForeignKey(d => d.MechanicalEngineerId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.Operator).WithMany(p => p.MaintenanceReportOperators)
                .HasForeignKey(d => d.OperatorId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasIndex(e => e.CreatedAt, "IX_Notifications_CreatedAt").IsDescending();

            entity.HasIndex(e => new { e.RelatedEntityType, e.RelatedEntityId }, "IX_Notifications_RelatedEntity");

            entity.HasIndex(e => e.Type, "IX_Notifications_Type");

            entity.HasIndex(e => new { e.UserId, e.CreatedAt }, "IX_Notifications_UserId_CreatedAt").IsDescending();

            entity.HasIndex(e => new { e.UserId, e.IsRead }, "IX_Notifications_UserId_IsRead");

            entity.Property(e => e.ActionUrl).HasMaxLength(500);
            entity.Property(e => e.Message).HasMaxLength(1000);
            entity.Property(e => e.RelatedEntityType).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.User).WithMany(p => p.Notifications).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<PasswordResetCode>(entity =>
        {
            entity.HasIndex(e => e.Email, "IX_PasswordResetCodes_Email");

            entity.HasIndex(e => e.UserId, "IX_PasswordResetCodes_UserId");

            entity.Property(e => e.Code).HasMaxLength(10);
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasDefaultValue("");

            entity.HasOne(d => d.User).WithMany(p => p.PasswordResetCodes).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<PatternSetting>(entity =>
        {
            entity.HasIndex(e => e.ProjectId, "IX_PatternSettings_ProjectId");

            entity.HasIndex(e => e.SiteId, "IX_PatternSettings_SiteId");

            entity.Property(e => e.Burden).HasDefaultValue(2.5f);
            entity.Property(e => e.Depth).HasDefaultValue(10f);
            entity.Property(e => e.Diameter).HasDefaultValue(0.89f);
            entity.Property(e => e.Spacing).HasDefaultValue(3f);
            entity.Property(e => e.Stemming).HasDefaultValue(3f);

            entity.HasOne(d => d.Project).WithMany(p => p.PatternSettings)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Site).WithMany(p => p.PatternSettings).HasForeignKey(d => d.SiteId);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasIndex(e => new { e.Module, e.Action }, "IX_Permissions_Module_Action").IsUnique();

            entity.Property(e => e.Action).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Module).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasIndex(e => e.AssignedUserId, "IX_Projects_AssignedUserId")
                .IsUnique()
                .HasFilter("([AssignedUserId] IS NOT NULL)");

            entity.HasIndex(e => e.Name, "IX_Projects_Name");

            entity.HasIndex(e => e.RegionId, "IX_Projects_RegionId");

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Region)
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.AssignedUser).WithOne(p => p.Project)
                .HasForeignKey<Project>(d => d.AssignedUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.RegionNavigation).WithMany(p => p.Projects)
                .HasForeignKey(d => d.RegionId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<ProjectSite>(entity =>
        {
            entity.HasIndex(e => e.CompletedByUserId, "IX_ProjectSites_CompletedByUserId");

            entity.HasIndex(e => e.ProjectId, "IX_ProjectSites_ProjectId");

            entity.Property(e => e.Coordinates).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.CompletedByUser).WithMany(p => p.ProjectSites).HasForeignKey(d => d.CompletedByUserId);

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectSites).HasForeignKey(d => d.ProjectId);
        });

        modelBuilder.Entity<QualityCheckRecord>(entity =>
        {
            entity.HasIndex(e => e.CentralWarehouseInventoryId, "IX_QualityCheckRecords_CentralWarehouseInventoryId");

            entity.HasIndex(e => e.CheckDate, "IX_QualityCheckRecords_CheckDate");

            entity.HasIndex(e => e.CheckType, "IX_QualityCheckRecords_CheckType");

            entity.HasIndex(e => e.CheckedByUserId, "IX_QualityCheckRecords_CheckedByUserId");

            entity.HasIndex(e => e.RequiresFollowUp, "IX_QualityCheckRecords_RequiresFollowUp");

            entity.HasIndex(e => e.Status, "IX_QualityCheckRecords_Status");

            entity.Property(e => e.ActionTaken).HasMaxLength(2000);
            entity.Property(e => e.CheckType).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Findings).HasMaxLength(2000);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.CentralWarehouseInventory).WithMany(p => p.QualityCheckRecords).HasForeignKey(d => d.CentralWarehouseInventoryId);

            entity.HasOne(d => d.CheckedByUser).WithMany(p => p.QualityCheckRecords)
                .HasForeignKey(d => d.CheckedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasDefaultValue("");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasIndex(e => e.Name, "IX_Roles_Name").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.NormalizedName).HasMaxLength(100);
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasIndex(e => e.PermissionId, "IX_RolePermissions_PermissionId");

            entity.HasIndex(e => new { e.RoleId, e.PermissionId }, "IX_RolePermissions_RoleId_PermissionId").IsUnique();

            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions).HasForeignKey(d => d.PermissionId);

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<SiteBlastingDatum>(entity =>
        {
            entity.HasIndex(e => e.CreatedByUserId, "IX_SiteBlastingData_CreatedByUserId");

            entity.HasIndex(e => new { e.ProjectId, e.SiteId, e.DataType }, "IX_SiteBlastingData_ProjectId_SiteId_DataType").IsUnique();

            entity.HasIndex(e => e.SiteId, "IX_SiteBlastingData_SiteId");

            entity.Property(e => e.DataType).HasMaxLength(50);

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.SiteBlastingData)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Project).WithMany(p => p.SiteBlastingData)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Site).WithMany(p => p.SiteBlastingData).HasForeignKey(d => d.SiteId);
        });

        modelBuilder.Entity<StockMovement>(entity =>
        {
            entity.HasIndex(e => e.BatchNumber, "IX_StockMovements_BatchNumber");

            entity.HasIndex(e => e.ExplosiveType, "IX_StockMovements_ExplosiveType");

            entity.HasIndex(e => e.MovementDate, "IX_StockMovements_MovementDate");

            entity.HasIndex(e => e.MovementNumber, "IX_StockMovements_MovementNumber").IsUnique();

            entity.HasIndex(e => e.MovementType, "IX_StockMovements_MovementType");

            entity.HasIndex(e => e.ProcessedById, "IX_StockMovements_ProcessedById");

            entity.HasIndex(e => e.ReferenceNumber, "IX_StockMovements_ReferenceNumber");

            entity.HasIndex(e => e.RelatedStoreId, "IX_StockMovements_RelatedStoreId");

            entity.HasIndex(e => e.StockRequestId, "IX_StockMovements_StockRequestId");

            entity.HasIndex(e => e.StockRequestItemId, "IX_StockMovements_StockRequestItemId");

            entity.HasIndex(e => e.StoreId, "IX_StockMovements_StoreId");

            entity.HasIndex(e => e.StoreInventoryRecordId, "IX_StockMovements_StoreInventoryRecordId");

            entity.Property(e => e.BalanceAfter).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BalanceBefore).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BatchNumber).HasMaxLength(50);
            entity.Property(e => e.MovementNumber).HasMaxLength(50);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Reason).HasMaxLength(500);
            entity.Property(e => e.ReferenceNumber).HasMaxLength(100);
            entity.Property(e => e.Supplier).HasMaxLength(100);
            entity.Property(e => e.TotalValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Unit).HasMaxLength(20);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.ProcessedBy).WithMany(p => p.StockMovements)
                .HasForeignKey(d => d.ProcessedById)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.StockRequest).WithMany(p => p.StockMovements).HasForeignKey(d => d.StockRequestId);

            entity.HasOne(d => d.StockRequestItem).WithMany(p => p.StockMovements).HasForeignKey(d => d.StockRequestItemId);

            entity.HasOne(d => d.StoreInventoryRecord).WithMany(p => p.StockMovements).HasForeignKey(d => d.StoreInventoryRecordId);
        });

        modelBuilder.Entity<StockRequest>(entity =>
        {
            entity.HasIndex(e => e.ApprovedById, "IX_StockRequests_ApprovedById");

            entity.HasIndex(e => e.RejectedById, "IX_StockRequests_RejectedById");

            entity.HasIndex(e => e.RequestDate, "IX_StockRequests_RequestDate");

            entity.HasIndex(e => e.RequestNumber, "IX_StockRequests_RequestNumber").IsUnique();

            entity.HasIndex(e => e.RequesterId, "IX_StockRequests_RequesterId");

            entity.HasIndex(e => e.RequesterStoreId, "IX_StockRequests_RequesterStoreId");

            entity.HasIndex(e => e.RequiredDate, "IX_StockRequests_RequiredDate");

            entity.HasIndex(e => e.Status, "IX_StockRequests_Status");

            entity.HasIndex(e => e.SupplierStoreId, "IX_StockRequests_SupplierStoreId");

            entity.Property(e => e.ApprovalNotes).HasMaxLength(500);
            entity.Property(e => e.Justification).HasMaxLength(1000);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.Priority)
                .HasMaxLength(100)
                .HasDefaultValue("Normal");
            entity.Property(e => e.Purpose).HasMaxLength(500);
            entity.Property(e => e.RejectionReason).HasMaxLength(500);
            entity.Property(e => e.RequestNumber).HasMaxLength(50);

            entity.HasOne(d => d.ApprovedBy).WithMany(p => p.StockRequestApprovedBies).HasForeignKey(d => d.ApprovedById);

            entity.HasOne(d => d.RejectedBy).WithMany(p => p.StockRequestRejectedBies).HasForeignKey(d => d.RejectedById);

            entity.HasOne(d => d.Requester).WithMany(p => p.StockRequestRequesters)
                .HasForeignKey(d => d.RequesterId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<StockRequestItem>(entity =>
        {
            entity.HasIndex(e => e.PreferredBatchNumber, "IX_StockRequestItems_PreferredBatchNumber");

            entity.HasIndex(e => new { e.StockRequestId, e.ExplosiveType }, "IX_StockRequestItems_StockRequestId_ExplosiveType");

            entity.Property(e => e.ActualTotalCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ActualUnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ApprovedQuantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EstimatedTotalCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EstimatedUnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FulfilledQuantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.PreferredBatchNumber).HasMaxLength(50);
            entity.Property(e => e.RequestedQuantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Specifications).HasMaxLength(500);
            entity.Property(e => e.Unit).HasMaxLength(20);

            entity.HasOne(d => d.StockRequest).WithMany(p => p.StockRequestItems).HasForeignKey(d => d.StockRequestId);
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasIndex(e => e.ManagerUserId, "IX_Stores_ManagerUserId");

            entity.HasIndex(e => e.ProjectId, "IX_Stores_ProjectId");

            entity.HasIndex(e => e.RegionId, "IX_Stores_RegionId");

            entity.HasIndex(e => e.Status, "IX_Stores_Status");

            entity.HasIndex(e => e.StoreName, "IX_Stores_StoreName");

            entity.HasIndex(e => e.UserId, "IX_Stores_UserId");

            entity.Property(e => e.AllowedExplosiveTypes)
                .HasMaxLength(200)
                .HasDefaultValue("");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.StorageCapacity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StoreAddress).HasMaxLength(500);
            entity.Property(e => e.StoreName).HasMaxLength(200);

            entity.HasOne(d => d.ManagerUser).WithMany(p => p.StoreManagerUsers)
                .HasForeignKey(d => d.ManagerUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.Project).WithMany(p => p.Stores).HasForeignKey(d => d.ProjectId);

            entity.HasOne(d => d.Region).WithMany(p => p.Stores)
                .HasForeignKey(d => d.RegionId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.StoreUsers).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<StoreInventory>(entity =>
        {
            entity.HasIndex(e => e.BatchNumber, "IX_StoreInventories_BatchNumber");

            entity.HasIndex(e => e.ExpiryDate, "IX_StoreInventories_ExpiryDate");

            entity.HasIndex(e => e.ExplosiveType, "IX_StoreInventories_ExplosiveType");

            entity.HasIndex(e => e.StoreId, "IX_StoreInventories_StoreId");

            entity.HasIndex(e => new { e.StoreId, e.ExplosiveType }, "IX_StoreInventories_StoreId_ExplosiveType").IsUnique();

            entity.Property(e => e.BatchNumber).HasMaxLength(100);
            entity.Property(e => e.ExplosiveType).HasMaxLength(50);
            entity.Property(e => e.MaximumStockLevel).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinimumStockLevel).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ReservedQuantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Supplier).HasMaxLength(200);
            entity.Property(e => e.Unit).HasMaxLength(20);

            entity.HasOne(d => d.Store).WithMany(p => p.StoreInventories).HasForeignKey(d => d.StoreId);
        });

        modelBuilder.Entity<StoreInventoryRecord>(entity =>
        {
            entity.HasIndex(e => e.BatchNumber, "IX_StoreInventoryRecords_BatchNumber");

            entity.HasIndex(e => e.ExpiryDate, "IX_StoreInventoryRecords_ExpiryDate");

            entity.HasIndex(e => new { e.StoreId, e.ExplosiveType }, "IX_StoreInventoryRecords_StoreId_ExplosiveType");

            entity.Property(e => e.BatchNumber).HasMaxLength(50);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StorageLocation).HasMaxLength(100);
            entity.Property(e => e.Supplier).HasMaxLength(100);
            entity.Property(e => e.TotalValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Unit).HasMaxLength(20);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<StoreTransaction>(entity =>
        {
            entity.HasIndex(e => e.ExplosiveType, "IX_StoreTransactions_ExplosiveType");

            entity.HasIndex(e => e.ProcessedByUserId, "IX_StoreTransactions_ProcessedByUserId");

            entity.HasIndex(e => e.ReferenceNumber, "IX_StoreTransactions_ReferenceNumber");

            entity.HasIndex(e => e.RelatedStoreId, "IX_StoreTransactions_RelatedStoreId");

            entity.HasIndex(e => e.StoreId, "IX_StoreTransactions_StoreId");

            entity.HasIndex(e => e.StoreInventoryId, "IX_StoreTransactions_StoreInventoryId");

            entity.HasIndex(e => e.TransactionDate, "IX_StoreTransactions_TransactionDate");

            entity.HasIndex(e => e.TransactionType, "IX_StoreTransactions_TransactionType");

            entity.Property(e => e.ExplosiveType).HasMaxLength(50);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ReferenceNumber).HasMaxLength(100);
            entity.Property(e => e.TransactionDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.TransactionType).HasMaxLength(20);
            entity.Property(e => e.Unit).HasMaxLength(20);

            entity.HasOne(d => d.ProcessedByUser).WithMany(p => p.StoreTransactions)
                .HasForeignKey(d => d.ProcessedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.RelatedStore).WithMany(p => p.StoreTransactionRelatedStores)
                .HasForeignKey(d => d.RelatedStoreId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(d => d.Store).WithMany(p => p.StoreTransactionStores)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.StoreInventory).WithMany(p => p.StoreTransactions).HasForeignKey(d => d.StoreInventoryId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email, "IX_Users_Email").IsUnique();

            entity.HasIndex(e => e.Name, "IX_Users_Name").IsUnique();

            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.CountryPhone).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.OmanPhone).HasMaxLength(20);
            entity.Property(e => e.PasswordHash).HasDefaultValue("");
            entity.Property(e => e.Region).HasMaxLength(100);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(20);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_UserRoles_RoleId");

            entity.HasIndex(e => new { e.UserId, e.RoleId }, "IX_UserRoles_UserId_RoleId").IsUnique();

            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles).HasForeignKey(d => d.RoleId);

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles).HasForeignKey(d => d.UserId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
