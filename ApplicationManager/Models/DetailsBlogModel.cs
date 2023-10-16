using System.ComponentModel.DataAnnotations;

namespace ApplicationManager.Models
{
    public class DetailsBlogModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        //public DateTime Created { get; set; } //не надо передавать, так как она не изменяется никогда
        public IFormFile? Image { get; set; }
    }
}
