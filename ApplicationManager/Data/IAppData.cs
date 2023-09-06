using ApplicationManager.Entitys;
using Microsoft.EntityFrameworkCore.Query;

namespace ApplicationManager.Data
{
    public interface IAppData
    {
        public IQueryable<MainPage> GetMains();
        public IQueryable<Project> GetProjects();
        public IQueryable<Service> GetServices();
        public IQueryable<Blog> GetBlogs();
        public IQueryable<Contacts> GetContacts();
        public IQueryable<SocialNet> GetSocialNet();
        public MainPage GetMainRequest();
        public void AddRequest(Request request);
        
    }
}
