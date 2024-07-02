using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NepalJobPortal.Data;
using NepalJobPortal.Models;
using System.Diagnostics;

namespace NepalJobPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _applicationDbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _applicationDbContext= applicationDbContext;
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
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult JobDetails(int id)
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
                                            CreatedDate = i.CreatedDate,
                                        }).ToList();

            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}