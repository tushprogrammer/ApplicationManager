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
        public async Task<IActionResult> Index()
        {
            MainPageUploadModel model = await data.GetMainsIndexPageAsync();
            return View(model);
        }
        //вызов верстки "Отправить заявку"
        public async Task<IActionResult> AddRequest()
        {
            MainPage request_title = await data.GetMainRequestAsync();
            ViewBag.Title = request_title.Value;
            return PartialView();
        }
        //метод добавления заявки
        public async Task<IActionResult> AddNewRequest(RequestModel model)
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
                await data.AddRequest(new_req);
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
        public async Task<IActionResult> Project()
        {
            ProjectsModel model = await data.GetProjectsAsync();
            return View(model);
        }
        //вызов страницы об конкретном проекте
        public async Task<IActionResult> ProjectDetails(int id) 
        {
            ProjectModel model = await data.GetProjectModelAsync(id);
            
            return View(model);
        }
        #endregion

        #region стр. "Блог"

        //вызов страницы "Блог"
        public async Task<IActionResult> Blogs() 
        {
            BlogsModel model = await data.GetBlogsAsync();
            return View(model);
        }
        //вызов страницы об конкретном блоге
        public async Task<IActionResult> BlogDetails(int id)
        {
            BlogModel model = await data.GetBlogModelAsync(id);
            model.Is_edit = false;
            return View(model);
        }
        #endregion

        #region стр. "Услуги"

        //вызов страницы "Услуги"
        public async Task<IActionResult> Services() 
        {
            IQueryable<MainPage> mains = await data.GetMainsAsync();
            ServicesModel model = new()
            {
                Services = await data.GetServicesAsync(),
                Name_page = mains.First(i => i.Id == 2).Value
            };
            return View(model);
        }
        #endregion

        #region стр. "Контакты"

        //вызов страницы "Контакты"
        public async Task<IActionResult> Contacts()
        {
            ContactsModel model = await data.GetContactsModelAsync();
            return View(model);
        }
        #endregion
    }
}