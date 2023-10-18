using ApplicationManager_ClassLibrary.Entitys;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplicationManager.Models
{
    public class ContactsModel : PageModel
    {
        public IQueryable<Contacts> Contacts { get; set; }
        public string ImageUrl { get; set; }
        public IQueryable<SocialNet> Nets { get; set; }
        public string Name_page { get; set; }
    }
}
