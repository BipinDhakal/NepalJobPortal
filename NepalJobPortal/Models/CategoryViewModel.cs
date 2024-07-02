namespace NepalJobPortal.Models
{
    public class CategoryViewModel : CommonViewModel
    {
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }

        public List<CategoryViewModel> CategoryList { get; set; }

        public CategoryViewModel()
        {
            CategoryList = new List<CategoryViewModel>();
        }
    }
}
