using ApplicationManager.Entitys;
using ApplicationManager.Models;
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
        public IQueryable<Request> GetRequests();
        public IQueryable<Request> GetRequests(DateTime DateFor, DateTime DateTo);
        public IQueryable<Request> GetRequestsStatus(string statusName);
        public IQueryable<Request> GetRequestsToday();
        public IQueryable<Request> GetRequestsYesterday();
        public IQueryable<Request> GetRequestsWeek();
        public IQueryable<Request> GetRequestsMonth();

        public IQueryable<MainTitle> GetMainTitles();
        public IQueryable<StatusRequest> GetStatuses();
        public MainPage GetMainRequest();
        public void AddRequest(Request request);
        public int CountRequests();
        public Request GetRequestsNow(string requestId);
        public void SaveNewRequest(Request reqNow);
        public void EditMain(MainPageUploadModel model);
        public void AddProject(DetailsProjectModel model);
        public Project GetProject(int id);
        public void EditProject(DetailsProjectModel model);
        public void DeleteProject(int id);
        public Service GetService(int id);
        public void AddService(Service newService);
        public void DeleteService(int id);
        public void EditService(Service service);
        public Blog GetBlog(int id);
        public void AddBlog(DetailsBlogModel newBlog);
        public void EditBlog(DetailsBlogModel blog);
        public void DeleteBlog(int id);
        public void SaveContacts(ContactsNewModel info);
        public void SaveNewFiles(List<IFormFile> files);
    }
}
