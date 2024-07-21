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
    public class JobDescriptionController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public JobDescriptionController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IActionResult Index()
        {
            JobDescriptionViewModel model = new JobDescriptionViewModel();
            model.JobDescriptionList = (from i in _applicationDbContext.JobDescription
                                        join j in _applicationDbContext.Category on i.CategoryId equals j.CategoryId
                                        join k in _applicationDbContext.VendorOrganization on i.vendorOrgId equals k.VendorOrgId
                                        where i.IsDeleted == false
                                        select new JobDescriptionViewModel
                                        {
                                            JobDescriptionId = i.JobDescriptionId,
                                            vendorOrgId = i.vendorOrgId,
                                            CategoryId = i.CategoryId,
                                            JobType = i.JobType,
                                            Level = i.Level,
                                            VacancyNo = i.VacancyNo,
                                            EmployeeType = i.EmployeeType,
                                            JobLocation = i.JobLocation,
                                            OfferedSalary = i.OfferedSalary,
                                            DeadLine = i.DeadLine,
                                            EducationLevel = i.EducationLevel,
                                            ExperienceRequired = i.ExperienceRequired,
                                            OtherSpecification = i.OtherSpecification,
                                            JobWorkDescription = i.JobWorkDescription,
                                            VendorName = k.VendorOrgName,
                                            CategoryName = j.CategoryName,
                                            VendorImage = k.VendorOrgImage,
                                        }).ToList();
            if(User.IsInRole("Vendor"))
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var OrdId = _applicationDbContext.Users.Where(x => x.Id == currentUserId).Select(x => x.OrgId).FirstOrDefault();
                model.JobDescriptionList = model.JobDescriptionList
                    .Where(x => x.vendorOrgId == OrdId).ToList();
            }
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(JobDescriptionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            int currentloginOrgId = CommonUtilities.GetCurrentUserId(_applicationDbContext, User.FindFirstValue(ClaimTypes.NameIdentifier));

            JobDescription org = new JobDescription();
            if(User.IsInRole("SuperAdmin"))
            {
                org.vendorOrgId = model.vendorOrgId;
            }
            else
            {
                org.vendorOrgId = currentloginOrgId;
            }
            org.CategoryId = model.CategoryId;
            org.JobType = model.JobType;
            org.Level = model.Level;
            org.VacancyNo = model.VacancyNo;
            org.EmployeeType = model.EmployeeType;
            org.JobLocation = model.JobLocation;
            org.OfferedSalary = model.OfferedSalary;
            org.DeadLine = model.DeadLine;
            org.EducationLevel = model.EducationLevel;
            org.ExperienceRequired = model.ExperienceRequired;
            org.OtherSpecification = model.OtherSpecification;
            org.JobWorkDescription = model.JobWorkDescription;
            org.Status = true;
            org.CreatedBy = currentloginOrgId;
            org.CreatedDate = DateTime.Now;
            _applicationDbContext.Entry(org).State = EntityState.Added;
            _applicationDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            JobDescriptionViewModel model = new JobDescriptionViewModel();
            if (id > 0)
            {
                var org = _applicationDbContext.JobDescription.Where(x => x.JobDescriptionId == id).FirstOrDefault();
                if (org != null)
                {
                    model.JobDescriptionId = org.JobDescriptionId;
                    model.vendorOrgId = org.vendorOrgId;
                    model.CategoryId = org.CategoryId;
                    model.JobType = org.JobType;
                    model.Level = org.Level;
                    model.VacancyNo = org.VacancyNo;
                    model.EmployeeType = org.EmployeeType;
                    model.JobLocation = org.JobLocation;
                    model.OfferedSalary = org.OfferedSalary;
                    model.DeadLine = org.DeadLine;
                    model.EducationLevel = org.EducationLevel;
                    model.ExperienceRequired = org.ExperienceRequired;
                    model.OtherSpecification = org.OtherSpecification;
                    model.JobWorkDescription = org.JobWorkDescription;
                }
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(JobDescriptionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            int currentloginOrgId = CommonUtilities.GetCurrentUserId(_applicationDbContext, User.FindFirstValue(ClaimTypes.NameIdentifier));

            JobDescription org = new JobDescription();
            org.JobDescriptionId = model.JobDescriptionId??0;
            org.vendorOrgId = model.vendorOrgId;
            org.CategoryId = model.CategoryId;
            org.JobType = model.JobType;
            org.Level = model.Level;
            org.VacancyNo = model.VacancyNo;
            org.EmployeeType = model.EmployeeType;
            org.JobLocation = model.JobLocation;
            org.OfferedSalary = model.OfferedSalary;
            org.DeadLine = model.DeadLine;
            org.EducationLevel = model.EducationLevel;
            org.ExperienceRequired = model.ExperienceRequired;
            org.OtherSpecification = model.OtherSpecification;
            org.JobWorkDescription = model.JobWorkDescription;
            org.Status = true;
            org.UpdatedBy = currentloginOrgId;
            org.UpdatedDate = DateTime.Now;
            _applicationDbContext.Entry(org).State = EntityState.Modified;
            _applicationDbContext.Entry(org).Property(x => x.CreatedBy).IsModified = false;
            _applicationDbContext.Entry(org).Property(x => x.CreatedDate).IsModified = false;
            _applicationDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (id > 0)
            {
                int currentloginOrgId = CommonUtilities.GetCurrentUserId(_applicationDbContext, User.FindFirstValue(ClaimTypes.NameIdentifier));

                var org = _applicationDbContext.JobDescription.Where(x => x.JobDescriptionId == id).FirstOrDefault();
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

        public IActionResult Details(int id)
        {
            JobDescriptionViewModel model = new JobDescriptionViewModel();
            model.JobDescriptionList = (from i in _applicationDbContext.JobDescription
                                        join j in _applicationDbContext.Category on i.CategoryId equals j.CategoryId
                                        join k in _applicationDbContext.VendorOrganization on i.vendorOrgId equals k.VendorOrgId
                                        where i.IsDeleted == false && i.JobDescriptionId == id
                                        select new JobDescriptionViewModel
                                        {
                                            JobDescriptionId = i.JobDescriptionId,
                                            vendorOrgId = i.vendorOrgId,
                                            CategoryId = i.CategoryId,
                                            JobType = i.JobType,
                                            Level = i.Level,
                                            VacancyNo = i.VacancyNo,
                                            EmployeeType = i.EmployeeType,
                                            JobLocation = i.JobLocation,
                                            OfferedSalary = i.OfferedSalary,
                                            DeadLine = i.DeadLine,
                                            EducationLevel = i.EducationLevel,
                                            ExperienceRequired = i.ExperienceRequired,
                                            OtherSpecification = i.OtherSpecification,
                                            JobWorkDescription = i.JobWorkDescription,
                                            VendorName = k.VendorOrgName,
                                            CategoryName = j.CategoryName,
                                            VendorImage = k.VendorOrgImage,
                                        }).ToList();
            
            return View(model);
        }
    }
}
