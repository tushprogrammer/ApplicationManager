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
    }
}
