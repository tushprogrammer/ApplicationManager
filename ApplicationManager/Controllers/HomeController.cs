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
        public async Task<IActionResult> IndexAsync()
        {
            MainPageUploadModel model = await data.GetMainsIndexPageAsync();
            return View(model);
        }
        //вызов верстки "Отправить заявку"
        public async Task<IActionResult> AddRequestAsync()
        {
            MainPage request_title = await data.GetMainRequestAsync();
            ViewBag.Title = request_title.Value;
            return PartialView();
        }
        //метод добавления заявки
        public async Task<IActionResult> AddNewRequestAsync(RequestModel model)
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
        public async Task<IActionResult> ProjectAsync()
        {
            //подгрузка данных из бд: 
            ProjectsModel model = await data.GetProjectsAsync();
            return View(model);
        }
        //вызов страницы об конкретном проекте
        public async Task<IActionResult> ProjectDetailsAsync(int id) 
        {
            ProjectModel model = await data.GetProjectModelAsync(id);
            
            return View(model);
        }
        #endregion

        #region стр. "Блог"

        //вызов страницы "Блог"
        public async Task<IActionResult> BlogsAsync() 
        {
            BlogsModel model = await data.GetBlogsAsync();
            return View(model);
        }
        //вызов страницы об конкретном блоге
        public async Task<IActionResult> BlogDetailsAsync(int id)
        {
            BlogModel model = await data.GetBlogModelAsync(id);
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
        public async Task<IActionResult> ServicesAsync() 
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
        public async Task<IActionResult> ContactsAsync()
        {
            ContactsModel model = await data.GetContactsModelAsync();
            //{               
            //    Contacts = data.GetContacts().Where(i => i.Id != 1),
            //    ImageUrl = data.GetContacts().First(i => i.Id == 1).Description,
            //    Nets = data.GetSocialNet(),
            //    Name_page = data.GetMains().First(i => i.Id == 5).Value,
            //};
            return View(model);
        }
        #endregion
    }
}