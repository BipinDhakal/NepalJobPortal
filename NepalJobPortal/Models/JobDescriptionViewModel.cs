namespace NepalJobPortal.Models
{
    public class JobDescriptionViewModel : CommonViewModel
    {
        public int? JobDescriptionId { get; set; }
        public int vendorOrgId { get; set; }
        public int CategoryId { get; set; }
        public string JobType { get; set; }
        public string Level { get; set; }
        public int VacancyNo { get; set; }
        public string EmployeeType { get; set; }
        public string JobLocation { get; set; }
        public string OfferedSalary { get; set; }
        public DateTime DeadLine { get; set; }
        public string EducationLevel { get; set; }
        public string ExperienceRequired { get; set; }
        public string OtherSpecification { get; set; }
        public string JobWorkDescription { get; set; }
        public string? VendorName {  get; set; }
        public string? CategoryName { get; set; }
        public string? VendorImage { get; set; }

        public List<JobDescriptionViewModel> JobDescriptionList { get; set; }

        public JobDescriptionViewModel()
        {
            JobDescriptionList = new List<JobDescriptionViewModel>();
        }
    }
}
