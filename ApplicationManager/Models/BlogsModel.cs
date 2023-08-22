using ApplicationManager.Entitys;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplicationManager.Models
{
    public class BlogsModel :PageModel
    {
        public IQueryable<Blog> Blogs { get; set; }
        public string Name_page { get; set; }
        public int BlogId { get; set; }
    }
}
