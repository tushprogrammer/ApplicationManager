using ApplicationManager.Entitys;

namespace ApplicationManager.Models
{
    public class HeaderModel
    {
        public string  Title { get; set; }
        public IQueryable<MainPage> MainPages { get; set; }
    }
}
