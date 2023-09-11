using ApplicationManager.Data;
using ApplicationManager.Entitys;
using ApplicationManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ApplicationManager.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAppData data;
        public AdminController(IAppData Data)
        {
            data = Data;
        }
        //рабочий стол (где отображаются все заявки)
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
        [HttpGet]
        public IActionResult StatusRequests(string statusName)
        {
            AdminModel model = new AdminModel()
            {
                Requests = data.GetRequestsStatus(statusName),
                Statuses = GetNameStatuses(),
                AllRequestsCount = data.CountRequests(),
            };
            return View("Index",model);
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


        private IQueryable<string> GetNameStatuses()
        {
            List<string> ListStatuses = new List<string>();
            IQueryable<StatusRequest> statusRequests = data.GetStatuses();
            foreach (var statusRequest in statusRequests)
            {
                ListStatuses.Add(statusRequest.StatusName);
            }
            return ListStatuses.AsQueryable();
        }
        public IActionResult SaveNewStatusRequest(string RequestId, string StatusName)
        {
            Request reqNow = data.GetRequestsNow(RequestId);
            StatusRequest Status = data.GetStatuses().First(s => s.StatusName == StatusName);
            reqNow.StatusId = Status.Id;
            data.SaveNewRequest(reqNow);
            AdminModel model = new AdminModel()
            {
                Requests = data.GetRequests(), //все заявки
                Statuses = GetNameStatuses(),
                AllRequestsCount = data.CountRequests(),
            };
            var r = Request.Headers["Referer"].ToString();
            return Redirect(r);
            //обновленме всего рабочего стола, а все настройки сортировки остаются


        }
    }
}
