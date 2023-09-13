namespace ApplicationManager.Models
{
    public class MainPageUploadModel
    {
        public IFormFile Image { get; set; }
        public string Title { get; set; }
        public string RequestTitle { get; set; }
        public string ButtonTitle { get; set; }
    }
}
