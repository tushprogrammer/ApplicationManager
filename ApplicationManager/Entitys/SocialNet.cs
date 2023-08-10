namespace ApplicationManager.Entitys
{
    //социальная сеть
    public class SocialNet
    {
        private int id;
        public int Id { get { return id; } set { id = value; } }

        //ссылка на картинку в бд
        private string imageurl;
        public string ImageUrl { get { return imageurl; } set { imageurl = value; } }

        //ссылка на соц. сеть
        private string url;
        public string Url { get { return url; } set { url = value; } }
    }
}
