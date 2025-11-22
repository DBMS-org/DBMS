using Domain.Entities.MachineManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations.MachineManagement
{
    public class MachineConfiguration : IEntityTypeConfiguration<Machine>
    {
        public void Configure(EntityTypeBuilder<Machine> builder)
        {
            builder.ToTable("Machines");

            builder.HasKey(m => m.Id);

            // Service tracking properties with decimal precision
            builder.Property(m => m.EngineServiceInterval)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(m => m.CurrentEngineServiceHours)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(m => m.DrifterServiceInterval)
                .HasColumnType("decimal(18,2)");

            builder.Property(m => m.CurrentDrifterServiceHours)
                .HasColumnType("decimal(18,2)");

            // Ignore computed properties (not mapped to database)
            builder.Ignore(m => m.ProjectName);
            builder.Ignore(m => m.OperatorName);
            builder.Ignore(m => m.RegionName);

            // Relationships
            builder.HasOne(m => m.Project)
                .WithMany(p => p.Machines)
                .HasForeignKey(m => m.ProjectId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(m => m.Operator)
                .WithMany()
                .HasForeignKey(m => m.OperatorId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(m => m.Region)
                .WithMany(r => r.Machines)
                .HasForeignKey(m => m.RegionId)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(m => m.OperatorId);
            builder.HasIndex(m => m.ProjectId);
            builder.HasIndex(m => m.RegionId);
            builder.HasIndex(m => m.Status);
        }
    }
}
