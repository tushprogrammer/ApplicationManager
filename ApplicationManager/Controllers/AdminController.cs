using AjaxControlToolkit;
using ApplicationManager.Data;
using ApplicationManager.Entitys;
using ApplicationManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Web.Helpers;
using static System.Net.Mime.MediaTypeNames;
//using System.Web.HttpPostedFileWrapper;

namespace ApplicationManager.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAppData data;
        private readonly IWebHostEnvironment webHost;
        private readonly ILogger<AdminController> _logger;
        public AdminController(ILogger<AdminController> logger, IAppData Data, IWebHostEnvironment WebHost)
        {
            _logger = logger;
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
            AdminModel model = new AdminModel()
            {
                Requests = data.GetRequests(), //все заявки
                Statuses = GetNameStatuses(),
                AllRequestsCount = data.CountRequests(),
            };
            return View("Index", model);
        }
        //заявки сегодня
        public IActionResult TodayRequests()
        {
            //на экран надо выводить отсортированные заявки и общее количество в бд
            int count = data.CountRequests(); 
            AdminModel model = new AdminModel()
            {
                Requests = data.GetRequestsToday(), 
                Statuses = GetNameStatuses(),
                AllRequestsCount = count,
            };
            return View("Index", model);
        }
        //заявки вчера
        public IActionResult YesterdayRequests()
        {
            AdminModel model = new AdminModel()
            {
                Requests = data.GetRequestsYesterday(), 
                Statuses = GetNameStatuses(),
                AllRequestsCount = data.CountRequests(),
            };
            return View("Index", model);
        }
        //заявки на этой неделе
        public IActionResult WeekRequests()
        {
            AdminModel model = new AdminModel()
            {
                Requests = data.GetRequestsWeek(), 
                Statuses = GetNameStatuses(),
                AllRequestsCount = data.CountRequests(),
            };
            return View("Index", model);
        }
        //заявки в этом месяце
        public IActionResult MonthRequests()
        {
            AdminModel model = new AdminModel()
            {
                Requests = data.GetRequestsMonth(), 
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
        //заявки по статусам
        [HttpGet]
        public IActionResult StatusRequests(string statusName) 
        {
            AdminModel model = new AdminModel()
            {
                Requests = data.GetRequestsStatus(statusName),
                Statuses = GetNameStatuses(),
                AllRequestsCount = data.CountRequests(),
            };
            return View("Index", model);
        }
       

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
            //обновление всего рабочего стола, а все настройки сортировки остаются
        }
        #endregion

        #region стр. "Главная"

        
        //вызов страницы просмотра "главная" перед редактированием
        public IActionResult MainAdmin()
        {
            
            IQueryable<MainPage> mainPages = data.GetMainsIndexpage();
            MainPageModel model = new()
            {
                Image_path = mainPages.First(i => i.Id == 8).Value,
                Title = mainPages.First(i => i.Id == 7).Value,
                ButtonTitle = mainPages.First(i => i.Id == 6).Value,
                RequestTitle = mainPages.First(i => i.Id == 9).Value,
            };
            
            return View(model);
        }

        //вызов страницы редактирования "Главная"
        public IActionResult EditMain()
        {
            IQueryable<MainPage> mainPages = data.GetMainsIndexpage();
            MainPageUploadModel model = new()
            {
                Title = mainPages.First(i => i.Id == 7).Value,
                ButtonTitle = mainPages.First(i => i.Id == 6).Value,
                RequestTitle = mainPages.First(i => i.Id == 9).Value,
            };
            //image идет отдельно, так как тут image это путь к файлу, а в модели это целый файл для загрузки
            //возможно при написании api все таки придется в таблице хранить целый файл, и сюда грузить массив байтов
            ViewBag.Image = mainPages.First(i => i.Id == 8).Value;
            return View(model);
        }

        //вызов метода редактирования страницы "Главная"
        [HttpPost]
        public IActionResult EditMainSave(MainPageUploadModel model)
        {
            //всякое сохранение
            data.EditMain(model);            
            
            return Redirect("~/Admin/MainAdmin");
        }
        #endregion

        #region стр. "Проекты"

        //вывод страницы для просмотра "Проекты" перед редактированием
        public IActionResult ProjectAdmin()
        {
            //все проекты
            ProjectsModel model = new()
            {
                Projects = data.GetProjects(),
                Name_page = data.GetMains().First(i => i.Id == 3).Value
            };
            //имя странциы проектов из тб mainpage
            return View(model);
        }
        //страница добавления нового проекта
        public IActionResult AddNewProject() 
        {
            //заголовок для шапки
            ViewBag.Name_page = data.GetMains().First(i => i.Id == 3).Value;
            return View("DetailsProject");
        }
        //метод добавления нового проекта
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddProjectMethod(DetailsProjectModel model)
        {
            if (ModelState.IsValid)
            {
                data.AddProject(model);
                return Redirect("~/Admin/ProjectAdmin"); //успех
            }
            else
            {
                //красная надпись сверху и подсвеченные поля, которые не заполнили
                ModelState.AddModelError("", "Заполните все обязательные поля");
                ViewBag.Name_page = data.GetMains().First(i => i.Id == 3).Value;
                return View("DetailsProject"); //повторная попытка
            }
        }
        //страница изменения проекта
        public IActionResult DetailsProject(int id)
        {
            Project temp = data.GetProject(id);
            ViewBag.ImageUrl = temp.ImageUrl;
            ViewBag.Name_page = data.GetMains().First(i => i.Id == 3).Value;
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
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditProjectMethod(DetailsProjectModel model)
        {
            if (ModelState.IsValid)
            {
                data.EditProject(model);
                return Redirect("~/Admin/ProjectAdmin");
            }
            else
            {
                ModelState.AddModelError("", "Заполните все обязательные поля");
                Project temp = data.GetProject(model.Id);
                ViewBag.Name_page = data.GetMains().First(i => i.Id == 3).Value;
                if (temp != null)
                {
                    ViewBag.ImageUrl = temp.ImageUrl;
                }

                return View("DetailsProject", model); //повторная попытка
            }
        }
        //метод удаления проекта
        public IActionResult DeleteProjectMethod(int id)
        {
            data.DeleteProject(id);
            return Redirect("~/Admin/ProjectAdmin");
        }
        #endregion

        #region стр. "Услуги"
        //вызов страницы для редактирования страницы "Услуги"
        public IActionResult ServicesAdmin()
        {
            ServicesModel model = new()
            {
                Services = data.GetServices(),
                Name_page = data.GetMains().First(i => i.Id == 2).Value
            };
            return View(model);
        }
        //страница изменения услуги
        public IActionResult EditService(int id)
        {
            Service model = data.GetService(id);
            ViewBag.Name_page = data.GetMains().First(i => i.Id == 2).Value;
            return View("DetailsService", model);
        }
        //страница добавления услуги
        public IActionResult AddNewService() 
        {
            ViewBag.Name_page = data.GetMains().First(i => i.Id == 2).Value;
            return View("DetailsService");
        }
        //метод добавления услуги
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddNewServiceMethod(Service New_Service)
        {
            if (ModelState.IsValid)
            {
                data.AddService(New_Service);
                return Redirect("~/Admin/ServicesAdmin");
            }
            else
            {
                //красная надпись сверху и подсвеченные поля, которые не заполнили
                ModelState.AddModelError("", "Заполните все обязательные поля");
                ViewBag.Name_page = data.GetMains().First(i => i.Id == 2).Value;
                return View("DetailsService"); //повторная попытка
            }
        }
        //метод удаления услуги
        public IActionResult DeleteServiceMethod(int id_Service)
        {
            data.DeleteService(id_Service);
            return Redirect("~/Admin/ServicesAdmin");
        }
        //метод изменения услуги
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditServiceMethod(Service Changed_Service)
        {
            if (ModelState.IsValid)
            {
                data.EditService(Changed_Service);
                return Redirect("~/Admin/ServicesAdmin");
            }
            else
            {
                ModelState.AddModelError("", "Заполните все обязательные поля");
                ViewBag.Name_page = data.GetMains().First(i => i.Id == 2).Value;
                return View("DetailsService", Changed_Service);
            }
        }
        #endregion

        #region стр. "Блог"
        //вызов страницы для редактирования страницы "Блог"
        public IActionResult BlogsAdmin()
        {
            BlogsModel model = new()
            {
                Blogs = data.GetBlogs(),
                Name_page = data.GetMains().First(i => i.Id == 4).Value,
            };
            return View(model);
        }
        //страница добавления блога
        
        public IActionResult AddNewBlog()
        {
            
            ViewBag.Name_page = data.GetMains().First(i => i.Id == 4).Value;
            return View("DetailsBlog");
        }
        //метод добавления блога
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddNewBlogMethod(DetailsBlogModel newBlog)
        {
            if (ModelState.IsValid)
            {
                data.AddBlog(newBlog);
                return Redirect("~/Admin/BlogsAdmin"); //успех
            }
            else
            {
                //красная надпись сверху и подсвеченные поля, которые не заполнили
                ModelState.AddModelError("", "Заполните все обязательные поля");
                ViewBag.Name_page = data.GetMains().First(i => i.Id == 4).Value;
                return View("DetailsBlog"); //повторная попытка
            }
            
        }
        //вызов страницы изменения блога
        public IActionResult EditBlog(int id)
        {
            ViewBag.Name_page = data.GetMains().First(i => i.Id == 4).Value;
            Blog blogNow =  data.GetBlog(id);
            ViewBag.ImageUrl = blogNow.ImageUrl;
            DetailsBlogModel model = new()
            {
                Id = id,
                Title = blogNow.Title,
                Description = blogNow.Description,
            };
            return View("DetailsBlog", model);
        }
        //метод изменения блога
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditBlogMethod(DetailsBlogModel model)
        {
            if (ModelState.IsValid)
            {
                data.EditBlog(model);
                return Redirect("~/Admin/BlogsAdmin"); //успех
            }
            else
            {
                ModelState.AddModelError("", "Заполните все обязательные поля");
                ViewBag.Name_page = data.GetMains().First(i => i.Id == 4).Value;
                Blog blog = data.GetBlog(model.Id);
                if (blog != null)
                {
                    ViewBag.ImageUrl = data.GetBlog(model.Id).ImageUrl;
                }
                
                return View("DetailsBlog", model); //повторная попытка
            }
        }
        //метод удаления блога
        public IActionResult DeleteBlogMEthod(int id)
        {
            data.DeleteBlog(id);
            return Redirect("~/Admin/BlogsAdmin");
        }
        #endregion

        #region стр. "Контакты"

        
        //вызов страницы просмотра "Контакты" перед редактированием
        public IActionResult ContactsAdmin()
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
        //вызов страницы для редактирования "Контакты"
        public IActionResult EditContact()
        {
            ContactsModel model = new()
            {
                //Contacts = data.GetContacts().Where(i => i.Id != 7),//это подгружает AngularJS
                ImageUrl = data.GetContacts().First(i => i.Id == 7).Description,
                //Nets = data.GetSocialNet(), //это подгружает AngularJS
                Name_page = "Изменить контакты",
            };
            return View(model);
        }
        //вызов информации от angular о контактах на стр. "Контакты"
        [HttpGet]
        public IActionResult GetString()
        {
            string json = JsonConvert.SerializeObject(data.GetContacts().Where(i => i.Id != 7));
            return new JsonResult(json);
        }
        //вызов информации от angular о социальных сетях на стр. "Контакты"
        [HttpGet]
        public IActionResult GetStringSocialNet() 
        {
            string json = JsonConvert.SerializeObject(data.GetSocialNet());
            return new JsonResult(json);
        }
        //сохранение информации о контактах и соц сетях
        public IActionResult SaveContacts(string stringData, IFormFile ImageUrl) 
        {
            if (stringData is not null)
            {
                
                ContactsNewModel newcontacts = JsonConvert.DeserializeObject<ContactsNewModel>(stringData);
                newcontacts.Image = ImageUrl;
                //перед сохранением осталось загрузить в форму картинку, прислать сюда, и закинуть в таблицу Contacts
                data.SaveContacts(newcontacts);
            }
            //return new JsonResult("все ок");
            return Redirect("~/Admin/ContactsAdmin");

        }
        //сохранение изображений соц. сетей от angular
        [HttpPost]
        public IActionResult SaveContactfiles(List<IFormFile> files)
        {
            //загрузка на сервер картинок соц. сетей
            if (files is not null)
            {
                data.SaveNewFiles(files);
            }
            
            return Ok();
        }

        //вызов верстки модального окна (надо на случай, если данные динамические)
        public IActionResult ModalViewContactsSocialNets()
        {
            //данные подгружаются за счёт angular?
            return View();
        }
        #endregion

        #region стр. "редактирование имен страниц"

        public IActionResult EditNamePages()
        {
            NamePagesModel model = new()
            {
                Name_page = "Изменение имен страниц",
                Names = data.GetMainsHeader().ToList(),
                Names_admin = data.GetMainsAdmin().ToList(),
            };
            return View(model);
        }
        public IActionResult EditNameMethod(List<MainPage> Names, List<MainPage> NamesAdmin)
        {
            //сохранение новых имен
            data.SaveNamePages(Names, NamesAdmin);
            return Redirect("/Admin/EditNamePages");
        }
        #endregion
    }
}


