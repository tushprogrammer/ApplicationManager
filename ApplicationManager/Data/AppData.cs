using ApplicationManager.ContextFolder;
using ApplicationManager.Entitys;
using ApplicationManager.Models;
using Microsoft.AspNetCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ApplicationManager.Data
{
    public class AppData : IAppData
    {
        private readonly ApplicationDbContext Context;
        private readonly IWebHostEnvironment webHost;
        public AppData(ApplicationDbContext context, IWebHostEnvironment WebHost)
        {
            webHost = WebHost;
            Context = context;
        }
        public IQueryable<MainPage> GetMains()
        {
            return Context.MainPage;
        }
        
        public void EditMain(MainPageUploadModel model)
        {
            // путь к папке images 
            
            //если картинку в этот раз не захотели поменять
            if (model.Image != null)
            {
                string uploadPath =
                Path.Combine(webHost.WebRootPath, "Images");
                string UniqueName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string FilePath = Path.Combine(uploadPath, UniqueName);
                model.Image.CopyTo(new FileStream(FilePath, FileMode.Create));
                //сохранение новых заголовков
                //(изначально хотел чтоб делался через 1 запрос, во избежании многократного обращения к бд)
                //так как update не работает с union,
                //а так как у меня постоянно разные источники данных,
                //то и запросы придется делать несколько раз
                //буква N перед данными - это позволение сохранять русский текст
                var rowsModified = Context.Database.ExecuteSqlRaw(
                   $"UPDATE MainPage SET Value = N'{model.ButtonTitle}' WHERE Id = 6");
                rowsModified = Context.Database.ExecuteSqlRaw(
                    $"UPDATE MainPage SET Value = N'{model.Title}' WHERE Id = 7");
                rowsModified = Context.Database.ExecuteSqlRaw(
                    $"UPDATE MainPage SET Value = N'{UniqueName}' WHERE Id = 8");
                rowsModified = Context.Database.ExecuteSqlRaw(
                    $"UPDATE MainPage SET Value = N'{model.RequestTitle}' WHERE Id = 9");
            }
            else
            {
                var rowsModified = Context.Database.ExecuteSqlRaw(
                    $"UPDATE MainPage SET Value = N'{model.ButtonTitle}' WHERE Id = 6");
                rowsModified = Context.Database.ExecuteSqlRaw(
                    $"UPDATE MainPage SET Value = N'{model.Title}' WHERE Id = 7");
                rowsModified = Context.Database.ExecuteSqlRaw(
                    $"UPDATE MainPage SET Value = N'{model.RequestTitle}' WHERE Id = 9");
            }
            /*Context.SaveChanges();*/ //эта штука нужна, если идет изменение через сам контекст, а тут sql
                
            
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
        public Project GetProject(int id)
        {
            return Context.Projects.First(i => i.Id == id);
        }
        public void AddProject(DetailsProjectModel model)
        {
            //проверка на валидацию
            //сохранение нового проекта в бд
            if (model.Image != null)
            {
                string uploadPath =
                Path.Combine(webHost.WebRootPath, "Images");
                string UniqueName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string FilePath = Path.Combine(uploadPath, UniqueName);
                model.Image.CopyTo(new FileStream(FilePath, FileMode.Create));
                Project newproject = new()
                {
                    Title = model.Title,
                    NameCompany = model.NameCompany,
                    Description = model.Description,
                    ImageUrl = UniqueName
                };
                Context.Projects.Add(newproject);
                Context.SaveChanges();
            }
        }
        public void EditProject(DetailsProjectModel model)
        {
            
            if (model.Image != null)
            {
                string uploadPath =
                Path.Combine(webHost.WebRootPath, "Images");
                string UniqueName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string FilePath = Path.Combine(uploadPath, UniqueName);
                model.Image.CopyTo(new FileStream(FilePath, FileMode.Create));
                //сохранение новых заголовков

                var rowsModified = Context.Database.ExecuteSqlRaw(
                   $"UPDATE [Projects] SET Title = N'{model.Title}', NameCompany = N'{model.NameCompany}', " +
                   $" Description = N'{model.Description}', ImageUrl = N'{UniqueName}' WHERE Id = {model.Id}");
       
            }
            else
            {
                var rowsModified = Context.Database.ExecuteSqlRaw(
                   $"UPDATE [Projects] SET Title = N'{model.Title}', NameCompany = N'{model.NameCompany}', " +
                   $" Description = N'{model.Description}' WHERE Id = {model.Id}");

            }
        }
        public void DeleteProject(int id)
        {
            Project project = GetProject(id);
            Context.Projects.Remove(project);
            Context.SaveChanges();
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
