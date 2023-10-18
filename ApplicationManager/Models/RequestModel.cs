using ApplicationManager.Entitys;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApplicationManager.Models
{
    public class RequestModel
    {
        private int id;
        public int Id { get { return id; } set { id = value; } }
        private DateTime dateCreated;
        public DateTime DateCreated { get { return dateCreated; } set { dateCreated = value; } }

        private string fullname;
        [Required]
        public string Fullname { get { return fullname; } set { fullname = value; } }

        private string email;
        [Required]
        public string Email { get { return email; } set { email = value; } }

        private string textrequest;
        [Required]
        public string Textrequest { get { return textrequest; } set { textrequest = value; } }

        [ForeignKey("StatusRequest")]
        public int StatusId { get; set; }

        private StatusRequest status;
        public StatusRequest? Status { get { return status; } set { status = value; } }
    }
}
