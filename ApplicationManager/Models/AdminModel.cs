using Microsoft.AspNetCore.Mvc.RazorPages;
using ApplicationManager_ClassLibrary.Entitys;

namespace ApplicationManager.Models
{
    public class AdminModel : PageModel
    {
       public IQueryable<Request> Requests { get; set; }
       public IQueryable<string> Statuses { get; set; }
       public int AllRequestsCount { get; set; }
    }
}