using InventorySystem.Models.DataEntities;
using InventorySystem.Models.Identities;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<CreateHistory> CreateHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*User model Constraints*/
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .UseCollation("utf8mb4_bin");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            /*Item model Constraints*/
            modelBuilder.Entity<Item>()
                .HasIndex(i => new { i.UserId, i.ItemCode })
                .IsUnique();

            /*UserRole model Builder*/
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            /*Create History Model Builder*/
            modelBuilder.Entity<CreateHistory>()
                .HasKey(ch => ch.HistoryId);
            
            /*Roles model builder*/
            modelBuilder.Entity<Role>()
                .HasKey(r => r.RoleId);

            /*Role Seeding*/
            modelBuilder.Entity<Role>().HasData(
                    new Role
                    {
                        RoleId = 1,
                        Name = "Administrator",
                        RoleDisplayName = "Admin"
                    },
                    new Role
                    {
                        RoleId = 2,
                        Name = "User",
                        RoleDisplayName = "User"
                    },
                    new Role
                    {
                        RoleId = 3,
                        Name = "Requester",
                        RoleDisplayName = "Requester"
                    }

                );

            /*Category Seeding*/
            modelBuilder.Entity<Category>().HasData(
                    new Category
                    {
                        CategoryId = 1,
                        Name = "Robots",
                        CategoryDisplayName = "Robots"
                    },
                    new Category
                    {
                        CategoryId = 2,
                        Name = "Books",
                        CategoryDisplayName = "Books"
                    },
                    new Category
                    {
                        CategoryId = 3,
                        Name = "Materials",
                        CategoryDisplayName = "Materials"
                    }


                );
        }
    }
}
