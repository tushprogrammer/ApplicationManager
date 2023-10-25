using ApplicationManager.Models;
using Microsoft.EntityFrameworkCore.Query;
using ApplicationManager_ClassLibrary.Entitys;

namespace ApplicationManager.Data
{
    public interface IAppData
    {
        public IQueryable<MainPage> GetMains();
        public MainPageUploadModel GetMainsIndexPage();
        public IQueryable<MainPage> GetMainsAdmin();
        public IQueryable<MainPage> GetMainsHeader();
        public ProjectsModel GetProjects();
        public IQueryable<Service> GetServices();
        public BlogsModel GetBlogs();
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
        public void SaveNewStatusRequest(Request reqNow);
        public void EditMain(MainForm form, IFormFile image);
        public void AddProject(Project new_project, IFormFile image); 
        public ProjectModel GetProjectModel(int id);
        public BlogModel GetBlogModel(int id);
        public Project GetProject(int id);
        public void EditProject(Project edit_project, IFormFile image);
        public void DeleteProject(int id);
        public Service GetService(int id);
        public void AddService(Service newService);
        public void DeleteService(int id);
        public void EditService(Service service);
        public Blog GetBlog(int id);
        public void AddBlog(Blog new_blog, IFormFile image);
        public void EditBlog(Blog edit_blog, IFormFile image);
        public void DeleteBlog(int id);
        public void SaveContacts(List<Contacts> contacts, List<SocialNet> socialNets, IFormFile Image);
        public void SaveNewImageSocialNets(List<IFormFile> files);
        public void SaveNamePages(List<MainPage> names, List<MainPage> NamesAdmin);
    }
}
