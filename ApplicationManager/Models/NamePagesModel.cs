using ApplicationManager_ClassLibrary.Entitys;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationManager.Models
{
    public class NamePagesModel
    {
        public string Name_page { get; set; }

        [BindProperty]
        [FromForm]
        public List<MainPage> Names { get; set; }
        [BindProperty]
        [FromForm]
        public List<MainPage> Names_admin { get; set; }
    }
}
