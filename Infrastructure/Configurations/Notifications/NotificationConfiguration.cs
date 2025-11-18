using Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Notifications
{
    /// <summary>
    /// Entity Framework configuration for Notification entity
    /// </summary>
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> entity)
        {
            // Primary key
            entity.HasKey(n => n.Id);

            // User relationship (cascade delete - when user is deleted, their notifications are deleted)
            entity.HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Property configurations
            entity.Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(n => n.Message)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(n => n.Type)
                .HasConversion<int>()
                .IsRequired();

            entity.Property(n => n.Priority)
                .HasConversion<int>()
                .IsRequired();

            entity.Property(n => n.IsRead)
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(n => n.RelatedEntityType)
                .HasMaxLength(100);

            entity.Property(n => n.ActionUrl)
                .HasMaxLength(500);

            // Indexes for performance
            // Most common query: get notifications by user and read status
            entity.HasIndex(n => new { n.UserId, n.IsRead })
                .HasDatabaseName("IX_Notifications_UserId_IsRead");

            // For sorting by creation date
            entity.HasIndex(n => n.CreatedAt)
                .HasDatabaseName("IX_Notifications_CreatedAt")
                .IsDescending();

            // For filtering by type
            entity.HasIndex(n => n.Type)
                .HasDatabaseName("IX_Notifications_Type");

            // Composite index for user notifications ordered by date
            entity.HasIndex(n => new { n.UserId, n.CreatedAt })
                .HasDatabaseName("IX_Notifications_UserId_CreatedAt")
                .IsDescending();

            // For finding notifications by related entity
            entity.HasIndex(n => new { n.RelatedEntityType, n.RelatedEntityId })
                .HasDatabaseName("IX_Notifications_RelatedEntity");
        }
    }
}
