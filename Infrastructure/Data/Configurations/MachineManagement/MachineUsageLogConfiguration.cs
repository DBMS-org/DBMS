using Domain.Entities.MachineManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations.MachineManagement
{
    public class MachineUsageLogConfiguration : IEntityTypeConfiguration<MachineUsageLog>
    {
        public void Configure(EntityTypeBuilder<MachineUsageLog> builder)
        {
            builder.ToTable("MachineUsageLogs");

            builder.HasKey(l => l.Id);

            // Relationships
            builder.HasOne(l => l.Machine)
                .WithMany()
                .HasForeignKey(l => l.MachineId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.Operator)
                .WithMany()
                .HasForeignKey(l => l.OperatorId)
                .OnDelete(DeleteBehavior.SetNull);

            // Properties with precision
            builder.Property(l => l.EngineHourStart)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(l => l.EngineHourEnd)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(l => l.EngineHoursDelta)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(l => l.DrifterHourStart)
                .HasColumnType("decimal(18,2)");

            builder.Property(l => l.DrifterHourEnd)
                .HasColumnType("decimal(18,2)");

            builder.Property(l => l.DrifterHoursDelta)
                .HasColumnType("decimal(18,2)");

            builder.Property(l => l.IdleHours)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(l => l.WorkingHours)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(l => l.FuelConsumed)
                .HasColumnType("decimal(18,2)");

            builder.Property(l => l.DowntimeHours)
                .HasColumnType("decimal(18,2)");

            // String properties
            builder.Property(l => l.SiteEngineer)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(l => l.Status)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(l => l.BreakdownDescription)
                .HasMaxLength(1000);

            builder.Property(l => l.Remarks)
                .HasMaxLength(2000);

            builder.Property(l => l.LogDate)
                .IsRequired();

            builder.Property(l => l.HasDowntime)
                .IsRequired();

            builder.Property(l => l.CreatedBy)
                .IsRequired();

            // Indexes for performance
            builder.HasIndex(l => l.MachineId);
            builder.HasIndex(l => l.OperatorId);
            builder.HasIndex(l => l.LogDate);
            builder.HasIndex(l => l.Status);
            builder.HasIndex(l => new { l.MachineId, l.LogDate });
        }
    }
}
