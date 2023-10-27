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
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //возможно переделать заполнение кнопок, так как идут разные
            HeaderModel model = new HeaderModel()
            {
                MainPages = await data.GetMainsAdminAsync()
            };


            return View("Default", model);
        }
    }
}
