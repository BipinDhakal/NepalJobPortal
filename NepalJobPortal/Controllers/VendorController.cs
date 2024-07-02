using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NepalJobPortal.Data;
using NepalJobPortal.EntityModel;
using NepalJobPortal.Models;
using NepalJobPortal.Utilities;
using System.Security.Claims;

namespace NepalJobPortal.Controllers
{
    [Authorize]
    public class VendorController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        public VendorController(ApplicationDbContext applicationDbContext, UserManager<ApplicationIdentityUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            VendorOrganizationViewModel model = new VendorOrganizationViewModel();
            model.VendorOrganizationList = (from i in _applicationDbContext.VendorOrganization
                                      where i.IsDeleted == false
                                      select new VendorOrganizationViewModel
                                      {
                                          VendorOrgId = i.VendorOrgId,
                                          VendorOrgName = i.VendorOrgName,
                                          VendorOrgAddress = i.VendorOrgAddress,
                                          VendorOrgContact = i.VendorOrgContact,
                                          VendorOrgEmail = i.VendorOrgEmail,
                                          VendorOrgImage = i.VendorOrgImage,
                                      }).ToList();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(VendorOrganizationViewModel model, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
          
            if (file != null)
            {
                var GuidId = Guid.NewGuid().ToString();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string fileName = GuidId + file.FileName;
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                model.VendorOrgImage = "/Files/" + GuidId + file.FileName;
            }

            int currentloginOrgId = CommonUtilities.GetCurrentUserId(_applicationDbContext, User.FindFirstValue(ClaimTypes.NameIdentifier));
            using (var transaction = _applicationDbContext.Database.BeginTransaction())
            {
                try
                {
                    VendorOrganization org = new VendorOrganization
                    {
                        VendorOrgName = model.VendorOrgName,
                        VendorOrgAddress = model.VendorOrgAddress,
                        VendorOrgEmail = model.VendorOrgEmail,
                        VendorOrgContact = model.VendorOrgContact,
                        VendorOrgImage = model.VendorOrgImage,
                        Status = true,
                        CreatedBy = currentloginOrgId,
                        CreatedDate = DateTime.Now
                    };
                    _applicationDbContext.Entry(org).State = EntityState.Added;
                    _applicationDbContext.SaveChanges();

                    ApplicationIdentityUser user = new ApplicationIdentityUser
                    {
                        UserName = model.VendorOrgEmail,
                        Email = model.VendorOrgEmail,
                        OrgId = org.VendorOrgId,
                        EmailConfirmed = true,
                    };
                    //var result = _userManager.CreateAsync(user, $"{model.VendorOrgName}@Vo123").Result;
                    var result = _userManager.CreateAsync(user, $"Job@12{model.VendorOrgEmail}").Result;
                    if (result.Succeeded)
                    {
                        _userManager.AddToRoleAsync(user, "Vendor").Wait();
                        transaction.Commit();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        transaction.Rollback();
                        ModelState.AddModelError(string.Empty, "Failed to create user.");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the organization.");
                }
            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            VendorOrganizationViewModel model = new VendorOrganizationViewModel();
            if (id > 0)
            {
                var org = _applicationDbContext.VendorOrganization.Where(x => x.VendorOrgId == id).FirstOrDefault();
                if (org != null)
                {
                    model.VendorOrgId = org.VendorOrgId;
                    model.VendorOrgName = org.VendorOrgName;
                    model.VendorOrgAddress = org.VendorOrgAddress;
                    model.VendorOrgEmail = org.VendorOrgEmail;
                    model.VendorOrgContact = org.VendorOrgContact;
                    model.VendorOrgImage = org.VendorOrgImage;
                }
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(VendorOrganizationViewModel model, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            int currentloginOrgId = CommonUtilities.GetCurrentUserId(_applicationDbContext, User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (file != null)
            {
                var GuidId = Guid.NewGuid().ToString();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string fileName = GuidId + file.FileName;
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                model.VendorOrgImage = "/Files/" + GuidId + file.FileName;
            }
            VendorOrganization org = new VendorOrganization();
            org.VendorOrgId = model.VendorOrgId ?? 0;
            org.VendorOrgName = model.VendorOrgName;
            org.VendorOrgAddress = model.VendorOrgAddress;
            org.VendorOrgEmail = model.VendorOrgEmail;
            org.VendorOrgContact = model.VendorOrgContact;
            org.VendorOrgImage = model.VendorOrgImage;
            org.Status = true;
            org.UpdatedBy = currentloginOrgId;
            org.UpdatedDate = DateTime.Now;
            _applicationDbContext.Entry(org).State = EntityState.Modified;
            _applicationDbContext.Entry(org).Property(x => x.CreatedBy).IsModified = false;
            _applicationDbContext.Entry(org).Property(x => x.CreatedDate).IsModified = false;
            if (model.VendorOrgImage == null)
            {
                _applicationDbContext.Entry(org).Property(x => x.VendorOrgImage).IsModified = false;
            }
            _applicationDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (id > 0)
            {
                int currentloginOrgId = CommonUtilities.GetCurrentUserId(_applicationDbContext, User.FindFirstValue(ClaimTypes.NameIdentifier));

                var org = _applicationDbContext.VendorOrganization.Where(x => x.VendorOrgId == id).FirstOrDefault();
                if (org != null)
                {
                    org.DeletedBy = currentloginOrgId;
                    org.DeletedDate = DateTime.Now;
                    org.IsDeleted = true;
                    _applicationDbContext.Entry(org).State = EntityState.Modified;
                    _applicationDbContext.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
