﻿namespace ApplicationManager.Models
{
    public class Project_with_image
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string NameCompany { get; set; }
        public string Description { get; set; }
        public byte[]? Image_byte { get; set; }
        public string? ImgSrc { get; set; }
        public string? Image_name { get; set; }
        public IFormFile? Image { get; set; }
    }
}
