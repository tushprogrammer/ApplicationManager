using ApplicationManager.ContextFolder;
using ApplicationManager.Entitys;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualBasic;

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
        public IQueryable<MainTitle> GetMainTitles()
        {
            return Context.Titles;
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
        public IQueryable<Request> GetRequests()
        {
            List<Request> tempRequests = Context.Requests.ToList();
            SetStatusRequests(ref tempRequests);
            return tempRequests.AsQueryable();
        }
        public IQueryable<Request> GetRequestsStatus(string statusName)
        {
            List<Request> tempRequests;
            if (statusName != string.Empty && statusName != null)
            {
                StatusRequest Status = GetStatuses().First(s => s.StatusName == statusName);
                tempRequests = Context.Requests.Where(i => i.StatusId == Status.Id).ToList();
            }
            else
                tempRequests = Context.Requests.ToList();
            SetStatusRequests(ref tempRequests);
            return tempRequests.AsQueryable();
        }
        public IQueryable<Request> GetRequests(DateTime DateFor, DateTime DateTo)
        {
            List<Request> tempRequests = 
                Context.Requests.Where(i => i.DateCreated.Date >= DateFor.Date && i.DateCreated.Date <= DateTo.Date).ToList();
            SetStatusRequests(ref tempRequests);
            return tempRequests.AsQueryable();
        }
        public IQueryable<Request> GetRequestsToday()
        {
            List<Request> tempRequests = Context.Requests.Where(i => i.DateCreated.Date == DateTime.Today).ToList();
            SetStatusRequests(ref tempRequests);
            return tempRequests.AsQueryable();
        }
        public IQueryable<Request> GetRequestsYesterday()
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);
            List<Request> tempRequests = 
                Context.Requests.Where(i => i.DateCreated.Date == yesterday).ToList();
            SetStatusRequests(ref tempRequests);
            return tempRequests.AsQueryable();
        }
        public IQueryable<Request> GetRequestsWeek()
        {
            int daynow;
            if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                daynow = 7;
            else
                daynow = (int)DateTime.Today.DayOfWeek;
            DateTime firstdayweek = DateTime.Today.AddDays(-daynow+1);
            DateTime lastdayweek = firstdayweek.AddDays(7);
            List<Request> tempRequests = 
                Context.Requests.Where(i => i.DateCreated >= firstdayweek && i.DateCreated <= lastdayweek).ToList();
            SetStatusRequests(ref tempRequests);
            return tempRequests.AsQueryable();
        }
        public IQueryable<Request> GetRequestsMonth()
        {
            DateTime now = DateTime.Now;
            DateTime firstdaymonth = new DateTime(now.Year, now.Month, 1);
            DateTime lastdaymonth = new DateTime(now.Year, now.Month + 1, 1).AddDays(-1);
            List<Request> tempRequests =
                Context.Requests.Where(i => i.DateCreated >= firstdaymonth && i.DateCreated <= lastdaymonth).ToList();
            SetStatusRequests(ref tempRequests);
            return tempRequests.AsQueryable();
        }
        private List<Request> SetStatusRequests(ref List<Request> requests)
        {
            List<StatusRequest> StatusRequests = GetStatuses().ToList();

            foreach (Request item in requests)
            {
                item.Status = StatusRequests.First(i => i.Id == item.StatusId);
            }

            return requests;
        }
        public int CountRequests()
        {
            return  Context.Requests.Count();
        }

        public IQueryable<StatusRequest> GetStatuses()
        {
            return Context.Statuses;
        }
        public Request GetRequestsNow(string requestId)
        {
            return Context.Requests.First(i => i.Id.ToString() == requestId);
        }

        public void SaveNewRequest(Request reqChange)
        {
            Request requestNow = Context.Requests.First(i => i.Id == reqChange.Id);
            requestNow.StatusId = reqChange.StatusId;
            Context.SaveChanges();
        }
    }
}
