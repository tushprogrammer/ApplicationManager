﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using ApplicationManager_ClassLibrary.Entitys;

namespace ApplicationManager.Models
{
    public class ServicesModel : PageModel
    {
        public IQueryable<Service> Services { get; set; }
        public string Name_page { get; set; }
    }
}
