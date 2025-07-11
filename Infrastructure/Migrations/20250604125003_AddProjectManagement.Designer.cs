// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250604125003_AddProjectManagement")]
    partial class AddProjectManagement
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Core.Entities.DrillHole", b =>
                {
                    b.Property<int>("SerialNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SerialNumber"));

                    b.Property<double>("ActualDepth")
                        .HasColumnType("float");

                    b.Property<double>("Azimuth")
                        .HasColumnType("float");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<double>("Depth")
                        .HasColumnType("float");

                    b.Property<double>("Dip")
                        .HasColumnType("float");

                    b.Property<double>("Easting")
                        .HasColumnType("float");

                    b.Property<double>("Elevation")
                        .HasColumnType("float");

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Length")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Northing")
                        .HasColumnType("float");

                    b.Property<double>("Stemming")
                        .HasColumnType("float");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("SerialNumber");

                    b.ToTable("DrillHoles");
                });

            modelBuilder.Entity("Core.Entities.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Module")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Module", "Action")
                        .IsUnique();

                    b.ToTable("Permissions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Action = "View",
                            CreatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3337),
                            Description = "Can view user list",
                            IsActive = true,
                            Module = "Users",
                            Name = "View Users",
                            UpdatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3337)
                        },
                        new
                        {
                            Id = 2,
                            Action = "Create",
                            CreatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3341),
                            Description = "Can create new users",
                            IsActive = true,
                            Module = "Users",
                            Name = "Create User",
                            UpdatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3341)
                        },
                        new
                        {
                            Id = 3,
                            Action = "Edit",
                            CreatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3343),
                            Description = "Can edit user details",
                            IsActive = true,
                            Module = "Users",
                            Name = "Edit User",
                            UpdatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3343)
                        },
                        new
                        {
                            Id = 4,
                            Action = "Delete",
                            CreatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3345),
                            Description = "Can delete users",
                            IsActive = true,
                            Module = "Users",
                            Name = "Delete User",
                            UpdatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3345)
                        },
                        new
                        {
                            Id = 5,
                            Action = "View",
                            CreatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3346),
                            Description = "Can view dashboard",
                            IsActive = true,
                            Module = "Dashboard",
                            Name = "View Dashboard",
                            UpdatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3346)
                        },
                        new
                        {
                            Id = 6,
                            Action = "Upload",
                            CreatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3348),
                            Description = "Can upload CSV files",
                            IsActive = true,
                            Module = "DrillHole",
                            Name = "Upload CSV",
                            UpdatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3348)
                        },
                        new
                        {
                            Id = 7,
                            Action = "View",
                            CreatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3349),
                            Description = "Can view drill holes",
                            IsActive = true,
                            Module = "DrillHole",
                            Name = "View DrillHoles",
                            UpdatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3350)
                        },
                        new
                        {
                            Id = 8,
                            Action = "View",
                            CreatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3351),
                            Description = "Can view project list",
                            IsActive = true,
                            Module = "Projects",
                            Name = "View Projects",
                            UpdatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3351)
                        },
                        new
                        {
                            Id = 9,
                            Action = "Create",
                            CreatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3353),
                            Description = "Can create new projects",
                            IsActive = true,
                            Module = "Projects",
                            Name = "Create Project",
                            UpdatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3353)
                        },
                        new
                        {
                            Id = 10,
                            Action = "Edit",
                            CreatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3354),
                            Description = "Can edit project details",
                            IsActive = true,
                            Module = "Projects",
                            Name = "Edit Project",
                            UpdatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3354)
                        },
                        new
                        {
                            Id = 11,
                            Action = "Delete",
                            CreatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3356),
                            Description = "Can delete projects",
                            IsActive = true,
                            Module = "Projects",
                            Name = "Delete Project",
                            UpdatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3356)
                        });
                });

            modelBuilder.Entity("Core.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AssignedUserId")
                        .HasColumnType("int");

                    b.Property<decimal>("Budget")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ProjectValue")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AssignedUserId");

                    b.HasIndex("Name");

                    b.ToTable("Projects");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AssignedUserId = 1,
                            Budget = 5500000.00m,
                            CreatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3484),
                            Description = "Major highway development project in Muscat region",
                            EndDate = new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Muscat Infrastructure Development",
                            ProjectValue = "Highway Construction",
                            Region = "Muscat",
                            StartDate = new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Status = "In Progress",
                            UpdatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3484)
                        },
                        new
                        {
                            Id = 2,
                            AssignedUserId = 1,
                            Budget = 8200000.00m,
                            CreatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3489),
                            Description = "Limestone quarrying operations in Dhofar region",
                            EndDate = new DateTime(2025, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Dhofar Mining Operations",
                            ProjectValue = "Mining Operations",
                            Region = "Dhofar",
                            StartDate = new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Status = "Planning",
                            UpdatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3490)
                        });
                });

            modelBuilder.Entity("Core.Entities.ProjectSite", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Coordinates")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectSites");
                });

            modelBuilder.Entity("Core.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3226),
                            Description = "Administrator with full access",
                            IsActive = true,
                            Name = "Admin",
                            NormalizedName = "ADMIN",
                            UpdatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3228)
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3233),
                            Description = "Blasting Engineer with technical access",
                            IsActive = true,
                            Name = "BlastingEngineer",
                            NormalizedName = "BLASTINGENGINEER",
                            UpdatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3233)
                        },
                        new
                        {
                            Id = 3,
                            CreatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3235),
                            Description = "Regular user with limited access",
                            IsActive = true,
                            Name = "User",
                            NormalizedName = "USER",
                            UpdatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3235)
                        });
                });

            modelBuilder.Entity("Core.Entities.RolePermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("GrantedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("PermissionId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("RevokedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PermissionId");

                    b.HasIndex("RoleId");

                    b.ToTable("RolePermissions");
                });

            modelBuilder.Entity("Core.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("CountryPhone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("LastLoginAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("OmanPhone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Country = "Oman",
                            CountryPhone = "+968 9999 9999",
                            CreatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3384),
                            Email = "admin@dbms.com",
                            Name = "System Administrator",
                            OmanPhone = "+968 9999 9999",
                            PasswordHash = "$2a$11$K8QQfR6Z5j6XgkHjWo9xXeNqO7QDj9qQVvjBZjR8g1jzQzKL9Yd3W",
                            Region = "Muscat",
                            Role = "Admin",
                            Status = "Active",
                            UpdatedAt = new DateTime(2025, 6, 4, 12, 50, 3, 406, DateTimeKind.Utc).AddTicks(3384)
                        });
                });

            modelBuilder.Entity("Core.Entities.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AssignedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("RevokedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Core.Entities.Project", b =>
                {
                    b.HasOne("Core.Entities.User", "AssignedUser")
                        .WithMany()
                        .HasForeignKey("AssignedUserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("AssignedUser");
                });

            modelBuilder.Entity("Core.Entities.ProjectSite", b =>
                {
                    b.HasOne("Core.Entities.Project", "Project")
                        .WithMany("ProjectSites")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Core.Entities.RolePermission", b =>
                {
                    b.HasOne("Core.Entities.Permission", "Permission")
                        .WithMany("RolePermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Core.Entities.UserRole", b =>
                {
                    b.HasOne("Core.Entities.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Core.Entities.Permission", b =>
                {
                    b.Navigation("RolePermissions");
                });

            modelBuilder.Entity("Core.Entities.Project", b =>
                {
                    b.Navigation("ProjectSites");
                });

            modelBuilder.Entity("Core.Entities.Role", b =>
                {
                    b.Navigation("RolePermissions");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Core.Entities.User", b =>
                {
                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
