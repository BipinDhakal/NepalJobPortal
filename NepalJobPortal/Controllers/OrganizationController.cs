using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NepalJobPortal.Data;
using NepalJobPortal.EntityModel;
using NepalJobPortal.Migrations;
using NepalJobPortal.Models;
using NepalJobPortal.Utilities;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace NepalJobPortal.Controllers
{
    [Authorize]
    public class OrganizationController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        public OrganizationController(ApplicationDbContext applicationDbContext, UserManager<ApplicationIdentityUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            OrganizationViewModel model = new OrganizationViewModel();
            model.OrganizationList = (from i in _applicationDbContext.Organization
                                      where i.IsDeleted == false
                                      select new OrganizationViewModel
                                      {
                                          OrgId = i.OrgId,
                                          OrgName = i.OrgName,
                                          OrgAddress = i.OrgAddress,
                                          OrgContact = i.OrgContact,
                                          OrgEmail = i.OrgEmail,
                                          OrgImage = i.OrgImage,
                                      }).ToList();
            return View(model);
        }

        public IActionResult Create()
        {
            //ViewBag.OrganizationList = _applicationDbContext.Organization
            //                            .Where(x => x.IsDeleted == false)
            //                            .ToList();

            return View();
        }

        //[HttpPost]
        //public IActionResult Create(OrganizationViewModel model)
        //{
        //    Organization org = new Organization();
        //    org.OrgName = model.OrgName;
        //    org.OrgAddress = model.OrgAddress;
        //    org.OrgEmail = model.OrgEmail;
        //    org.OrgContact = model.OrgContact;
        //    org.Status = true;
        //    org.CreatedBy = 1;
        //    org.CreatedDate = DateTime.Now;
        //    _applicationDbContext.Entry(org).State = EntityState.Added;
        //    _applicationDbContext.SaveChanges();

        //    ApplicationIdentityUser user = new ApplicationIdentityUser();
        //    user.Email = model.OrgEmail;
        //    user.OrgId = org.OrgId;
        //    //_userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
        //    var result = _userManager.CreateAsync(user, $"{model.OrgName}@123");
        //    if (result.IsCompletedSuccessfully)
        //    {
        //        _userManager.AddToRoleAsync(user, "Vendor");
        //    }
        //        return RedirectToAction("Index");
        //}

        [HttpPost]
        public IActionResult Create(OrganizationViewModel model, IFormFile? file)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            //ViewBag.OrganizationList = _applicationDbContext.Organization
            //                            .Where(x => x.IsDeleted == false)
            //                            .ToList();
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
                model.OrgImage = "/Files/" + GuidId + file.FileName;
            }

            int currentloginOrgId = CommonUtilities.GetCurrentUserId(_applicationDbContext, User.FindFirstValue(ClaimTypes.NameIdentifier));
            using (var transaction = _applicationDbContext.Database.BeginTransaction())
            {
                try
                {
                    Organization org = new Organization
                    {
                        OrgName = model.OrgName,
                        OrgAddress = model.OrgAddress,
                        OrgEmail = model.OrgEmail,
                        OrgContact = model.OrgContact,
                        OrgImage = model.OrgImage,
                        Status = true,
                        CreatedBy = currentloginOrgId,
                        CreatedDate = DateTime.Now
                    };
                    _applicationDbContext.Entry(org).State = EntityState.Added;
                    _applicationDbContext.SaveChanges();

                    ApplicationIdentityUser user = new ApplicationIdentityUser
                    {
                        UserName = model.OrgEmail,
                        Email = model.OrgEmail,
                        OrgId = org.OrgId,
                        EmailConfirmed = true,
                    };
                    var result = _userManager.CreateAsync(user, $"{model.OrgName}@Vo123").Result;
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
            OrganizationViewModel model = new OrganizationViewModel();
            if (id > 0)
            {
                var org = _applicationDbContext.Organization.Where(x=>x.OrgId == id).FirstOrDefault();  
                if(org != null)
                {
                    model.OrgId = org.OrgId;
                    model.OrgName = org.OrgName;
                    model.OrgAddress = org.OrgAddress;
                    model.OrgEmail = org.OrgEmail;
                    model.OrgContact = org.OrgContact;
                    model.OrgImage = org.OrgImage;
                }
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(OrganizationViewModel model, IFormFile? file)
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
                model.OrgImage = "/Files/" + GuidId + file.FileName;
            }
            Organization org = new Organization();
            org.OrgId = model.OrgId??0;
            org.OrgName = model.OrgName;
            org.OrgAddress = model.OrgAddress;
            org.OrgEmail = model.OrgEmail;
            org.OrgContact = model.OrgContact;
            org.OrgImage = model.OrgImage;
            org.Status = true;
            org.UpdatedBy = currentloginOrgId;
            org.UpdatedDate = DateTime.Now;
            _applicationDbContext.Entry(org).State = EntityState.Modified;
            _applicationDbContext.Entry(org).Property(x => x.CreatedBy).IsModified = false;
            _applicationDbContext.Entry(org).Property(x => x.CreatedDate).IsModified = false;
            if(model.OrgImage == null)
            {
                _applicationDbContext.Entry(org).Property(x => x.OrgImage).IsModified = false;
            }
            _applicationDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (id > 0)
            {
                int currentloginOrgId = CommonUtilities.GetCurrentUserId(_applicationDbContext, User.FindFirstValue(ClaimTypes.NameIdentifier));
                var org = _applicationDbContext.Organization.Where(x => x.OrgId == id).FirstOrDefault();
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
