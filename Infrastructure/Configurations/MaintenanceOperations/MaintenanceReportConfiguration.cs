using Domain.Entities.MaintenanceOperations;
using Domain.Entities.MaintenanceOperations.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.MaintenanceOperations
{
    public class MaintenanceReportConfiguration : IEntityTypeConfiguration<MaintenanceReport>
    {
        public void Configure(EntityTypeBuilder<MaintenanceReport> entity)
        {
            entity.HasKey(e => e.Id);

            // Unique ticket ID
            entity.Property(e => e.TicketId)
                .IsRequired()
                .HasMaxLength(50);
            entity.HasIndex(e => e.TicketId).IsUnique();

            // Required fields
            entity.Property(e => e.OperatorId).IsRequired();
            entity.Property(e => e.MachineId).IsRequired();
            entity.Property(e => e.MachineName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.MachineModel).HasMaxLength(200);
            entity.Property(e => e.SerialNumber).HasMaxLength(100);
            entity.Property(e => e.Location).HasMaxLength(500);

            // Problem details
            entity.Property(e => e.AffectedPart)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.ProblemCategory)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.CustomDescription)
                .IsRequired()
                .HasMaxLength(2000);

            // Diagnostic information (stored as JSON)
            entity.Property(e => e.Symptoms).HasColumnType("nvarchar(max)");
            entity.Property(e => e.ErrorCodes).HasMaxLength(500);
            entity.Property(e => e.RecentMaintenanceHistory).HasColumnType("nvarchar(max)");

            // Severity and Status
            entity.Property(e => e.Severity)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Status)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);

            // Timestamps
            entity.Property(e => e.ReportedAt).IsRequired();
            entity.Property(e => e.AcknowledgedAt);
            entity.Property(e => e.InProgressAt);
            entity.Property(e => e.ResolvedAt);
            entity.Property(e => e.ClosedAt);

            // Mechanical engineer information
            entity.Property(e => e.MechanicalEngineerId);
            entity.Property(e => e.MechanicalEngineerName).HasMaxLength(200);
            entity.Property(e => e.ResolutionNotes).HasColumnType("nvarchar(max)");
            entity.Property(e => e.EstimatedResponseTime).HasMaxLength(50);

            // Relationships
            entity.HasOne(e => e.Operator)
                .WithMany()
                .HasForeignKey(e => e.OperatorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Machine)
                .WithMany()
                .HasForeignKey(e => e.MachineId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.MechanicalEngineer)
                .WithMany()
                .HasForeignKey(e => e.MechanicalEngineerId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.MaintenanceJob)
                .WithOne(j => j.MaintenanceReport)
                .HasForeignKey<MaintenanceJob>(j => j.MaintenanceReportId)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes for performance
            entity.HasIndex(e => e.OperatorId);
            entity.HasIndex(e => e.MachineId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.Severity);
            entity.HasIndex(e => e.ReportedAt);
            entity.HasIndex(e => e.MechanicalEngineerId);
        }
    }
}
