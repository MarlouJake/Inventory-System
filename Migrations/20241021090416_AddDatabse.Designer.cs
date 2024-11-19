﻿// <auto-generated />
using System;
using InventorySystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace InventorySystem.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241021090416_AddDatabse")]
    partial class AddDatabse
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("InventorySystem.Models.DataEntities.Admin", b =>
                {
                    b.Property<int>("AdminId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("AdminId"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .HasColumnType("longtext");

                    b.HasKey("AdminId");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("InventorySystem.Models.DataEntities.CreateHistory", b =>
                {
                    b.Property<int?>("HistoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int?>("HistoryId"));

                    b.Property<string>("Category")
                        .HasColumnType("longtext");

                    b.Property<string>("ItemCode")
                        .HasColumnType("longtext");

                    b.Property<int?>("ItemId")
                        .HasColumnType("int");

                    b.Property<string>("ItemName")
                        .HasColumnType("longtext");

                    b.Property<string>("RelativeTimeStamp")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("longtext");

                    b.HasKey("HistoryId");

                    b.HasIndex("ItemId");

                    b.HasIndex("UserId");

                    b.ToTable("CreateHistories");
                });

            modelBuilder.Entity("InventorySystem.Models.DataEntities.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ItemId"));

                    b.Property<string>("Category")
                        .HasColumnType("longtext");

                    b.Property<string>("FirmwareUpdated")
                        .HasColumnType("longtext");

                    b.Property<string>("ItemCode")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("ItemDateAdded")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("ItemDateUpdated")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ItemDescription")
                        .HasMaxLength(5000)
                        .HasColumnType("varchar(5000)");

                    b.Property<string>("ItemName")
                        .HasColumnType("longtext");

                    b.Property<string>("Status")
                        .HasColumnType("longtext");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ItemId");

                    b.HasIndex("UserId", "ItemCode")
                        .IsUnique();

                    b.ToTable("Items");
                });

            modelBuilder.Entity("InventorySystem.Models.DataEntities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Password")
                        .HasColumnType("longtext");

                    b.Property<string>("Username")
                        .HasColumnType("varchar(255)")
                        .UseCollation("utf8mb4_bin");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("InventorySystem.Models.Identities.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            CategoryId = 1,
                            CategoryDisplayName = "Robots",
                            Name = "Robots"
                        },
                        new
                        {
                            CategoryId = 2,
                            CategoryDisplayName = "Books",
                            Name = "Books"
                        },
                        new
                        {
                            CategoryId = 3,
                            CategoryDisplayName = "Materials",
                            Name = "Materials"
                        });
                });

            modelBuilder.Entity("InventorySystem.Models.Identities.ItemCategory", b =>
                {
                    b.Property<int>("ItemCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ItemCategoryId"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.HasKey("ItemCategoryId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ItemId");

                    b.ToTable("ItemCategories");
                });

            modelBuilder.Entity("InventorySystem.Models.Identities.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleDisplayName")
                        .HasColumnType("longtext");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            RoleId = 1,
                            Name = "Administrator",
                            RoleDisplayName = "Admin"
                        },
                        new
                        {
                            RoleId = 2,
                            Name = "User",
                            RoleDisplayName = "User"
                        },
                        new
                        {
                            RoleId = 3,
                            Name = "Requester",
                            RoleDisplayName = "Requester"
                        });
                });

            modelBuilder.Entity("InventorySystem.Models.Identities.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnOrder(0);

                    b.Property<int>("RoleId")
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("InventorySystem.Models.DataEntities.CreateHistory", b =>
                {
                    b.HasOne("InventorySystem.Models.DataEntities.Item", "items")
                        .WithMany("Histories")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("InventorySystem.Models.DataEntities.User", "Users")
                        .WithMany("CreateHistories")
                        .HasForeignKey("UserId");

                    b.Navigation("Users");

                    b.Navigation("items");
                });

            modelBuilder.Entity("InventorySystem.Models.DataEntities.Item", b =>
                {
                    b.HasOne("InventorySystem.Models.DataEntities.User", "User")
                        .WithMany("Items")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("InventorySystem.Models.Identities.ItemCategory", b =>
                {
                    b.HasOne("InventorySystem.Models.Identities.Category", "Category")
                        .WithMany("ItemCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventorySystem.Models.DataEntities.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("InventorySystem.Models.Identities.UserRole", b =>
                {
                    b.HasOne("InventorySystem.Models.Identities.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventorySystem.Models.DataEntities.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("InventorySystem.Models.DataEntities.Item", b =>
                {
                    b.Navigation("Histories");
                });

            modelBuilder.Entity("InventorySystem.Models.DataEntities.User", b =>
                {
                    b.Navigation("CreateHistories");

                    b.Navigation("Items");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("InventorySystem.Models.Identities.Category", b =>
                {
                    b.Navigation("ItemCategories");
                });

            modelBuilder.Entity("InventorySystem.Models.Identities.Role", b =>
                {
                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
