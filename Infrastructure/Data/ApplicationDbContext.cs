using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets for all entities
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<DrillHole> DrillHoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Area).HasMaxLength(100);
                entity.Property(e => e.Region).HasMaxLength(100);
                entity.Property(e => e.Country).HasMaxLength(100);
                entity.Property(e => e.OmanPhone).HasMaxLength(20);
                entity.Property(e => e.CountryPhone).HasMaxLength(20);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // Configure Role entity
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.NormalizedName).HasMaxLength(100);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // Configure Permission entity
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.Module).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Action).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => new { e.Module, e.Action }).IsUnique();
            });

            // Configure UserRole entity (Many-to-Many)
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.User)
                      .WithMany(e => e.UserRoles)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Role)
                      .WithMany(e => e.UserRoles)
                      .HasForeignKey(e => e.RoleId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure RolePermission entity (Many-to-Many)
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Role)
                      .WithMany(e => e.RolePermissions)
                      .HasForeignKey(e => e.RoleId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(e => e.Permission)
                      .WithMany(e => e.RolePermissions)
                      .HasForeignKey(e => e.PermissionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure DrillHole entity
            modelBuilder.Entity<DrillHole>(entity =>
            {
                entity.HasKey(e => e.SerialNumber);
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Name).IsRequired();
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin", Description = "Administrator with full access", NormalizedName = "ADMIN", IsActive = true },
                new Role { Id = 2, Name = "BlastingEngineer", Description = "Blasting Engineer with technical access", NormalizedName = "BLASTINGENGINEER", IsActive = true },
                new Role { Id = 3, Name = "User", Description = "Regular user with limited access", NormalizedName = "USER", IsActive = true }
            );

            // Seed Permissions
            modelBuilder.Entity<Permission>().HasData(
                new Permission { Id = 1, Name = "View Users", Description = "Can view user list", Module = "Users", Action = "View", IsActive = true },
                new Permission { Id = 2, Name = "Create User", Description = "Can create new users", Module = "Users", Action = "Create", IsActive = true },
                new Permission { Id = 3, Name = "Edit User", Description = "Can edit user details", Module = "Users", Action = "Edit", IsActive = true },
                new Permission { Id = 4, Name = "Delete User", Description = "Can delete users", Module = "Users", Action = "Delete", IsActive = true },
                new Permission { Id = 5, Name = "View Dashboard", Description = "Can view dashboard", Module = "Dashboard", Action = "View", IsActive = true },
                new Permission { Id = 6, Name = "Upload CSV", Description = "Can upload CSV files", Module = "DrillHole", Action = "Upload", IsActive = true },
                new Permission { Id = 7, Name = "View DrillHoles", Description = "Can view drill holes", Module = "DrillHole", Action = "View", IsActive = true }
            );

            // Seed Admin User
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    Id = 1, 
                    Name = "System Administrator", 
                    Email = "admin@dbms.com",
                    PasswordHash = "$2a$11$K8QQfR6Z5j6XgkHjWo9xXeNqO7QDj9qQVvjBZjR8g1jzQzKL9Yd3W", // Password: "admin123"
                    Role = "Admin", 
                    Status = "Active",
                    Area = "Central",
                    Region = "Muscat",
                    Country = "Oman",
                    OmanPhone = "+968 9999 9999",
                    CountryPhone = "+968 9999 9999"
                }
            );
        }
    }
} 