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
    [Migration("20250602042904_InitialCreate")]
    partial class InitialCreate
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
                            CreatedAt = new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5398),
                            Description = "Can view user list",
                            IsActive = true,
                            Module = "Users",
                            Name = "View Users",
                            UpdatedAt = new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5398)
                        },
                        new
                        {
                            Id = 2,
                            Action = "Create",
                            CreatedAt = new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5401),
                            Description = "Can create new users",
                            IsActive = true,
                            Module = "Users",
                            Name = "Create User",
                            UpdatedAt = new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5401)
                        },
                        new
                        {
                            Id = 3,
                            Action = "Edit",
                            CreatedAt = new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5403),
                            Description = "Can edit user details",
                            IsActive = true,
                            Module = "Users",
                            Name = "Edit User",
                            UpdatedAt = new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5403)
                        },
                        new
                        {
                            Id = 4,
                            Action = "Delete",
                            CreatedAt = new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5405),
                            Description = "Can delete users",
                            IsActive = true,
                            Module = "Users",
                            Name = "Delete User",
                            UpdatedAt = new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5405)
                        },
                        new
                        {
                            Id = 5,
                            Action = "View",
                            CreatedAt = new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5406),
                            Description = "Can view dashboard",
                            IsActive = true,
                            Module = "Dashboard",
                            Name = "View Dashboard",
                            UpdatedAt = new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5407)
                        },
                        new
                        {
                            Id = 6,
                            Action = "Upload",
                            CreatedAt = new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5408),
                            Description = "Can upload CSV files",
                            IsActive = true,
                            Module = "DrillHole",
                            Name = "Upload CSV",
                            UpdatedAt = new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5408)
                        },
                        new
                        {
                            Id = 7,
                            Action = "View",
                            CreatedAt = new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5472),
                            Description = "Can view drill holes",
                            IsActive = true,
                            Module = "DrillHole",
                            Name = "View DrillHoles",
                            UpdatedAt = new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5472)
                        });
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
                            CreatedAt = new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5283),
                            Description = "Administrator with full access",
                            IsActive = true,
                            Name = "Admin",
                            NormalizedName = "ADMIN",
                            UpdatedAt = new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5285)
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5290),
                            Description = "Blasting Engineer with technical access",
                            IsActive = true,
                            Name = "BlastingEngineer",
                            NormalizedName = "BLASTINGENGINEER",
                            UpdatedAt = new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5290)
                        },
                        new
                        {
                            Id = 3,
                            CreatedAt = new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5292),
                            Description = "Regular user with limited access",
                            IsActive = true,
                            Name = "User",
                            NormalizedName = "USER",
                            UpdatedAt = new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5292)
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

                    b.Property<string>("Area")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

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

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("OmanPhone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

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

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Area = "Central",
                            Country = "Oman",
                            CountryPhone = "+968 9999 9999",
                            CreatedAt = new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5492),
                            Email = "admin@dbms.com",
                            Name = "System Administrator",
                            OmanPhone = "+968 9999 9999",
                            Region = "Muscat",
                            Role = "Admin",
                            Status = "Active",
                            UpdatedAt = new DateTime(2025, 6, 2, 4, 29, 3, 959, DateTimeKind.Utc).AddTicks(5492)
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
