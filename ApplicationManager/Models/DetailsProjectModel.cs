
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ApplicationManager.Models
{
    public class DetailsProjectModel : PageModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string NameCompany { get; set; }
        [Required]
        public string Description { get; set; } 
        public IFormFile? Image { get; set; }

    }
}
