namespace ApplicationManager.Entitys
{
    //статус заявки
    public class StatusRequest
    {
        private int id;
        public int Id { get { return id; } set { id = value; } }

        private string status_name;
        public string StatusName { get { return status_name; }  set { this.status_name = value; } }
    }
}
