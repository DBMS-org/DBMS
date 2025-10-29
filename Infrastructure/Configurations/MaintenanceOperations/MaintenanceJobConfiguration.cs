using Domain.Entities.MaintenanceOperations;
using Domain.Entities.MaintenanceOperations.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.MaintenanceOperations
{
    public class MaintenanceJobConfiguration : IEntityTypeConfiguration<MaintenanceJob>
    {
        public void Configure(EntityTypeBuilder<MaintenanceJob> entity)
        {
            entity.HasKey(e => e.Id);

            // Required fields
            entity.Property(e => e.MachineId).IsRequired();
            entity.Property(e => e.ProjectId);
            entity.Property(e => e.MaintenanceReportId);

            // Job classification
            entity.Property(e => e.Type)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Status)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);

            // Scheduling
            entity.Property(e => e.ScheduledDate).IsRequired();
            entity.Property(e => e.CompletedDate);
            entity.Property(e => e.InProgressAt);

            // Time tracking
            entity.Property(e => e.EstimatedHours)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            entity.Property(e => e.ActualHours)
                .HasColumnType("decimal(5,2)");

            // Job details
            entity.Property(e => e.Reason)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(e => e.Observations)
                .HasColumnType("nvarchar(max)");

            // Parts replaced (stored as JSON)
            entity.Property(e => e.PartsReplaced)
                .HasColumnType("nvarchar(max)");

            // Creator
            entity.Property(e => e.CreatedBy).IsRequired();

            // Relationships
            entity.HasOne(e => e.Machine)
                .WithMany()
                .HasForeignKey(e => e.MachineId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Project)
                .WithMany()
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.MaintenanceReport)
                .WithOne(r => r.MaintenanceJob)
                .HasForeignKey<MaintenanceJob>(j => j.MaintenanceReportId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Creator)
                .WithMany()
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique constraint for one-to-one with MaintenanceReport
            entity.HasIndex(e => e.MaintenanceReportId).IsUnique();

            // Indexes for performance
            entity.HasIndex(e => e.MachineId);
            entity.HasIndex(e => e.ProjectId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.ScheduledDate);
            entity.HasIndex(e => e.Type);
        }
    }
}
