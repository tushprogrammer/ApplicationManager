using System.ComponentModel.DataAnnotations;

namespace ApplicationManager.Models
{
    public class Blog_with_image
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string? Image_name { get; set; }
        public byte[]? Image_byte { get; set; }
        public string? ImgSrc { get; set; }
        public IFormFile? Image {  get; set; }
    }
}
