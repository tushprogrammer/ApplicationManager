using ApplicationManager_ClassLibrary.Entitys;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplicationManager.Models
{
    public class BlogsModel
    {
        public List<Blog_with_image> Blogs { get; set; }
        public string Name_page { get; set; }
    }
}
