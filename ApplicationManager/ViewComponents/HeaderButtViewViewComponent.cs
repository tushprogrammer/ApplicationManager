using ApplicationManager.Data;
using ApplicationManager.Entitys;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationManager.ViewComponents
{
    public class HeaderButtViewViewComponent : ViewComponent
    {
        private readonly IAppData data;
        public HeaderButtViewViewComponent(IAppData data)
        {
            this.data = data;
        }
        public IViewComponentResult Invoke()
        {
            //осталось решить, как тут же в компонент (или уже в другой компонент) выгружать блок с описанием,
            //это рандомные фразочки
            IQueryable<MainPage> mainPages = data.GetMains().Where(item => item.Id >= 1 && item.Id <= 5);
            return View("Default", mainPages);
        }
    }
}
