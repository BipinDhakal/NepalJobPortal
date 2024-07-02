using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace NepalJobPortal.EntityModel
{
    public class JobDescription : Common
    {
        [Key]
        public int JobDescriptionId {  get; set; }
        public int vendorOrgId { get; set; }
        public int CategoryId {  get; set; }
        public string JobType { get; set; }
        public string Level { get; set; }
        public int VacancyNo { get; set; }
        public string EmployeeType { get; set; }
        public string JobLocation { get; set; }
        public string OfferedSalary {  get; set; }
        public DateTime DeadLine { get; set; }
        public string EducationLevel { get; set; }
        public string ExperienceRequired { get; set; }
        public string OtherSpecification { get; set; }
        public string JobWorkDescription { get; set; }

    }
}
