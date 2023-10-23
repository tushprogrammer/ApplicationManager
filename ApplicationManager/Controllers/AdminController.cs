﻿using AjaxControlToolkit;
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

            MainForm mainForm = data.GetMainsIndexPage();
            //MainPageModel model = new()
            //{
            //    Image_path = mainForm.UrlImage,
            //    Title = mainForm.Title,
            //    ButtonTitle = mainForm.ButtonTitle,
            //    RequestTitle = mainForm.RequestTitle,
            //};
            
            return View(mainForm);
        }

        //вызов страницы редактирования "Главная"
        public IActionResult EditMain()
        {
            MainForm mainForm = data.GetMainsIndexPage();
            MainPageUploadModel model = new()
            {
                Title = mainForm.Title,
                ButtonTitle = mainForm.ButtonTitle,
                RequestTitle = mainForm.RequestTitle,
            };
            //image идет отдельно, так как тут image это путь к файлу, а в модели это целый файл для загрузки
            //возможно при написании api все таки придется в таблице хранить целый файл, и сюда грузить массив байтов
            ViewBag.Image = mainForm.UrlImage;
            return View(model);
        }

        //вызов метода редактирования страницы "Главная"
        [HttpPost]
        public IActionResult EditMainSave(MainPageUploadModel model)
        {
            //всякое сохранение
            //тут разобрать модель на составляющие, потому что модель нужна только для копановки воедино инфы
            //и отправить на страницу, а потом тут её принять. то есть не надо эту модель дальше в appdata отправлять
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
        public IActionResult AddProjectMethod(DetailsProjectModel model)
        {
            //валидация модели на заполненность всех обязательных полей
            if (ModelState.IsValid)
            {
                Project new_project = new()
                {
                    Title = model.Title,
                    Description = model.Description,
                    NameCompany = model.NameCompany,
                   
                };
                data.AddProject(new_project, model.Image);
                return Redirect("~/Admin/ProjectAdmin"); //успех
            }
            else
            {
                //предупреждение и подсвеченные поля, которые не заполнили
                ModelState.AddModelError("", "Заполните все обязательные поля");
                ViewBag.Name_page = data.GetMains().First(i => i.Id == 3).Value;
                //при повторной попытке можно было б из модели достать iformfile, если он уже загружен
                //и внести в ImgSrc, чтоб опять отобразить на странице, но это будет лишь заполнение
                //div заливкой, а input останется пустым, и если пользователь при повторной попытке увидит,
                //что картинка осталась, не будет её заполнять заного, и iformfile не передастся сюда, 
                //и информация с ImgSrc не превратится в iformfile
                //есть такая еще альтернатива:

                // Определите тип изображения (в данном случае gif)
                //var fileType = "image/gif";

                //// Извлеките строку base64 из ImgSrc
                //var base64Data = Regex.Match(imgSrc, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;

                //// Преобразуйте строку base64 в массив байтов
                //var bytes = Convert.FromBase64String(base64Data);

                //// Создайте поток из массива байтов
                //var stream = new MemoryStream(bytes);

                //// Создайте IFormFile из потока
                //IFormFile file = new FormFile(stream, 0, stream.Length, "image", fileName)
                //{
                //    ContentType = fileType
                //};
                //но для неё надо запоминать имя файла и сам imgSrc и через input type="hidden" передавать сюда
                return View("DetailsProject"); //повторная попытка
            }
        }
        //страница изменения проекта
        public IActionResult DetailsProject(int id)
        {
            DetailsProjectModel model = data.GetProjectModel(id);
            return View(model);
        }
        //метод изменения проекта
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditProjectMethod(DetailsProjectModel model)
        {
            if (ModelState.IsValid)
            {
                Project Edit_project = new()
                {
                    Id = model.Id,
                    Title = model.Title,
                    NameCompany = model.NameCompany,
                    Description = model.Description,
                };
                data.EditProject(Edit_project, model.Image);
                return Redirect("~/Admin/ProjectAdmin");
            }
            else
            {
                //можно в теории подумать о своих спрятанных инпутах, чтоб в них продолжалась хранится информация, а не скидывалась,
                //чтоб заново её не искать, то есть картинка по умолчанию и имя странцы
                //можно еще подумать о сохранении первой введенной картинки, но без фанатизма
                ModelState.AddModelError("", "Заполните все обязательные поля");
                DetailsProjectModel temp = data.GetProjectModel(model.Id);
                model.Name_page = temp.Name_page;
                if (temp != null)
                {
                    model.ImgSrc = temp.ImgSrc;
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
        public IActionResult EditServiceMethod(DetailsServiceModel model)
        {
            if (ModelState.IsValid)
            {
                Service edit_service = new()
                {
                    Title = model.Title,
                    Description = model.Description,
                };
                data.EditService(edit_service);
                return Redirect("~/Admin/ServicesAdmin");
            }
            else
            {
                ModelState.AddModelError("", "Заполните все обязательные поля");
                ViewBag.Name_page = data.GetMains().First(i => i.Id == 2).Value;
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
        public IActionResult AddNewBlogMethod(DetailsBlogModel model)
        {
            if (ModelState.IsValid)
            {
                Blog new_blog = new()
                {
                    Title = model.Title,
                    Description = model.Description,
                    Created = DateTime.Now,
                };
                data.AddBlog(new_blog, model.Image);
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
            DetailsBlogModel model = data.GetBlogModel(id);
            //ViewBag.Name_page = data.GetMains().First(i => i.Id == 4).Value;
            //Blog blogNow =  data.GetBlog(id);
            //ViewBag.ImageUrl = blogNow.ImageUrl;
            //DetailsBlogModel model = new()
            //{
            //    Id = id,
            //    Title = blogNow.Title,
            //    Description = blogNow.Description,
            //};
            return View("DetailsBlog", model);
        }
        //метод изменения блога
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult EditBlogMethod(DetailsBlogModel model)
        {
            if (ModelState.IsValid)
            {
                Blog edit_blog = new()
                {
                    Id = model.Id,
                    Title = model.Title,
                    Description = model.Description,
                };
                data.EditBlog(edit_blog, model.Image);
                return Redirect("~/Admin/BlogsAdmin"); //успех
            }
            else
            {
                ModelState.AddModelError("", "Заполните все обязательные поля");
                DetailsBlogModel temp = data.GetBlogModel(model.Id);
                model.Name_page = temp.Name_page;
                if (temp != null)
                {
                    model.ImgSrc = temp.ImgSrc;
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
                Contacts = data.GetContacts().Where(i => i.Id != 1),
                ImageUrl = data.GetContacts().First(i => i.Id == 1).Description,
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
                ImageUrl = data.GetContacts().First(i => i.Id == 1).Description,
                //Nets = data.GetSocialNet(), //это подгружает AngularJS
                Name_page = "Изменить контакты",
            };
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
                data.SaveContacts(model.Contacts, model.SocialNets, ImageUrl);
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
    }
}


