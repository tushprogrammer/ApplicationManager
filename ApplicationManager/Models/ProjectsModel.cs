using ApplicationManager_ClassLibrary.Entitys;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplicationManager.Models
{
    public class ProjectsModel
    {
        public List<Project_with_image> Projects { get; set; }
        public string Name_page { get; set; }

    }
}
