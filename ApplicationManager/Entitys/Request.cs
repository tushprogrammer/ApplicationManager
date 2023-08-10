namespace ApplicationManager.Entitys
{
    //заявка
    public class Request
    {
        private int id;
        public int Id { get { return id; } set { id = value; } }

        private string fullname;
        public string Fullname { get { return fullname; } set {  fullname = value; } }

        private string email;
        public string Email { get { return email; } set { email = value; } }

        private string textrequest;
        public string Textrequest { get { return textrequest; } set { textrequest = value; } }

        private StatusRequest status;
        public StatusRequest Status { get { return status; } set { status = value; } }
    }
}
