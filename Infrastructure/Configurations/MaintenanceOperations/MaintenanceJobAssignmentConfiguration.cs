using Domain.Entities.MaintenanceOperations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.MaintenanceOperations
{
    public class MaintenanceJobAssignmentConfiguration : IEntityTypeConfiguration<MaintenanceJobAssignment>
    {
        public void Configure(EntityTypeBuilder<MaintenanceJobAssignment> entity)
        {
            entity.HasKey(e => e.Id);

            // Required fields
            entity.Property(e => e.MaintenanceJobId).IsRequired();
            entity.Property(e => e.MechanicalEngineerId).IsRequired();
            entity.Property(e => e.AssignedAt).IsRequired();

            // Relationships
            entity.HasOne(e => e.MaintenanceJob)
                .WithMany(j => j.Assignments)
                .HasForeignKey(e => e.MaintenanceJobId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.MechanicalEngineer)
                .WithMany()
                .HasForeignKey(e => e.MechanicalEngineerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique constraint to prevent duplicate assignments
            entity.HasIndex(e => new { e.MaintenanceJobId, e.MechanicalEngineerId }).IsUnique();

            // Indexes for performance
            entity.HasIndex(e => e.MechanicalEngineerId);
            entity.HasIndex(e => e.MaintenanceJobId);
        }
    }
}
