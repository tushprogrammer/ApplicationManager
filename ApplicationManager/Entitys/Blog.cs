namespace ApplicationManager.Entitys
{
    //блог
    public class Blog
    {
        private int id;
        public int Id { get { return id; } set { id = value; } }

        private string title;
        public string Title { get { return title; } set { title = value; } }

        private DateTime created;
        public DateTime Created { get { return created; } }

        private string imageurl;
        public string ImageUrl { get { return imageurl; } set { imageurl = value; } }

        private string description;
        public string Description { get { return description; } set { description = value; } }
    }
}
