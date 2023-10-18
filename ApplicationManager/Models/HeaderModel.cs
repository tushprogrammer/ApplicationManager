using ApplicationManager_ClassLibrary.Entitys;

namespace ApplicationManager.Models
{
    public class HeaderModel
    {
        public string  Title { get; set; }
        public IQueryable<MainPage> MainPages { get; set; }
    }
}
