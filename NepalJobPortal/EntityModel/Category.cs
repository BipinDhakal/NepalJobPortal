using System.ComponentModel.DataAnnotations;

namespace NepalJobPortal.EntityModel
{
    public class Category : Common
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
