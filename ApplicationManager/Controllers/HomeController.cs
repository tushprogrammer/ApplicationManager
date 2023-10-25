using ApplicationManager.Data;
using ApplicationManager_ClassLibrary.Entitys;
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
            MainPageUploadModel model = data.GetMainsIndexPage();
            return View(model);
        }
        //вызов верстки "Отправить заявку"
        public IActionResult AddRequest()
        {
            ViewBag.Title = data.GetMainRequest().Value;
            return PartialView();
        }
        //метод добавления заявки
        public IActionResult AddNewRequest(RequestModel model)
        {
            if (ModelState.IsValid)
            {
                Request new_req = new Request()
                {
                    DateCreated = DateTime.Now,
                    StatusId = 1,
                    Email = model.Email,
                    Fullname = model.Fullname,
                    Textrequest = model.Textrequest,
                };
                data.AddRequest(new_req);
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
            ProjectsModel model = data.GetProjects();
            return View(model);
        }
        //вызов страницы об конкретном проекте
        public IActionResult ProjectDetails(int id) 
        {
            ProjectModel model = data.GetProjectModel(id);
            
            return View(model);
        }
        #endregion

        #region стр. "Блог"

        //вызов страницы "Блог"
        public IActionResult Blogs() 
        {
            BlogsModel model = data.GetBlogs();
            return View(model);
        }
        //вызов страницы об конкретном блоге
        public IActionResult BlogDetails(int id)
        {
            BlogModel model = data.GetBlogModel(id);
            model.Is_edit = false;
            //BlogsModel model = new()
            //{
            //    Blogs = data.GetBlogs(),
            //    Name_page = data.GetMains().First(i => i.Id == 4).Value,
            //    BlogId = id
            //};
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
                Contacts = data.GetContacts().Where(i => i.Id != 1),
                ImageUrl = data.GetContacts().First(i => i.Id == 1).Description,
                Nets = data.GetSocialNet(),
                Name_page = data.GetMains().First(i => i.Id == 5).Value,
            };
            return View(model);
        }
        #endregion
    }
}