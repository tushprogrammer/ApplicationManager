using System.ComponentModel.DataAnnotations;

namespace ApplicationManager.Entitys
{
    //услуги
    public class Service
    {
        private int id;
        public int Id { get { return id; } set { id = value; } }

        private string title;

        [Required]
        public string Title { get { return title; } set { this.title = value; } }

        private string description;

        [Required]
        public string Description { get { return description; } set { this.description = value; } }


    }
}
