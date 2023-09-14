using ApplicationManager.Data;
using ApplicationManager.Entitys;
using ApplicationManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;
//using System.Web.HttpPostedFileWrapper;

namespace ApplicationManager.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAppData data;
        private readonly IWebHostEnvironment webHost;
        public AdminController(IAppData Data, IWebHostEnvironment WebHost)
        {
            webHost = WebHost;
            data = Data;
        }
       
       

        #region Заявки

        //все заявки
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            //на рабочий стол выводится информация о заявках
            //в зависимости от выборанной даты (сегодня, вчера, дата)
            //по умолчанию выводятся все заявки

            //пока что все, но тут должна быть сортировка в зависимтости от нажатия на кнопки (перегрузка метода вызова страницы)
            //+сортировка заявок по выбранному статусу
            //List<Request> requests = data.GetRequests().ToList(); 
            AdminModel model = new AdminModel()
            {
                Requests = data.GetRequests(), //все заявки
                Statuses = GetNameStatuses(),
                AllRequestsCount = data.CountRequests(),
            };
            return View("Index", model);
        }

        public IActionResult TodayRequests()
        {
            //вопрос по реализации: как будет лучше организовать запросы к бд по поводу заявок в сортированном виде,
            //на экран надо выводить отсортированные заявки и общее количество в бд
            //либо: сделать два запроса, на общее количество и конкретные заявки
            //либо сделать один запрос на все заявки, и полученную коллекцию посчитать и отбросить лишние 
            int count = data.CountRequests(); 
            AdminModel model = new AdminModel()
            {
                Requests = data.GetRequestsToday(), //заявки сегодня
                Statuses = GetNameStatuses(),
                AllRequestsCount = count,
            };
            return View("Index", model);
        }
        public IActionResult YesterdayRequests()
        {
            AdminModel model = new AdminModel()
            {
                Requests = data.GetRequestsYesterday(), //заявки вчера
                Statuses = GetNameStatuses(),
                AllRequestsCount = data.CountRequests(),
            };
            return View("Index", model);
        }
        public IActionResult WeekRequests()
        {
            AdminModel model = new AdminModel()
            {
                Requests = data.GetRequestsWeek(), //заявки на этой неделе
                Statuses = GetNameStatuses(),
                AllRequestsCount = data.CountRequests(),
            };
            return View("Index", model);
        }
        public IActionResult MonthRequests()
        {
            AdminModel model = new AdminModel()
            {
                Requests = data.GetRequestsMonth(), //заявки в этом месяце
                Statuses = GetNameStatuses(),
                AllRequestsCount = data.CountRequests(),
            };
            return View("Index", model);
        }
        //заявки по диапазону дат
        public IActionResult RangeDateRequests(DateTime DateFor, DateTime DateTo)
        {
            AdminModel model = new AdminModel()
            {
                Requests = data.GetRequests(DateFor, DateTo),
                Statuses = GetNameStatuses(),
                AllRequestsCount = data.CountRequests(),
            };
            return View("Index", model);
        }

        [HttpGet]
        public IActionResult StatusRequests(string statusName) //заявки по статусам
        {
            AdminModel model = new AdminModel()
            {
                Requests = data.GetRequestsStatus(statusName),
                Statuses = GetNameStatuses(),
                AllRequestsCount = data.CountRequests(),
            };
            return View("Index", model);
        }
        #endregion

        //выдача заявкам элемент статуса в дополнение к id
        private IQueryable<string> GetNameStatuses()
        {
            List<string> ListStatuses = new();
            IQueryable<StatusRequest> statusRequests = data.GetStatuses();
            foreach (var statusRequest in statusRequests)
            {
                ListStatuses.Add(statusRequest.StatusName);
            }
            return ListStatuses.AsQueryable();
        }
        //обновление статусов у заявок
        public IActionResult SaveNewStatusRequest(string RequestId, string StatusName)
        {
            Request reqNow = data.GetRequestsNow(RequestId);
            StatusRequest Status = data.GetStatuses().First(s => s.StatusName == StatusName);
            reqNow.StatusId = Status.Id;
            data.SaveNewRequest(reqNow);
            AdminModel model = new()
            {
                Requests = data.GetRequests(), //все заявки
                Statuses = GetNameStatuses(),
                AllRequestsCount = data.CountRequests(),
            };
            var r = Request.Headers["Referer"].ToString();
            return Redirect(r);
            //обновленме всего рабочего стола, а все настройки сортировки остаются
        }

        //вызов страницы просмотра перед редактированием
        public IActionResult MainAdmin()
        {
            IQueryable<MainPage> mainPages = data.GetMains().Where(item => item.Id >= 6 && item.Id <= 9);
            MainPageModel model = new()
            {
                Image_path = mainPages.First(i => i.Id == 8).Value,
                Title = mainPages.First(i => i.Id == 7).Value,
                ButtonTitle = mainPages.First(i => i.Id == 6).Value,
                RequestTitle = mainPages.First(i => i.Id == 9).Value,
            };
            
            return View(model);
        }

        //вызов страницы редактирования
        public IActionResult EditMain()
        {
            //сюда должны перебрасываться те же данные, что и выше
            //может все таки модель полноценную сделать ?
            //и в модели разбить по состовляющим
            //а то херня какая то получается, если еще на фронте искать через LINQ нужный элемент
            IQueryable<MainPage> mainPages = data.GetMains().Where(item => item.Id >= 6 && item.Id <= 9);
            MainPageUploadModel model = new()
            {
                Title = mainPages.First(i => i.Id == 7).Value,
                ButtonTitle = mainPages.First(i => i.Id == 6).Value,
                RequestTitle = mainPages.First(i => i.Id == 9).Value,
            };
            ViewBag.Image = mainPages.First(i => i.Id == 8).Value;
            return View(model);
        }

        //вызов метода редактирования 
        [HttpPost]
        public IActionResult EditMainSave(MainPageUploadModel model)
        {
            //всякое сохранение
            data.EditMain(model);            
            
            return Redirect("~/Admin/MainAdmin");
        }

        //вывод страницы для редактирования
        public IActionResult ProjectAdmin()
        {
            //все проекты
            ProjectsModel model = new()
            {
                Projects = data.GetProjects(),
                Name_page = data.GetMains().First(i => i.Id == 1).Value
            };
            //имя странциы проектов из тб mainpage
            return View(model);
        }
        //страница добавления нового проекта
        public IActionResult AddNewProject() 
        {
            //заголовок для шапки
            ViewBag.Name_page = data.GetMains().First(i => i.Id == 1).Value;
            return View();
        }
        //метод добавления нового проекта
        public IActionResult AddProjectMethod(DetailsProjectModel model)
        {
            //data логика добавления
            data.AddProject(model);
            return Redirect("~/Admin/ProjectAdmin");
        }
        //страница изменения проекта
        public IActionResult DetailsProject(int id)
        {
            Project temp = data.GetProject(id);
            ViewBag.ImageUrl = temp.ImageUrl;
            DetailsProjectModel model = new()
            {
                Id = id,
                Description = temp.Description,
                Title = temp.Title,
                NameCompany = temp.NameCompany,
            };
            return View(model);
        }
        //метод изменения проекта
        public IActionResult EditProjectMethod(DetailsProjectModel model)
        {
            data.EditProject(model);
            return Redirect("~/Admin/ProjectAdmin");
        }
        //метод удаления проекта
        public IActionResult DeleteProjectMethod(int id)
        {
            data.DeleteProject(id);
            return Redirect("~/Admin/ProjectAdmin");
        }
        //вызов страницы для редактирования
        public IActionResult ServicesAdmin()
        {
            ServicesModel model = new()
            {
                Services = data.GetServices(),
                Name_page = data.GetMains().First(i => i.Id == 2).Value
            };
            return View(model);
        }
        //изменение услуги
        public IActionResult EditService(int id)
        {
            Service model = data.GetService(id);
            ViewBag.Name_page = data.GetMains().First(i => i.Id == 2).Value;
            return View("DetailsService", model);
            
        }
        //добавление услуги
        public IActionResult AddNewService() 
        {
            ViewBag.Name_page = data.GetMains().First(i => i.Id == 2).Value;
            return View("DetailsService");
        }
        public IActionResult AddNewServiceMethod(Service newService)
        {
            data.AddService(newService);
            return Redirect("~/Admin/ServicesAdmin");
        }
        public IActionResult DeleteServiceMethod(int id)
        {
            data.DeleteService(id);
            return Redirect("~/Admin/ServicesAdmin");
        }
        public IActionResult EditServiceMethod(Service Service)
        {
            data.EditService(Service);
            return Redirect("~/Admin/ServicesAdmin");
        }
    }
}
