using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationManager.Entitys
{
    //заявка
    public class Request
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        private int id;
        public int Id { get { return id; } set { id = value; } }

        private string fullname;
        public string Fullname { get { return fullname; } set {  fullname = value; } }

        private string email;
        public string Email { get { return email; } set { email = value; } }

        private string textrequest;
        public string Textrequest { get { return textrequest; } set { textrequest = value; } }

        [ForeignKey("StatusRequest")]
        public int StatusId { get; set; }

        //private StatusRequest status;
        //public StatusRequest Status { get { return status; } set { status = value; } }
    }
}
