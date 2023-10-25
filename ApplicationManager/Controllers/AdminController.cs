using AjaxControlToolkit;
using ApplicationManager.Data;
using ApplicationManager_ClassLibrary.Entitys;
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
using ApplicationManager_ClassLibrary;
using System.Text.RegularExpressions;
using System.Net.Mime;
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
            data.SaveNewStatusRequest(reqNow);
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
            MainPageUploadModel model = data.GetMainsIndexPage();
            return View(model);
        }

        //вызов страницы редактирования "Главная"
        public IActionResult EditMain()
        {
            MainPageUploadModel model = data.GetMainsIndexPage();
            return View(model);
        }

        //вызов метода редактирования страницы "Главная"
        [HttpPost]
        public IActionResult EditMainSave(MainPageUploadModel model)
        {
            //тут разобрать модель на составляющие, потому что модель нужна только для копановки воедино инфы
            //и отправить на страницу, а потом тут её принять. то есть не надо эту модель дальше в appdata отправлять
            //валидации не будет, вдруг это дизайнерское решение, убрать какую нибудь надпись
            IFormFile Image = model.Image;
            MainForm mainForm = new()
            {
                Title = model.Title,
                ButtonTitle = model.ButtonTitle,
                RequestTitle = model.RequestTitle,
            };
            data.EditMain(mainForm, Image);            
            
            return Redirect("~/Admin/MainAdmin");
        }
        #endregion

        #region стр. "Проекты"

        //вывод страницы для просмотра "Проекты" перед редактированием
        public IActionResult ProjectAdmin()
        {
            //все проекты + имя страницы
            ProjectsModel model = data.GetProjects();
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
        public IActionResult AddProjectMethod(ProjectModel model)
        {
            //валидация модели на заполненность всех обязательных полей
            if (ModelState.IsValid)
            {
                Project new_project = new()
                {
                    Title = model.Project_with_image.Title,
                    Description = model.Project_with_image.Description,
                    NameCompany = model.Project_with_image.NameCompany,
                   
                };
                //если повторно вводили, и основная картинка скинулась, возможно остался её ImgSrc
                if (model.Project_with_image.Image == null && model.Project_with_image.ImgSrc != null)
                {
                    
                    IFormFile image = ConvertImgSrcToFormFile(model.Project_with_image.ImgSrc, model.Project_with_image.Image_name);
                    data.AddProject(new_project, image);
                }
                else
                {
                    data.AddProject(new_project, model.Project_with_image.Image);
                }
                
                return Redirect("~/Admin/ProjectAdmin"); //успех
            }
            else
            {
                //предупреждение и подсвеченные поля, которые не заполнили
                ModelState.AddModelError("", "Заполните все обязательные поля");
                model.Is_edit = false;
                if (model.Project_with_image.ImgSrc is null && model.Project_with_image.Image != null)
                {
                    //то есть попытались ввести новый элемент блога, загрузили картинку, но где то в другом месте ошиблись
                    //чтоб картинку заного не загружать, можно преобразовать её в ImgSrc и вывести снова
                    model.Project_with_image.ImgSrc = ConvertFormFileToImgSrc(model.Project_with_image.Image);
                    model.Project_with_image.Image_name = model.Project_with_image.Image.FileName;
                }
                return View("DetailsProject", model); //повторная попытка
            }
        }
        //страница изменения проекта
        public IActionResult DetailsProject(int id)
        {
            ProjectModel model = data.GetProjectModel(id);
            model.Is_edit = true;
            return View("DetailsProject", model);
        }
        //метод изменения проекта
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditProjectMethod(ProjectModel model)
        {
            if (ModelState.IsValid)
            {
                Project Edit_project = new()
                {
                    Id = model.Project_with_image.Id,
                    Title = model.Project_with_image.Title,
                    NameCompany = model.Project_with_image.NameCompany,
                    Description = model.Project_with_image.Description,
                };
                if (model.Project_with_image.Image == null && model.Project_with_image.ImgSrc != null)
                {
                    // преобразование из ImgSrc в iformfile
                    IFormFile image = ConvertImgSrcToFormFile(model.Project_with_image.ImgSrc, model.Project_with_image.Image_name);

                    data.EditProject(Edit_project, image);
                }
                else
                {
                    data.EditProject(Edit_project, model.Project_with_image.Image);
                }
                return Redirect("~/Admin/ProjectAdmin");
            }
            else
            {
                
                ModelState.AddModelError("", "Заполните все обязательные поля");
                if (model.Project_with_image.Image != null)
                {
                    //то есть попытались ввести новый элемент блога, загрузили картинку, но где то в другом месте ошиблись
                    //чтоб картинку заного не загружать, можно преобразовать её в ImgSrc и вывести снова
                    model.Project_with_image.ImgSrc = ConvertFormFileToImgSrc(model.Project_with_image.Image);
                    model.Project_with_image.Image_name = model.Project_with_image.Image.FileName;
                }
                model.Is_edit = true;
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
            DetailsServiceModel model = data.GetServiceModel(id);
            model.is_edit = true;
            //Service model = data.GetService(id);
            //ViewBag.Name_page = data.GetMains().First(i => i.Id == 2).Value;
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
        public IActionResult AddNewServiceMethod(DetailsServiceModel model)
        {
            if (ModelState.IsValid)
            {
                Service new_service = new()
                {
                    Title = model.Title,
                    Description = model.Description,
                };
                data.AddService(new_service);
                return Redirect("~/Admin/ServicesAdmin");
            }
            else
            {
                //красная надпись сверху и подсвеченные поля, которые не заполнили
                ModelState.AddModelError("", "Заполните все обязательные поля");
                //ViewBag.Name_page = data.GetMains().First(i => i.Id == 2).Value;
                model.is_edit = false;
                return View("DetailsService", model); //повторная попытка
            }
        }
        //метод удаления услуги
        public IActionResult DeleteServiceMethod(int id)
        {
            data.DeleteService(id);
            return Redirect("~/Admin/ServicesAdmin");
        }
        //метод изменения услуги
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditServiceMethod(DetailsServiceModel model)
        {
            if (ModelState.IsValid)
            {
                Service edit_service = new()
                {
                    Id = model.Id,
                    Title = model.Title,
                    Description = model.Description,
                };
                data.EditService(edit_service);
                return Redirect("~/Admin/ServicesAdmin");
            }
            else
            {
                ModelState.AddModelError("", "Заполните все обязательные поля");
                //ViewBag.Name_page = data.GetMains().First(i => i.Id == 2).Value;
                model.is_edit = true;
                return View("DetailsService", model);
            }
        }
        #endregion

        #region стр. "Блог"
        //вызов страницы для редактирования страницы "Блог"
        public IActionResult BlogsAdmin()
        {
            //все элементы блога + имя страницы
            BlogsModel model = data.GetBlogs();            
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
        public IActionResult AddNewBlogMethod(BlogModel model)
        {
            if (ModelState.IsValid)
            {
                Blog new_blog = new()
                {
                    Title = model.blog_With_Image.Title,
                    Description = model.blog_With_Image.Description,
                    Created = DateTime.Now,
                };
               
                //если повторно вводили, и основная картинка скинулась, возможно остался её ImgSrc
                if (model.blog_With_Image.Image == null  && model.blog_With_Image.ImgSrc != null)
                {
                    // преобразование из ImgSrc в iformfile
                    IFormFile image = ConvertImgSrcToFormFile(model.blog_With_Image.ImgSrc, model.blog_With_Image.Image_name);
                 
                    data.AddBlog(new_blog, image);
                }
                else
                {
                    data.AddBlog(new_blog, model.blog_With_Image.Image);
                }
                return Redirect("~/Admin/BlogsAdmin"); //успех
            }
            else
            {
                //красная надпись сверху и подсвеченные поля, которые не заполнили
                ModelState.AddModelError("", "Заполните все обязательные поля");
                model.Is_edit = false;
                if (model.blog_With_Image.ImgSrc is null && model.blog_With_Image.Image != null)
                {
                    //то есть попытались ввести новый элемент блога, загрузили картинку, но где то в другом месте ошиблись
                    //чтоб картинку заного не загружать, можно преобразовать её в ImgSrc и вывести снова
                    model.blog_With_Image.ImgSrc = ConvertFormFileToImgSrc(model.blog_With_Image.Image);
                    model.blog_With_Image.Image_name = model.blog_With_Image.Image.FileName;
                }
                //ViewBag.Name_page = data.GetMains().First(i => i.Id == 4).Value;
                return View("DetailsBlog", model); //повторная попытка
            }
            
        }
        //вызов страницы изменения блога
        public IActionResult EditBlog(int id)
        {
            BlogModel model = data.GetBlogModel(id);
            model.Is_edit = true;
            return View("DetailsBlog", model);
        }
        //метод изменения блога
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditBlogMethod(BlogModel model)
        {
            if (ModelState.IsValid)
            {
                Blog edit_blog = new()
                {
                    Id = model.blog_With_Image.Id,
                    Title = model.blog_With_Image.Title,
                    Description = model.blog_With_Image.Description,
                };
                //если повторно вводили, и основная картинка скинулась, возможно остался её ImgSrc
                if (model.blog_With_Image.Image == null && model.blog_With_Image.ImgSrc != null)
                {
                    // преобразование из ImgSrc в iformfile
                    IFormFile image = ConvertImgSrcToFormFile(model.blog_With_Image.ImgSrc, model.blog_With_Image.Image_name);

                    data.EditBlog(edit_blog, image);
                }
                else
                {
                    data.EditBlog(edit_blog, model.blog_With_Image.Image);
                }
                return Redirect("~/Admin/BlogsAdmin"); //успех
            }
            else
            {
                ModelState.AddModelError("", "Заполните все обязательные поля");
                if (model.blog_With_Image.Image != null)
                {
                    //то есть попытались ввести новый элемент блога, загрузили картинку, но где то в другом месте ошиблись
                    //чтоб картинку заного не загружать, можно преобразовать её в ImgSrc и вывести снова
                    model.blog_With_Image.ImgSrc = ConvertFormFileToImgSrc(model.blog_With_Image.Image);
                    model.blog_With_Image.Image_name = model.blog_With_Image.Image.FileName;
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
            ContactsModel model = data.GetContactsModel();
            //{
            //    Contacts = data.GetContacts().Where(i => i.Id != 1),
            //    ImageUrl = data.GetContacts().First(i => i.Id == 1).Description,
            //    Nets = data.GetSocialNet(),
            //    Name_page = data.GetMains().First(i => i.Id == 5).Value,
            //};
            return View(model);
        }
        //вызов страницы для редактирования "Контакты"
        public IActionResult EditContact()
        {
            ContactsModel model = data.GetContactsModel();
            //{
            //    //Contacts = data.GetContacts().Where(i => i.Id != 7),//это подгружает AngularJS
            //    ImageUrl = data.GetContacts().First(i => i.Id == 1).Description,
            //    //Nets = data.GetSocialNet(), //это подгружает AngularJS
            //    Name_page = "Изменить контакты",
            //};
            return View(model);
        }
        //вызов информации от angular о контактах на стр. "Контакты"
        [HttpGet]
        public IActionResult GetContactsDate()
        {
            string json = JsonConvert.SerializeObject(data.GetContacts().Where(i => i.Id != 1));
            return new JsonResult(json);
        }
        //вызов информации от angular о социальных сетях на стр. "Контакты"
        [HttpGet]
        public IActionResult GetSocialNetsDate() 
        {
            //тут проблемка, надо через angular выводить ImgSrc
            string json = JsonConvert.SerializeObject(data.GetSocialNet());
            return new JsonResult(json);
        }
        //сохранение информации о контактах и соц сетях
        public IActionResult SaveContacts(string stringData, IFormFile ImageUrl) 
        {
            if (stringData is not null)
            {
                ContactsUploadModel model = JsonConvert.DeserializeObject<ContactsUploadModel>(stringData);
                model.Contacts.RemoveAll(i => i.Description == string.Empty || i.Name == string.Empty );
                //при передаче в сохранение достать имена, из соц сетей, по возможности соединить с сохраненными картинками
                //data.SaveContacts(model.Contacts, model.SocialNets, ImageUrl);
            }
            return Redirect("~/Admin/ContactsAdmin");

        }
        //сохранение изображений соц. сетей от angular
        [HttpPost]
        public IActionResult SaveContactfiles(List<IFormFile> files)
        {
            //загрузка на сервер картинок соц. сетей
            if (files is not null)
            {
                data.SaveNewImageSocialNets(files);
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

        // преобразование из ImgSrc в iformfile
        private IFormFile ConvertImgSrcToFormFile (string imgSrc, string image_name)
        {
            var base64Data = Regex.Match(imgSrc, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
            var bytes = Convert.FromBase64String(base64Data);
            var stream = new MemoryStream(bytes);
            return new FormFile(stream, 0, stream.Length, "image", image_name);
        }
        //преобразование из iformfile в ImgSrc
        private string ConvertFormFileToImgSrc(IFormFile image)
        {
            byte[] Image_byte;
            using (var memoryStream = new MemoryStream())
            {
                image.CopyTo(memoryStream);
                Image_byte = memoryStream.ToArray();
            }

            var base64 = Convert.ToBase64String(Image_byte);
            return String.Format("data:image/gif;base64,{0}", base64);
        }


    }
}


