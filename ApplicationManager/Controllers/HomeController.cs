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
            List<MainPage> mainPages = data.GetMains();
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
                Email = Email,
                Fullname = Name,
                Textrequest = Description,
                StatusId = 1

            };
            data.AddRequest(newRequest);
            return Redirect("~/"); //возврат к первой странице
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