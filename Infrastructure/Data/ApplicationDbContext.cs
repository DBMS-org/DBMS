using Microsoft.EntityFrameworkCore;
using Domain.Entities.UserManagement;
using Domain.Entities.ProjectManagement;
using Domain.Entities.DrillingOperations;
using Domain.Entities.BlastingOperations;
using Domain.Entities.MachineManagement;
using Domain.Entities.StoreManagement;
using Domain.Entities.ExplosiveInventory;
using Domain.Common;
using Application.Interfaces.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IDomainEventDispatcher? _dispatcher;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDomainEventDispatcher? dispatcher = null) : base(options)
        {
            _dispatcher = dispatcher;
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
        public DbSet<PasswordResetCode> PasswordResetCodes { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<DrillPoint> DrillPoints { get; set; }
        public DbSet<PatternSettings> PatternSettings { get; set; }
        public DbSet<ExplosiveCalculationResult> ExplosiveCalculationResults { get; set; }
        public DbSet<BlastConnection> BlastConnections { get; set; }
        public DbSet<DetonatorInfo> DetonatorInfos { get; set; }
        public DbSet<ExplosiveApprovalRequest> ExplosiveApprovalRequests { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreInventory> StoreInventories { get; set; }
        public DbSet<StoreTransaction> StoreTransactions { get; set; }

        // Explosive Inventory DbSets
        public DbSet<CentralWarehouseInventory> CentralWarehouseInventories { get; set; }
        public DbSet<ANFOTechnicalProperties> ANFOTechnicalProperties { get; set; }
        public DbSet<EmulsionTechnicalProperties> EmulsionTechnicalProperties { get; set; }
        public DbSet<InventoryTransferRequest> InventoryTransferRequests { get; set; }
        public DbSet<QualityCheckRecord> QualityCheckRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Automatically apply all IEntityTypeConfiguration<T> from this assembly
            // This will pick up all configurations from the categorized folders
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seeding Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin", NormalizedName = "ADMIN", Description = "Administrator with full access" },
                new Role { Id = 2, Name = "Blasting Engineer", NormalizedName = "BLASTING_ENGINEER", Description = "Manages blasting operations" },
                new Role { Id = 3, Name = "Mechanical Engineer", NormalizedName = "MECHANICAL_ENGINEER", Description = "Manages mechanical tasks" },
                new Role { Id = 4, Name = "Machine Manager", NormalizedName = "MACHINE_MANAGER", Description = "Manages machine inventory and assignments" },
                new Role { Id = 5, Name = "Explosive Manager", NormalizedName = "EXPLOSIVE_MANAGER", Description = "Manages explosive materials" },
                new Role { Id = 6, Name = "Store Manager", NormalizedName = "STORE_MANAGER", Description = "Manages store inventory" },
                new Role { Id = 7, Name = "Operator", NormalizedName = "OPERATOR", Description = "Operates machinery" }
            );

            // Seeding Permissions
            modelBuilder.Entity<Permission>().HasData(
                new Permission { Id = 1, Module = "UserManagement", Action = "Create", Name = "Create User", Description = "Allows creating a new user" },
                new Permission { Id = 2, Module = "UserManagement", Action = "Read", Name = "Read User", Description = "Allows viewing user details" },
                new Permission { Id = 3, Module = "UserManagement", Action = "Update", Name = "Update User", Description = "Allows editing user details" },
                new Permission { Id = 4, Module = "UserManagement", Action = "Delete", Name = "Delete User", Description = "Allows deleting a user" },
                new Permission { Id = 5, Module = "ProjectManagement", Action = "Create", Name = "Create Project", Description = "Allows creating a new project" },
                new Permission { Id = 6, Module = "ProjectManagement", Action = "Read", Name = "Read Project", Description = "Allows viewing project details" },
                new Permission { Id = 7, Module = "ProjectManagement", Action = "Update", Name = "Update Project", Description = "Allows editing project details" },
                new Permission { Id = 8, Module = "ProjectManagement", Action = "Delete", Name = "Delete Project", Description = "Allows deleting a project" }
            );

            // Seeding RolePermissions
            modelBuilder.Entity<RolePermission>().HasData(
                // Admin has all permissions
                new RolePermission { Id = 1, RoleId = 1, PermissionId = 1 },
                new RolePermission { Id = 2, RoleId = 1, PermissionId = 2 },
                new RolePermission { Id = 3, RoleId = 1, PermissionId = 3 },
                new RolePermission { Id = 4, RoleId = 1, PermissionId = 4 },
                new RolePermission { Id = 5, RoleId = 1, PermissionId = 5 },
                new RolePermission { Id = 6, RoleId = 1, PermissionId = 6 },
                new RolePermission { Id = 7, RoleId = 1, PermissionId = 7 },
                new RolePermission { Id = 8, RoleId = 1, PermissionId = 8 },
                // Blasting Engineer can read projects
                new RolePermission { Id = 9, RoleId = 2, PermissionId = 6 }
            );

            // Seeding Regions
            modelBuilder.Entity<Region>().HasData(
                new Region { Id = 1, Name = "Muscat", Country = "Oman" },
                new Region { Id = 2, Name = "Dhofar", Country = "Oman" },
                new Region { Id = 3, Name = "Musandam", Country = "Oman" },
                new Region { Id = 4, Name = "Al Buraimi", Country = "Oman" },
                new Region { Id = 5, Name = "Ad Dakhiliyah", Country = "Oman" },
                new Region { Id = 6, Name = "Al Batinah North", Country = "Oman" },
                new Region { Id = 7, Name = "Al Batinah South", Country = "Oman" },
                new Region { Id = 8, Name = "Ash Sharqiyah South", Country = "Oman" },
                new Region { Id = 9, Name = "Ash Sharqiyah North", Country = "Oman" },
                new Region { Id = 10, Name = "Ad Dhahirah", Country = "Oman" },
                new Region { Id = 11, Name = "Al Wusta", Country = "Oman" }
            );
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            if (_dispatcher != null)
            {
                var domainEntities = ChangeTracker.Entries<Domain.Common.BaseEntity>()
                    .Where(e => e.Entity.DomainEvents.Any())
                    .Select(e => e.Entity);

                var domainEvents = domainEntities.SelectMany(e => e.PullDomainEvents()).ToList();

                foreach (var domainEvent in domainEvents)
                {
                    await _dispatcher.DispatchAsync(domainEvent, cancellationToken);
                }
            }

            return result;
        }
    }
}
