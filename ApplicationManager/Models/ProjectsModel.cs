using ApplicationManager.Entitys;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplicationManager.Models
{
    public class ProjectsModel : PageModel
    {
        public IQueryable<Project> Projects { get; set; }
        public string Name_page { get; set; }
        public int IdProject { get; set; }

    }
}
