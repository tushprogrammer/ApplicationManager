using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplicationManager.Models
{
    public class MainPageModel : PageModel
    {
       public string Image_path { get; set; }
       public string Title { get; set; }
       public string RequestTitle { get; set; }
       public string ButtonTitle { get; set; }
    }
}
