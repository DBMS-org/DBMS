using Domain.Entities.MachineManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.MachineManagement
{
    public class MachineAssignmentRequestConfiguration : IEntityTypeConfiguration<MachineAssignmentRequest>
    {
        public void Configure(EntityTypeBuilder<MachineAssignmentRequest> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.MachineType)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(e => e.Quantity)
                  .IsRequired();

            entity.Property(e => e.RequestedBy)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.RequestedDate)
                  .IsRequired();

            entity.Property(e => e.Status)
                  .HasConversion<string>()
                  .IsRequired()
                  .HasMaxLength(30);

            entity.Property(e => e.Urgency)
                  .HasConversion<string>()
                  .IsRequired()
                  .HasMaxLength(20);

            entity.Property(e => e.DetailsOrExplanation)
                  .HasMaxLength(1000);

            entity.Property(e => e.ApprovedBy)
                  .HasMaxLength(100);

            entity.Property(e => e.AssignedMachinesJson)
                  .HasColumnType("nvarchar(max)");

            entity.Property(e => e.Comments)
                  .HasMaxLength(500);

            entity.Property(e => e.ExpectedUsageDuration)
                  .HasMaxLength(100);

            // Relationships
            entity.HasOne(e => e.Project)
                  .WithMany()
                  .HasForeignKey(e => e.ProjectId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.ProjectId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.Urgency);
            entity.HasIndex(e => e.RequestedDate);
        }
    }
}
