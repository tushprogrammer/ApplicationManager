using ApplicationManager.Data;
using ApplicationManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationManager.ViewComponents
{
    public class HeaderAdminViewComponent : ViewComponent
    {
        private readonly IAppData data;
        public HeaderAdminViewComponent(IAppData data)
        {
            this.data = data;
        }
        public IViewComponentResult Invoke()
        {
            //возможно переделать заполнение кнопок, так как идут разные
            HeaderModel model = new HeaderModel()
            {
                MainPages = data.GetMainsAdmin()
            };


            return View("Default", model);
        }
    }
}
