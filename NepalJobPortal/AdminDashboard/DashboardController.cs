using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NepalJobPortal.Data;
using NepalJobPortal.EntityModel;
using NepalJobPortal.Models;
using NepalJobPortal.Utilities;
using System.Security.Claims;

namespace NepalJobPortal.AdminDashboard
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        public DashboardController(ApplicationDbContext applicationDbContext, UserManager<ApplicationIdentityUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }
        //public IActionResult Index()
        //{
        //    var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (currentUserId != null)
        //    {
        //        var OrdId = _applicationDbContext.Users.Where(x=>x.Id == currentUserId).Select(x=>x.OrgId).FirstOrDefault();
        //        int currentloginOrgId = CommonUtilities.GetCurrentUserId(_applicationDbContext, currentUserId);
        //    }
        //    var currentUsername = User.FindFirstValue(ClaimTypes.Name);
        //    var currentUseremail = User.FindFirstValue(ClaimTypes.Email);
        //    //var currentUserId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    var email = HttpContext.Session.GetString("Email");
        //    var userId = HttpContext.Session.GetString("UserId");

        //    var roleId = _applicationDbContext.UserRoles.Where(x => x.UserId == currentUserId).Select(x => x.RoleId).FirstOrDefault();
        //    var currentUserRole = _applicationDbContext.Roles.Where(x => x.Id == roleId).Select(x => x.Name).FirstOrDefault();
            
        //    return View();
        //}

        public async Task<IActionResult> Index()
        {
            DashboardViewModel model = new DashboardViewModel();
            model.VendorCount = _applicationDbContext.VendorOrganization.Count();
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != null)
            {
                var OrdId = _applicationDbContext.Users.Where(x => x.Id == currentUserId).Select(x => x.OrgId).FirstOrDefault();
                int currentloginOrgId = CommonUtilities.GetCurrentUserId(_applicationDbContext, currentUserId);
            }
            var currentUsername = User.FindFirstValue(ClaimTypes.Name);
            var currentUseremail = User.FindFirstValue(ClaimTypes.Email);
            var email = HttpContext.Session.GetString("Email");
            var userId = HttpContext.Session.GetString("UserId");
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            var currentUserRole = roles.FirstOrDefault();
            return View(model);
        }
    }
}



