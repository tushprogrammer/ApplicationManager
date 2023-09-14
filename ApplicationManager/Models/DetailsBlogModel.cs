namespace ApplicationManager.Models
{
    public class DetailsBlogModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        //public DateTime Created { get; set; } //не надо передавать, так как она не изменяется никогда
        public IFormFile Image { get; set; }
    }
}
