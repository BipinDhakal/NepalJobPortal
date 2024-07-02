using System.ComponentModel.DataAnnotations;

namespace NepalJobPortal.EntityModel
{
    public class VendorOrganization : Common
    {
        [Key]
        public int VendorOrgId { get; set; }
        public string VendorOrgName { get; set; }
        public string VendorOrgAddress { get; set; }
        public string VendorOrgContact { get; set; }
        public string VendorOrgEmail { get; set; }
        public string VendorOrgImage { get; set; }
    }
}
