using Domain.Entities.StoreManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.StoreManagement
{
    public class StoreConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> entity)
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.StoreName)
                  .IsRequired()
                  .HasMaxLength(200);
            
            entity.Property(e => e.StoreAddress)
                  .IsRequired()
                  .HasMaxLength(500);
            
            entity.Property(e => e.StorageCapacity)
                  .IsRequired()
                  .HasColumnType("decimal(18,2)");

            // CurrentOccupancy is now a computed property - ignore it in EF Core mapping
            entity.Ignore(e => e.CurrentOccupancy);

            entity.Property(e => e.AllowedExplosiveTypes)
                  .HasMaxLength(200);

            entity.Property(e => e.Status)
                  .HasConversion<string>()
                  .IsRequired()
                  .HasMaxLength(20);

            // Relationships
            entity.HasOne(e => e.Region)
                  .WithMany(r => r.Stores)
                  .HasForeignKey(e => e.RegionId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ManagerUser)
                  .WithMany()
                  .HasForeignKey(e => e.ManagerUserId)
                  .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            entity.HasIndex(e => e.StoreName);
            entity.HasIndex(e => e.RegionId);
            entity.HasIndex(e => e.ManagerUserId);
            entity.HasIndex(e => e.Status);
        }
    }
}