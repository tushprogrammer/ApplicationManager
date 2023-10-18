using ApplicationManager.ContextFolder;
using ApplicationManager_ClassLibrary.Entitys;
using ApplicationManager.Models;
using Microsoft.AspNetCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;
using System.Web.Mvc;

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
        [HttpGet]
        public IQueryable<MainPage> GetMains()
        {
            //all
            return Context.MainPage;
        }
        [HttpGet]
        public MainForm GetMainsIndexPage()
        {
            // Butt_main, Title, Image_main, Main_request
            IQueryable<MainPage> data = Context.MainPage.Where(item => item.Id >= 6 && item.Id <= 9);
            MainForm form = new()
            {
                ButtonTitle = data.First(i => i.Id == 6).Value,
                Title = data.First(i => i.Id == 7).Value,
                UrlImage = data.First(i => i.Id == 8).Value,
                RequestTitle = data.First(i => i.Id == 6).Value,
            };
            return form;
        }
        [HttpGet]
        public IQueryable<MainPage> GetMainsHeader()
        {
            //Services, Project, Blogs, Contacts
            return Context.MainPage.Where(item => item.Id >= 2 && item.Id <= 5);
        }
        [HttpGet]
        public IQueryable<MainPage> GetMainsAdmin()
        {
            //MainAdmin, ProjectAdmin, ServicesAdmin, BlogsAdmin, ContactsAdmin, Index

            return Context.MainPage.Where(i => i.Id >= 13 && i.Id <= 18);
        }
        [HttpPost]
        public void SaveNamePages(List<MainPage> names, List<MainPage> NamesAdmin)
        {
            if (names.Count > 0 && NamesAdmin.Count > 0)
            {
                for (int i = 0; i < names.Count; i++)
                {
                    MainPage name_admin = NamesAdmin.Find(item => item.Name.Contains(names[i].Name));
                    name_admin.Value = $"Ред. \"{names[i].Value}\"";
                }
                StringBuilder queryBuilder = new StringBuilder();

                for (int i = 0; i < names.Count; i++)
                {
                    queryBuilder.Append($"UPDATE MainPage SET Value = CASE Id WHEN {names[i].Id} THEN N'{names[i].Value}' ELSE Value END;");
                }
                for (int i = 0; i < NamesAdmin.Count; i++)
                {
                    queryBuilder.Append($"UPDATE MainPage SET Value = CASE Id WHEN {NamesAdmin[i].Id} THEN N'{NamesAdmin[i].Value}' ELSE Value END;");
                }


                var query = queryBuilder.ToString();
                var rowsModified = Context.Database.ExecuteSqlRaw(query);
            }
        }
    
        public void EditMain(MainForm data, IFormFile Image)
        {
            
            //если картинку в этот раз не захотели поменять
            if (Image != null)
            {
                //путь к папке images на сервере
                string uploadPath =
                Path.Combine(webHost.WebRootPath, "Images");
                string UniqueName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                string FilePath = Path.Combine(uploadPath, UniqueName);
                Image.CopyTo(new FileStream(FilePath, FileMode.Create));
                //сохранение новых заголовков
                //(изначально хотел чтоб делался через 1 запрос, во избежании многократного обращения к бд)
                //так как update не работает с union,
                //а так как у меня постоянно разные источники данных,
                //то и запросы придется делать несколько раз
                //буква N перед данными - это позволение сохранять русский текст
                var rowsModified = Context.Database.ExecuteSqlRaw(
                   $"UPDATE MainPage SET Value = N'{data.ButtonTitle}' WHERE Id = 6");
                rowsModified = Context.Database.ExecuteSqlRaw(
                    $"UPDATE MainPage SET Value = N'{data.Title}' WHERE Id = 7");
                rowsModified = Context.Database.ExecuteSqlRaw(
                    $"UPDATE MainPage SET Value = N'{UniqueName}' WHERE Id = 8");
                rowsModified = Context.Database.ExecuteSqlRaw(
                    $"UPDATE MainPage SET Value = N'{data.RequestTitle}' WHERE Id = 9");
            }
            else
            {
                var rowsModified = Context.Database.ExecuteSqlRaw(
                    $"UPDATE MainPage SET Value = N'{data.ButtonTitle}' WHERE Id = 6");
                rowsModified = Context.Database.ExecuteSqlRaw(
                    $"UPDATE MainPage SET Value = N'{data.Title}' WHERE Id = 7");
                rowsModified = Context.Database.ExecuteSqlRaw(
                    $"UPDATE MainPage SET Value = N'{data.RequestTitle}' WHERE Id = 9");
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
            //Request new_request = new()
            //{
            //    DateCreated = NewRequest.DateCreated,
            //    Email = NewRequest.Email,
            //    Fullname = NewRequest.Fullname,
            //    Status = NewRequest.Status,
            //    StatusId = NewRequest.StatusId,
            //    Textrequest = NewRequest.Textrequest
            //};
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
            return Context.Projects.FirstOrDefault(i => i.Id == id);
        }
        public void AddProject(Project new_project, IFormFile Image)
        {
            if (new_project != null)
            {
                //сохранение нового проекта в бд
                
                string uploadPath =
                Path.Combine(webHost.WebRootPath, "Images");
                string UniqueName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                string FilePath = Path.Combine(uploadPath, UniqueName);
                Image.CopyTo(new FileStream(FilePath, FileMode.Create));
                new_project.ImageUrl = UniqueName;
                
                Context.Projects.Add(new_project);
                Context.SaveChanges();
            }
        }
        public void AddProject(Project new_project)
        {
            if (new_project != null)
            {
                new_project.ImageUrl = "/Default/default.png"; //имя по умолчанию
                Context.Projects.Add(new_project);
                Context.SaveChanges();
            }
        }
        public void EditProject(Project edit_project, IFormFile Image)
        {
            if (edit_project != null)
            {
                string uploadPath =
                Path.Combine(webHost.WebRootPath, "Images");
                string UniqueName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                string FilePath = Path.Combine(uploadPath, UniqueName);
                Image.CopyTo(new FileStream(FilePath, FileMode.Create));
                //сохранение новых заголовков
                var rowsModified = Context.Database.ExecuteSqlRaw(
                    $"UPDATE [Projects] SET Title = N'{edit_project.Title}', NameCompany = N'{edit_project.NameCompany}', " +
                    $" Description = N'{edit_project.Description}', ImageUrl = N'{UniqueName}' WHERE Id = {edit_project.Id}");
            }
        }
        public void EditProject(Project edit_project)
        {
            if (edit_project != null)
            {
                var rowsModified = Context.Database.ExecuteSqlRaw(
                       $"UPDATE [Projects] SET Title = N'{edit_project.Title}', NameCompany = N'{edit_project.NameCompany}', " +
                       $" Description = N'{edit_project.Description}' WHERE Id = {edit_project.Id}");
            }
        }
        public void DeleteProject(int id)
        {
            Context.Projects.Remove(GetProject(id));
            Context.SaveChanges();
        }
        public IQueryable<Service> GetServices()
        {
            return Context.Services;
        }
        public Service GetService(int id)
        {
            return Context.Services.First(i => i.Id == id);
        }
        public void AddService(Service newService)
        {
            Context.Services.Add(newService);
            Context.SaveChanges();
        }
        public void DeleteService(int id)
        {
            Context.Services.Remove(GetService(id));
            Context.SaveChanges();
        }
        public void EditService(Service model)
        {
            var rowsModified = Context.Database.ExecuteSqlRaw(
                  $"UPDATE [Services] SET Title = N'{model.Title}', " +
                  $" Description = N'{model.Description}' WHERE Id = {model.Id}");
        }
        public IQueryable<Blog> GetBlogs()
        {
            return Context.Blogs;
        }
        public Blog GetBlog(int id)
        {
            return Context.Blogs.FirstOrDefault(i => i.Id == id);
        }
        public void AddBlog(Blog new_blog)
        {
            if (new_blog != null)
            {
                new_blog.ImageUrl = "/Default/default.png"; //имя по умолчанию
                Context.Blogs.Add(new_blog);
                Context.SaveChanges();
            }
        }
        public void AddBlog(Blog new_blog, IFormFile Image)
        {
            if (new_blog != null)
            {
                string uploadPath =
                        Path.Combine(webHost.WebRootPath, "Images");
                string UniqueName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                string FilePath = Path.Combine(uploadPath, UniqueName);
                Image.CopyTo(new FileStream(FilePath, FileMode.Create));
                new_blog.ImageUrl = UniqueName;
                Context.Blogs.Add(new_blog);
                Context.SaveChanges();
            }
        }
        public void EditBlog(Blog edit_blog)
        {
            if (edit_blog != null)
            {
                var rowsModified = Context.Database.ExecuteSqlRaw(
                    $"UPDATE [Blogs] SET Title = N'{edit_blog.Title}', " +
                    $" Description = N'{edit_blog.Description}' WHERE Id = {edit_blog.Id}");
            }
        }
        public void EditBlog(Blog edit_blog, IFormFile Image)
        {
            if (edit_blog != null)
            {
                string uploadPath =
                    Path.Combine(webHost.WebRootPath, "Images");
                string UniqueName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                string FilePath = Path.Combine(uploadPath, UniqueName);
                Image.CopyTo(new FileStream(FilePath, FileMode.Create));

                var rowsModified = Context.Database.ExecuteSqlRaw(
                   $"UPDATE [Blogs] SET Title = N'{edit_blog.Title}', " +
                   $" Description = N'{edit_blog.Description}', ImageUrl = N'{UniqueName}' WHERE Id = {edit_blog.Id}");
            }
        }

        public void DeleteBlog(int id)
        {
            Context.Blogs.Remove(GetBlog(id));
            Context.SaveChanges();
        }
        public IQueryable<Contacts> GetContacts()
        {
            return Context.Contacts;
        }
        public IQueryable<SocialNet> GetSocialNet()
        {
            return Context.SocialNets;
        }
        public void SaveContacts(List<Contacts> edit_contacts, List<SocialNet> edit_socialNets, IFormFile Image)
        {
            //чтоб выполнить сохранение изменений, надо:
            //найти все элементы, которые есть еще в контексте и поменять их значения, по id, если они были изменены
            //всех, кого не стало, удалить из контекста
            //всех, кого не было, добавить
            //но это придется 2 раз обращаться к бд для получения исходных данных,
            //а после сравнения еще 6 раз запросами на изменение
            //или
            //заменить полностью коллекции внутри контекста
            //как показала практика, низя тупо заменить таблицы, хы
            //а если попробовать сначала полносью опустошить таблицы, а потом их заполнить новыми объектами?
            //сохранение идет с привязкой к именам новых файлов
            
            if (Image != null)
            {
                string uploadPath =
                Path.Combine(webHost.WebRootPath, "Images");
                string UniqueName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                string FilePath = Path.Combine(uploadPath, UniqueName);
                Image.CopyTo(new FileStream(FilePath, FileMode.Create));

                var rowsModified = Context.Database.ExecuteSqlRaw(
                   $"UPDATE [Contacts] SET Description = N'{UniqueName}' WHERE Id = 1");

            }
            var oldContacts = Context.Contacts;
            var oldSocialNets = Context.SocialNets;
            Context.SocialNets.RemoveRange(oldSocialNets);
            Context.Contacts.RemoveRange(oldContacts.Where(i => i.Id != 1 )); //удалить все элементы в таблице кроме первой картинки
            

            Context.SocialNets.AddRange(edit_socialNets);
            Context.Contacts.AddRange(edit_contacts);

            Context.SaveChanges();

            //перед сохранением осталось загрузить в форму картинку адреса, прислать сюда, и закинуть в таблицу Contacts
            //Context.SaveChanges();
        }
        public void SaveNewFiles(List<IFormFile> files)
        {
            //загрузка всех новых файлов на сервер
            if (files != null)
            {
                foreach (IFormFile item in files)
                {
                    string uploadPath =
                    Path.Combine(webHost.WebRootPath, "Images");
                    string UniqueName = Guid.NewGuid().ToString() + "_" + item.FileName;
                    string FilePath = Path.Combine(uploadPath, UniqueName);
                    item.CopyTo(new FileStream(FilePath, FileMode.Create));
                }
     
            }
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

        public void SaveNewStatusRequest(Request reqChange)
        {
            Request requestNow = Context.Requests.First(i => i.Id == reqChange.Id);
            requestNow.StatusId = reqChange.StatusId;
            Context.SaveChanges();
        }

    }
}
