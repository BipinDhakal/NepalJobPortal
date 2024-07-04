using Azure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NepalJobPortal.EntityModel;

namespace NepalJobPortal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationIdentityUser>
    {
        private readonly string _superAdminRoleId = Guid.NewGuid().ToString();
        private readonly string _vendorRoleId = Guid.NewGuid().ToString();
        private readonly string _superAdminUserId = Guid.NewGuid().ToString();
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Organization> Organization { get; set; }
        public DbSet<VendorOrganization> VendorOrganization { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<JobDescription> JobDescription { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.SeedRoles(builder);
            this.SeedUsers(builder);
            this.SeedUserRoles(builder);
        }

        private void SeedUsers(ModelBuilder builder)
        {
            ApplicationIdentityUser user = new ApplicationIdentityUser()
            {
                Id = _superAdminUserId,
                UserName = "bipindhakal05@gmail.com",
                Email = "bipindhakal05@gmail.com",
                NormalizedUserName = "bipindhakal05@gmail.com".ToUpper(),
                EmailConfirmed = true
            };

            PasswordHasher<ApplicationIdentityUser> passwordHasher = new PasswordHasher<ApplicationIdentityUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, "Bipin@123");

            builder.Entity<ApplicationIdentityUser>().HasData(user);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id = _superAdminRoleId, Name = "SuperAdmin", ConcurrencyStamp = "1", NormalizedName = "SUPERADMIN" },
                new IdentityRole() { Id = _vendorRoleId, Name = "Vendor", ConcurrencyStamp = "2", NormalizedName = "VENDOR" }
                );
        }

        private void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { RoleId = _superAdminRoleId, UserId = _superAdminUserId }
                );
        }

    }

}