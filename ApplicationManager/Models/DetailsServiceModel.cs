﻿using System.ComponentModel.DataAnnotations;

namespace ApplicationManager.Models
{
    public class DetailsServiceModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }


        [Required]
        public string Description { get; set; }
        public string Name_page { get; set; }
        public bool is_edit { get; set; }
    }
}
