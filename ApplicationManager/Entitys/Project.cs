using Microsoft.AspNetCore.Http.Features;

namespace ApplicationManager.Entitys
{
    //проекты
    public class Project
    {
        private int id;
        public int Id { get { return id; } set { id = value; } }

        private string title;
        public string Title { get { return title; } set { title = value; } }

        //имя компании, для отображения в линейке истории 
        private string namecompany;
        public string NameCompany { get {  return namecompany; } set {  namecompany = value; } }

        private string imageurl;
        public string ImageUrl { get {  return imageurl; } set {  imageurl = value; } }

        private string description;
        public string Description { get { return description; } set { description = value; } }

    }
}
