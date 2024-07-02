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
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public CategoryController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IActionResult Index()
        {
            CategoryViewModel model = new CategoryViewModel();
            model.CategoryList = (from i in _applicationDbContext.Category
                                            where i.IsDeleted == false
                                            select new CategoryViewModel
                                            {
                                                CategoryId = i.CategoryId,
                                                CategoryName = i.CategoryName,
                                            }).ToList();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            int currentloginOrgId = CommonUtilities.GetCurrentUserId(_applicationDbContext, User.FindFirstValue(ClaimTypes.NameIdentifier));

            Category org = new Category();
            org.CategoryName = model.CategoryName;
            org.Status = true;
            org.CreatedBy = currentloginOrgId;
            org.CreatedDate = DateTime.Now;
            _applicationDbContext.Entry(org).State = EntityState.Added;
            _applicationDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            CategoryViewModel model = new CategoryViewModel();
            if (id > 0)
            {
                var data = _applicationDbContext.Category.Where(x => x.CategoryId == id).FirstOrDefault();
                if (data != null)
                {
                    model.CategoryId = data.CategoryId;
                    model.CategoryName = data.CategoryName;
                }
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            int currentloginOrgId = CommonUtilities.GetCurrentUserId(_applicationDbContext, User.FindFirstValue(ClaimTypes.NameIdentifier));

            Category org = new Category();
            org.CategoryId = model.CategoryId??0;
            org.CategoryName = model.CategoryName;
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
                var org = _applicationDbContext.Category.Where(x => x.CategoryId == id).FirstOrDefault();
                if (org != null)
                {
                    org.DeletedBy = 1;
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
