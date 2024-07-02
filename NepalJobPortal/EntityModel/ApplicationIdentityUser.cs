using Microsoft.AspNetCore.Identity;

namespace NepalJobPortal.EntityModel
{
    public class ApplicationIdentityUser : IdentityUser
    {
        public int? OrgId { get; set; }
    }
}
