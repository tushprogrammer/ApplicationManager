using ApplicationManager.Data;
using ApplicationManager.Entitys;
using ApplicationManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ApplicationManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAppData data;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IAppData Data)
        {
            _logger = logger;
            data = Data;
        }

        public IActionResult Index()
        {
            //что мне надо вывести на страницу индекс:
            //3 элемента - картинка, кнопка и заголовок поверх картинки
            //все это хранится в таблице MainPage, то есть надо в интерфейсе IAppData реализовать получение этих трех товарищей, без лишнего
            IQueryable<MainPage> mainPages = data.GetMains().Where(item => item.Id >= 6 && item.Id <= 9);
            return View(mainPages);
        }
        public IActionResult AddRequest() 
        {
            //на страницу добавление надо вывести заголовок id == 9
            MainPage Title = data.GetMainRequest();
            return View(Title);
        }
        public IActionResult AddNewRequest(string Name, string Email, string Description) 
        {
            //тут же создать статус по умолчанию
            //StatusRequest status = new StatusRequest()
            //{
            //    Id = 1,
            //    StatusName = "Получена"
            //};
            Request newRequest = new Request()
            {
                DateCreated = DateTime.Now,
                Email = Email,
                Fullname = Name,
                Textrequest = Description,
                StatusId = 1

            };
            data.AddRequest(newRequest);
            return Redirect("~/"); //возврат к первой странице
        }
        public IActionResult Project()
        {
            //подгрузка данных из бд: 
            //все проекты
            ProjectsModel model = new()
            {
                Projects = data.GetProjects(),
                Name_page = data.GetMains().First(i => i.Id == 1).Value
            };
            //имя странциы проектов из тб mainpage
            return View(model);
        }
        public IActionResult ProjectDetails(int id) 
        {
            ProjectsModel model = new()
            {
                Projects = data.GetProjects(),
                Name_page = data.GetMains().First(i => i.Id == 1).Value,
                IdProject = id
            };
            return View(model);
        }
        public IActionResult Blogs() 
        {
            BlogsModel model = new()
            {
                Blogs = data.GetBlogs(),
                Name_page = data.GetMains().First(i => i.Id == 4).Value,
            };
            return View(model);
        }
        public IActionResult BlogDetails(int id)
        {
            BlogsModel model = new()
            {
                Blogs = data.GetBlogs(),
                Name_page = data.GetMains().First(i => i.Id == 4).Value,
                BlogId = id
            };
            return View(model);
        }
        public IActionResult Services() 
        {
            ServicesModel model = new()
            {
                Services = data.GetServices(),
                Name_page = data.GetMains().First(i => i.Id == 2).Value
            };
            return View(model);
        }
        public IActionResult Contacts()
        {
            ContactsModel model = new()
            {               
                Contacts = data.GetContacts().Where(i => i.Id != 7),
                ImageUrl = data.GetContacts().First(i => i.Id == 7).Description,
                Nets = data.GetSocialNet(),
                Name_page = data.GetMains().First(i => i.Id == 5).Value,
            };
            return View(model);
        }
        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}