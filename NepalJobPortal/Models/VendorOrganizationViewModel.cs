namespace NepalJobPortal.Models
{
    public class VendorOrganizationViewModel : CommonViewModel
    {
        public int? VendorOrgId { get; set; }
        public string VendorOrgName { get; set; }
        public string VendorOrgAddress { get; set; }
        public string VendorOrgContact { get; set; }
        public string VendorOrgEmail { get; set; }
        public string? VendorOrgImage { get; set; }

        public List<VendorOrganizationViewModel> VendorOrganizationList { get; set;}

        public VendorOrganizationViewModel()
        {
            VendorOrganizationList = new List<VendorOrganizationViewModel>();
        }

    }
}
