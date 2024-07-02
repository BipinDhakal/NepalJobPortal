

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NepalJobPortal.EntityModel;

namespace NepalJobPortal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationIdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Organization> Organization { get; set; }
        public DbSet<VendorOrganization> VendorOrganization { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<JobDescription> JobDescription { get; set; }



    }

}