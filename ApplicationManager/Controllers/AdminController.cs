﻿using ApplicationManager.Data;
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
    }
}
