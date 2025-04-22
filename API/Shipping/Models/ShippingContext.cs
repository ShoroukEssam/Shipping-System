using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mono.TextTemplating;

namespace Shipping.Models
{
    public class ShippingContext : IdentityDbContext<AppUser, UserRole, string, IdentityUserClaim<string>,
        IdentityUserRole<string>, IdentityUserLogin<string>, UserRoleClaims, IdentityUserToken<string>>
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Government> Governments { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<WeightSetting> weightSettings { get; set; }
        public DbSet<SpecialCitiesPrice> SpecialCitiesPrice { get; set; }

        public ShippingContext() : base()
        { }
        public ShippingContext(DbContextOptions<ShippingContext> options)
        : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Seeding Data For AppUser table with Role Admin
            string ADMIN_ID = "76f86073-b51c-47c4-b7fa-731628055ebb";
            string ROLE_ID = "5ab58670-8727-4b67-85d5-4199912a70bf";

            AppUser admin = new AppUser
            {
                Id = ADMIN_ID,
                Email = "admin@gmail.com",
                NormalizedEmail = "admin@gmail.com".ToUpper(),
                EmailConfirmed = true,
                UserName = "admin",
                NormalizedUserName = "admin@gmail.com".ToUpper(),
                LockoutEnabled = true
            };
            var hasher = new PasswordHasher<AppUser>();
            admin.PasswordHash = hasher.HashPassword(admin, password: "@Admin123");
            modelBuilder.Entity<AppUser>().HasData(admin);


            //Admin Role
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole
                {
                    Id = ROLE_ID,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Date = DateTime.Now.ToString()
                });

            //Connect An admin to Role Admin
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                { RoleId = ROLE_ID, UserId = ADMIN_ID });

            //Create Static Roles
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "المناديب",
                    NormalizedName = "المناديب",
                    Date = DateTime.Now.ToString()
                });
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "التجار",
                    NormalizedName = "التجار",
                    Date = DateTime.Now.ToString()
                });

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "الموظفين",
                    NormalizedName = "الموظفين",
                    Date = DateTime.Now.ToString()
                });

            //Add Permissions
            modelBuilder.Entity<UserRoleClaims>().HasData(
                new UserRoleClaims
                {
                    Id = 1,
                    ClaimType = "Permissions",
                    ClaimValue = "Permissions.Controls.View",
                    RoleId = ROLE_ID,
                    ArabicName = "صلاحيات المجموعات"

                },
                new UserRoleClaims
                {
                    Id = 2,
                    ClaimType = "Permissions",
                    ClaimValue = "Permissions.Controls.Edit",
                    RoleId = ROLE_ID,
                    ArabicName = "صلاحيات المجموعات"
                },
                new UserRoleClaims
                {
                    Id = 3,
                    ClaimType = "Permissions",
                    ClaimValue = "Permissions.Controls.Delete",
                    RoleId = ROLE_ID,
                    ArabicName = "صلاحيات المجموعات"
                },
                new UserRoleClaims
                {
                    Id = 4,
                    ClaimType = "Permissions",
                    ClaimValue = "Permissions.Controls.Create",
                    RoleId = ROLE_ID,
                    ArabicName = "صلاحيات المجموعات"
                });

            //Add WeightSetting
            modelBuilder.Entity<WeightSetting>().HasData(
                new WeightSetting
                {
                    Id = 1,
                    StandaredWeight=10,
                    Addition_Cost=30
                }
                );
            #endregion

        }
    }

}

