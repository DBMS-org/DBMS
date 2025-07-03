using Domain.Common;
using Domain.Entities.MachineManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.MachineManagement
{
    public class MachineConfiguration : IEntityTypeConfiguration<Machine>
    {
        public void Configure(EntityTypeBuilder<Machine> entity)
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Status)
                  .HasConversion<string>()
                  .IsRequired()
                  .HasMaxLength(20);
            entity.Property(e => e.SerialNumber).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Model).HasMaxLength(100);
            entity.Property(e => e.Manufacturer).HasMaxLength(100);
            entity.Property(e => e.RigNo).HasMaxLength(50);
            entity.Property(e => e.PlateNo).HasMaxLength(50);
            entity.Property(e => e.ChassisDetails).HasMaxLength(500);
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.CurrentLocation).HasMaxLength(200);
            entity.Property(e => e.AssignedToProject).HasMaxLength(100);
            entity.Property(e => e.AssignedToOperator).HasMaxLength(100);

            entity.HasOne(e => e.Region)
                  .WithMany(r => r.Machines)
                  .HasForeignKey(e => e.RegionId)
                  .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => e.RegionId);
            entity.HasIndex(e => e.Status);
        }
    }
} 