using Domain.Entities.ExplosiveInventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.ExplosiveInventory
{
    /// <summary>
    /// EF Core configuration for InventoryTransferRequest entity
    /// </summary>
    public class InventoryTransferRequestConfiguration : IEntityTypeConfiguration<InventoryTransferRequest>
    {
        public void Configure(EntityTypeBuilder<InventoryTransferRequest> builder)
        {
            builder.ToTable("InventoryTransferRequests");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.RequestNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(x => x.RequestNumber)
                .IsUnique();

            // Source & Destination
            builder.Property(x => x.CentralWarehouseInventoryId)
                .IsRequired();

            builder.Property(x => x.DestinationStoreId)
                .IsRequired();

            // Quantity
            builder.Property(x => x.RequestedQuantity)
                .IsRequired()
                .HasPrecision(18, 3);

            builder.Property(x => x.ApprovedQuantity)
                .HasPrecision(18, 3);

            builder.Property(x => x.Unit)
                .IsRequired()
                .HasMaxLength(10);

            // Status & Workflow
            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.RequestDate)
                .IsRequired();

            builder.Property(x => x.ApprovedDate);

            builder.Property(x => x.CompletedDate);

            builder.Property(x => x.RequiredByDate);

            // People
            builder.Property(x => x.RequestedByUserId)
                .IsRequired();

            builder.Property(x => x.ApprovedByUserId);

            builder.Property(x => x.ProcessedByUserId);

            // Dispatch/Delivery Tracking
            builder.Property(x => x.DispatchDate);

            builder.Property(x => x.TruckNumber)
                .HasMaxLength(50);

            builder.Property(x => x.DriverName)
                .HasMaxLength(200);

            builder.Property(x => x.DriverContactNumber)
                .HasMaxLength(20);

            builder.Property(x => x.DispatchNotes)
                .HasMaxLength(1000);

            builder.Property(x => x.DispatchedByUserId);

            builder.Property(x => x.DeliveryConfirmedDate);

            // Notes
            builder.Property(x => x.RequestNotes)
                .HasMaxLength(1000);

            builder.Property(x => x.ApprovalNotes)
                .HasMaxLength(1000);

            builder.Property(x => x.RejectionReason)
                .HasMaxLength(1000);

            // Transaction Reference
            builder.Property(x => x.CompletedTransactionId);

            // Audit fields
            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Relationships
            builder.HasOne(x => x.CentralInventory)
                .WithMany(x => x.TransferRequests)
                .HasForeignKey(x => x.CentralWarehouseInventoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.DestinationStore)
                .WithMany()
                .HasForeignKey(x => x.DestinationStoreId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.RequestedByUser)
                .WithMany()
                .HasForeignKey(x => x.RequestedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ApprovedByUser)
                .WithMany()
                .HasForeignKey(x => x.ApprovedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ProcessedByUser)
                .WithMany()
                .HasForeignKey(x => x.ProcessedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.DispatchedByUser)
                .WithMany()
                .HasForeignKey(x => x.DispatchedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CompletedTransaction)
                .WithMany()
                .HasForeignKey(x => x.CompletedTransactionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.RequestDate);
            builder.HasIndex(x => x.RequiredByDate);
            builder.HasIndex(x => x.DestinationStoreId);
            builder.HasIndex(x => x.RequestedByUserId);
            builder.HasIndex(x => x.DispatchDate);
        }
    }
}
