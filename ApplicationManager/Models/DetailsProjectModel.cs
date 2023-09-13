
namespace ApplicationManager.Models
{
    public class DetailsProjectModel 
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string NameCompany { get; set; }
        public string Description { get; set; } 
        public IFormFile Image { get; set; }

    }
}
