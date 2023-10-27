using ApplicationManager.Models;
using Microsoft.EntityFrameworkCore.Query;
using ApplicationManager_ClassLibrary.Entitys;

namespace ApplicationManager.Data
{
    public interface IAppData
    {
        public  Task<IQueryable<MainPage>> GetMainsAsync();
        public Task<MainPageUploadModel> GetMainsIndexPageAsync();
        public Task<IQueryable<MainPage>> GetMainsAdminAsync();
        public Task<IQueryable<MainPage>> GetMainsHeaderAsync();
        public Task<ProjectsModel> GetProjectsAsync();
        public Task<IQueryable<Service>> GetServicesAsync();
        public Task<BlogsModel> GetBlogsAsync();
        public Task<IQueryable<Contacts>> GetContactsAsync();
        public Task<ContactsModel> GetContactsModelAsync();
        public Task<IQueryable<SocialNet_with_image>> GetSocialNetAsync();
        public Task<IQueryable<Request>> GetRequestsAsync();
        public Task<IQueryable<Request>> GetRequestsAsync(DateTime DateFor, DateTime DateTo);
        public Task<IQueryable<Request>> GetRequestsStatusAsync(string statusName);
        public Task<IQueryable<Request>> GetRequestsTodayAsync();
        public Task<IQueryable<Request>> GetRequestsYesterdayAsync();
        public Task<IQueryable<Request>> GetRequestsWeekAsync();
        public Task<IQueryable<Request>> GetRequestsMonthAsync();

        public Task<IQueryable<MainTitle>> GetMainTitlesAsync();
        public Task<IQueryable<StatusRequest>> GetStatusesAsync();
        public Task<MainPage> GetMainRequestAsync();
        public Task AddRequest(Request request);
        public Task<int> CountRequestsAsync();
        public Task<Request> GetRequestsNowAsync(string requestId);
        public Task SaveNewStatusRequest(Request reqNow);
        public Task EditMain(MainForm form, IFormFile image);
        public Task AddProject(Project new_project, IFormFile image); 
        public Task<ProjectModel> GetProjectModelAsync(int id);
        public Task<BlogModel> GetBlogModelAsync(int id);
        public Task<Project> GetProjectAsync(int id);
        public Task EditProject(Project edit_project, IFormFile image);
        public Task DeleteProject(int id);
        public Task<DetailsServiceModel> GetServiceModelAsync(int id);
        public Task AddService(Service newService);
        public Task DeleteService(int id);
        public Task EditService(Service service);
        public Task<Blog> GetBlogAsync(int id);
        public Task AddBlog(Blog new_blog, IFormFile image);
        public Task EditBlog(Blog edit_blog, IFormFile image);
        public Task DeleteBlog(int id);
        public Task SaveContacts(List<Contacts> contacts, IFormFile Image);
        public Task SaveSocialNets(List<IFormFile> files, List<SocialNet_with_image> socialNets);
        public Task SaveNamePages(List<MainPage> names, List<MainPage> NamesAdmin);
        public Task<bool> Authenticate(string login, string password);
    }
}
