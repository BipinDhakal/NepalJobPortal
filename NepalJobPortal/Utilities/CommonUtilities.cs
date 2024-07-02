using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NepalJobPortal.Data;
using NepalJobPortal.EntityModel;

namespace NepalJobPortal.Utilities
{

    public class CommonUtilities
    {
        //private readonly ApplicationDbContext _applicationDbContext;
        //public CommonUtilities(ApplicationDbContext applicationDbContext)
        //{
        //    _applicationDbContext = applicationDbContext;
        //}

        //public int GetCurrentUserId(string userId)
        //{
        //    int? orgId = _applicationDbContext.Users.Where(x => x.Id == userId).Select(x => x.OrgId).FirstOrDefault();
        //    return orgId??0;
        //}

        public static int GetCurrentUserId(ApplicationDbContext context, string userId)
        {
            int? orgId = context.Users.Where(x => x.Id == userId).Select(x => x.OrgId).FirstOrDefault();
            return orgId ?? 0;
        }


        public static IEnumerable<SelectListItem> GetOrganizationList(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>();

            using (var _ent = new ApplicationDbContext(options))
            {
                return _ent.Organization
                    .Select(x => new SelectListItem
                    {
                        Value = x.OrgId.ToString(),
                        Text = x.OrgName
                    })
                    .ToList();
            }
        }


        //public static IEnumerable<SelectListItem> GetVendorList()
        //{
        //    using (ApplicationDbContext _ent = new ApplicationDbContext())
        //    {
        //        return new SelectList(_ent.VendorOrganization.ToList(), "VendorOrgId", "VendorOrgName");
        //    }
        //}
        //public static IEnumerable<SelectListItem> GetVendorList(DbContextOptions<ApplicationDbContext> options)
        //{
        //    using (ApplicationDbContext _ent = new ApplicationDbContext(options))
        //    {
        //        return new SelectList(_ent.VendorOrganization.ToList(), "VendorOrgId", "VendorOrgName");
        //    }
        //}

        public static IEnumerable<SelectListItem> GetVendorList(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>();

            using (var _ent = new ApplicationDbContext(options))
            {
                return _ent.VendorOrganization
                    .Select(v => new SelectListItem
                    {
                        Value = v.VendorOrgId.ToString(),
                        Text = v.VendorOrgName
                    })
                    .ToList();
            }
        }

        public static IEnumerable<SelectListItem> GetCategoryList(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>();

            using (var _ent = new ApplicationDbContext(options))
            {
                return _ent.Category
                    .Select(v => new SelectListItem
                    {
                        Value = v.CategoryId.ToString(),
                        Text = v.CategoryName
                    })
                    .ToList();
            }
        }

    }
}
