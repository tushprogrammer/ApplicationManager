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

        #region стр. "Главная"

        //Вызов страницы "Главная"
        public IActionResult Index()
        {
            IQueryable<MainPage> mainPages = data.GetMainsIndexpage();
            return View(mainPages);
        }
        //вызов верстки "Отправить заявку"
        public IActionResult AddRequest()
        {
            ViewBag.Title = data.GetMainRequest().Value;
            return PartialView();
        }
        //метод добавления заявки
        public IActionResult AddNewRequest(Request model)
        {
            if (ModelState.IsValid)
            {
                model.DateCreated = DateTime.Now;
                model.StatusId = 1;
                data.AddRequest(model);
                return Redirect("~/"); //возврат к первой странице
            }
            else
            {
                return Redirect("~/");
            }
        }
        #endregion

        #region стр. "Проекты"

        //вызов страницы "Проекты"
        public IActionResult Project()
        {
            //подгрузка данных из бд: 
            //все проекты
            ProjectsModel model = new()
            {
                Projects = data.GetProjects(),
                Name_page = data.GetMains().First(i => i.Id == 3).Value
            };
            //имя странциы проектов из тб mainpage
            return View(model);
        }
        //вызов страницы об конкретном проекте
        public IActionResult ProjectDetails(int id) 
        {
            ProjectsModel model = new()
            {
                Projects = data.GetProjects(),
                Name_page = data.GetMains().First(i => i.Id == 3).Value,
                IdProject = id
            };
            return View(model);
        }
        #endregion

        #region стр. "Блог"

        //вызов страницы "Блог"
        public IActionResult Blogs() 
        {
            BlogsModel model = new()
            {
                Blogs = data.GetBlogs(),
                Name_page = data.GetMains().First(i => i.Id == 4).Value,
            };
            return View(model);
        }
        //вызов страницы об конкретном блоге
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
        #endregion

        #region стр. "Услуги"

        //вызов страницы "Услуги"
        public IActionResult Services() 
        {
            ServicesModel model = new()
            {
                Services = data.GetServices(),
                Name_page = data.GetMains().First(i => i.Id == 2).Value
            };
            return View(model);
        }
        #endregion

        #region стр. "Контакты"

        //вызов страницы "Контакты"
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
        #endregion
    }
}