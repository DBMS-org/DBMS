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
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectSite> ProjectSites { get; set; }
        public DbSet<SiteBlastingData> SiteBlastingData { get; set; }
        public DbSet<DrillPattern> DrillPatterns { get; set; }
        public DbSet<BlastSequence> BlastSequences { get; set; }
        public DbSet<PasswordResetCode> PasswordResetCodes { get; set; }

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
                entity.Property(e => e.SerialNumber).ValueGeneratedOnAdd();
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Name).IsRequired();
            });

            // Configure Project entity
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Region).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Description).HasMaxLength(1000);
                
                // Foreign key relationship with User
                entity.HasOne(e => e.AssignedUser)
                      .WithMany()
                      .HasForeignKey(e => e.AssignedUserId)
                      .OnDelete(DeleteBehavior.SetNull);
                      
                entity.HasIndex(e => e.Name);

                // Ensure an operator (AssignedUserId) can only be linked to a single project
                entity.HasIndex(e => e.AssignedUserId)
                      .IsUnique()
                      .HasFilter("[AssignedUserId] IS NOT NULL"); // Unique when not null
            });

            // Configure ProjectSite entity
            modelBuilder.Entity<ProjectSite>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Location).HasMaxLength(200);
                entity.Property(e => e.Coordinates).HasMaxLength(200);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Description).HasMaxLength(500);
                
                // Foreign key relationship with Project
                entity.HasOne(e => e.Project)
                      .WithMany(e => e.ProjectSites)
                      .HasForeignKey(e => e.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasIndex(e => e.ProjectId);
            });

            // Configure SiteBlastingData entity
            modelBuilder.Entity<SiteBlastingData>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DataType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.JsonData).IsRequired();
                
                // Foreign key relationships
                entity.HasOne(e => e.Project)
                      .WithMany()
                      .HasForeignKey(e => e.ProjectId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(e => e.Site)
                      .WithMany()
                      .HasForeignKey(e => e.SiteId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(e => e.CreatedBy)
                      .WithMany()
                      .HasForeignKey(e => e.CreatedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);
                
                // Indexes for performance
                entity.HasIndex(e => new { e.ProjectId, e.SiteId, e.DataType }).IsUnique();
                entity.HasIndex(e => e.ProjectId);
                entity.HasIndex(e => e.SiteId);
            });

            // Configure DrillPattern entity
            modelBuilder.Entity<DrillPattern>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.DrillPointsJson).IsRequired();
                
                // Foreign key relationships
                entity.HasOne(e => e.Project)
                      .WithMany()
                      .HasForeignKey(e => e.ProjectId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(e => e.Site)
                      .WithMany()
                      .HasForeignKey(e => e.SiteId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(e => e.CreatedBy)
                      .WithMany()
                      .HasForeignKey(e => e.CreatedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);
                
                // Indexes for performance
                entity.HasIndex(e => new { e.ProjectId, e.SiteId });
                entity.HasIndex(e => e.ProjectId);
                entity.HasIndex(e => e.SiteId);
                entity.HasIndex(e => e.IsActive);
            });

            // Configure BlastSequence entity
            modelBuilder.Entity<BlastSequence>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.ConnectionsJson).IsRequired();
                entity.Property(e => e.SimulationSettingsJson).HasDefaultValue("{}");
                
                // Foreign key relationships
                entity.HasOne(e => e.Project)
                      .WithMany()
                      .HasForeignKey(e => e.ProjectId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(e => e.Site)
                      .WithMany()
                      .HasForeignKey(e => e.SiteId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                entity.HasOne(e => e.DrillPattern)
                      .WithMany(e => e.BlastSequences)
                      .HasForeignKey(e => e.DrillPatternId)
                      .OnDelete(DeleteBehavior.Restrict);
                      
                entity.HasOne(e => e.CreatedBy)
                      .WithMany()
                      .HasForeignKey(e => e.CreatedByUserId)
                      .OnDelete(DeleteBehavior.Restrict);
                
                // Indexes for performance
                entity.HasIndex(e => new { e.ProjectId, e.SiteId });
                entity.HasIndex(e => e.DrillPatternId);
                entity.HasIndex(e => e.ProjectId);
                entity.HasIndex(e => e.SiteId);
                entity.HasIndex(e => e.IsActive);
            });

            // Configure PasswordResetCode entity
            modelBuilder.Entity<PasswordResetCode>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(6);
                entity.Property(e => e.ExpiresAt).IsRequired();
                entity.Property(e => e.IsUsed).HasDefaultValue(false);
                entity.Property(e => e.AttemptCount).HasDefaultValue(0);
                
                // Foreign key relationship with User
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                // Indexes for performance
                entity.HasIndex(e => new { e.UserId, e.Code });
                entity.HasIndex(e => e.ExpiresAt);
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
                new Role { Id = 4, Name = "Operator", Description = "Operator with operational access", NormalizedName = "OPERATOR", IsActive = true },
                new Role { Id = 5, Name = "MachineManager", Description = "Machine Manager with machinery operational access", NormalizedName = "MACHINEMANAGER", IsActive = true }
            );

            // Seed Permissions
            modelBuilder.Entity<Permission>().HasData(
                new Permission { Id = 1, Name = "View Users", Description = "Can view user list", Module = "Users", Action = "View", IsActive = true },
                new Permission { Id = 2, Name = "Create User", Description = "Can create new users", Module = "Users", Action = "Create", IsActive = true },
                new Permission { Id = 3, Name = "Edit User", Description = "Can edit user details", Module = "Users", Action = "Edit", IsActive = true },
                new Permission { Id = 4, Name = "Delete User", Description = "Can delete users", Module = "Users", Action = "Delete", IsActive = true },
                new Permission { Id = 5, Name = "View Dashboard", Description = "Can view dashboard", Module = "Dashboard", Action = "View", IsActive = true },
                new Permission { Id = 6, Name = "Upload CSV", Description = "Can upload CSV files", Module = "DrillHole", Action = "Upload", IsActive = true },
                new Permission { Id = 7, Name = "View DrillHoles", Description = "Can view drill holes", Module = "DrillHole", Action = "View", IsActive = true },
                new Permission { Id = 8, Name = "View Projects", Description = "Can view project list", Module = "Projects", Action = "View", IsActive = true },
                new Permission { Id = 9, Name = "Create Project", Description = "Can create new projects", Module = "Projects", Action = "Create", IsActive = true },
                new Permission { Id = 10, Name = "Edit Project", Description = "Can edit project details", Module = "Projects", Action = "Edit", IsActive = true },
                new Permission { Id = 11, Name = "Delete Project", Description = "Can delete projects", Module = "Projects", Action = "Delete", IsActive = true }
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
                    Region = "Muscat",
                    Country = "Oman",
                    OmanPhone = "+968 9999 9999",
                    CountryPhone = "+968 9999 9999"
                }
            );

            // Seed Sample Projects
            modelBuilder.Entity<Project>().HasData(
                new Project
                {
                    Id = 1,
                    Name = "Muscat Infrastructure Development - Highway Construction",
                    Region = "Muscat",
                    Status = "Active",
                    Description = "Major highway development project in Muscat region",
                    StartDate = new DateTime(2024, 1, 15),
                    EndDate = new DateTime(2024, 12, 31),
                    AssignedUserId = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Project
                {
                    Id = 2,
                    Name = "Dhofar Mining Operations - Site Development",
                    Region = "Dhofar",
                    Status = "Active",
                    Description = "Mining site expansion project in Dhofar region",
                    StartDate = new DateTime(2024, 2, 1),
                    EndDate = new DateTime(2024, 11, 30),
                    AssignedUserId = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Project
                {
                    Id = 3,
                    Name = "Sohar Industrial Zone - Development",
                    Region = "Al Batinah North",
                    Status = "Completed",
                    Description = "Industrial zone construction project in Sohar",
                    StartDate = new DateTime(2023, 6, 1),
                    EndDate = new DateTime(2023, 12, 31),
                    AssignedUserId = 1,
                    CreatedAt = DateTime.UtcNow.AddMonths(-6),
                    UpdatedAt = DateTime.UtcNow.AddMonths(-1)
                }
            );

            // Seed Sample Project Sites
            modelBuilder.Entity<ProjectSite>().HasData(
                new ProjectSite
                {
                    Id = 1,
                    ProjectId = 1,
                    Name = "Muscat Main Site",
                    Location = "Muscat Highway Junction",
                    Coordinates = "{\"Latitude\":23.5880,\"Longitude\":58.3829}",
                    Status = "Active",
                    Description = "Primary construction site for highway project",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new ProjectSite
                {
                    Id = 2,
                    ProjectId = 2,
                    Name = "Dhofar Mining Pit A",
                    Location = "Dhofar Mining Area",
                    Coordinates = "{\"Latitude\":17.0194,\"Longitude\":54.1085}",
                    Status = "Active",
                    Description = "Main mining pit for ore extraction",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }
    }
} 