using ApplicationManager_ClassLibrary.Entitys;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ApplicationManager.Models
{
    public class ContactsModel : PageModel
    {
        public List<Contacts> Contacts { get; set; }
        public string? Image_name { get; set; }
        public byte[]? Image_byte { get; set; }
        public string? ImgSrc { get; set; }

        public List<SocialNet_with_image> Nets { get; set; }
        public string Name_page { get; set; }
    }
}
