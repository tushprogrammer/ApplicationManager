using ApplicationManager.ContextFolder;
using ApplicationManager.Entitys;
using Microsoft.EntityFrameworkCore.Query;

namespace ApplicationManager.Data
{
    public class AppData : IAppData
    {
        private readonly ApplicationDbContext Context;
        public AppData(ApplicationDbContext context)
        {
            Context = context;
        }
        public IQueryable<MainPage> GetMains()
        {
            return Context.MainPage;
        }
        public MainPage GetMainRequest()
        {
            return Context.MainPage.First(item => item.Id == 9);
        }
        public void AddRequest(Request NewRequest)
        {

            Context.Requests.Add(NewRequest);
            //Context.ChangeTracker.DetectChanges();
            Context.SaveChanges();
        }
        public IQueryable<Project> GetProjects()
        {
            return Context.Projects;
        }
        public IQueryable<Service> GetServices()
        {
            return Context.Services;
        }
        public IQueryable<Blog> GetBlogs()
        {
            return Context.Blogs;
        }
        public IQueryable<Contacts> GetContacts()
        {
            return Context.Contacts;
        }
        public IQueryable<SocialNet> GetSocialNet()
        {
            return Context.SocialNets;
        }
    }
}
