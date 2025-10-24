using Domain.Entities.MachineManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.MachineManagement
{
    public class MachineAssignmentConfiguration : IEntityTypeConfiguration<MachineAssignment>
    {
        public void Configure(EntityTypeBuilder<MachineAssignment> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.AssignedBy)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.AssignedDate)
                  .IsRequired();

            entity.Property(e => e.Status)
                  .HasConversion<string>()
                  .IsRequired()
                  .HasMaxLength(20);

            entity.Property(e => e.Location)
                  .HasMaxLength(200);

            entity.Property(e => e.Notes)
                  .HasMaxLength(500);

            // Relationships
            entity.HasOne(e => e.Machine)
                  .WithMany()
                  .HasForeignKey(e => e.MachineId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Project)
                  .WithMany()
                  .HasForeignKey(e => e.ProjectId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Operator)
                  .WithMany()
                  .HasForeignKey(e => e.OperatorId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.MachineId);
            entity.HasIndex(e => e.ProjectId);
            entity.HasIndex(e => e.OperatorId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.AssignedDate);
        }
    }
}
