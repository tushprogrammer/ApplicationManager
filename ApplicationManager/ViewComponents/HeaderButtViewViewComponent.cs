using ApplicationManager.Data;
using ApplicationManager.Entitys;
using ApplicationManager.Models;
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
            Random r = new Random();
            List<MainPage> titles = data.GetMains().Where(i => i.Id > 9).ToList();
            int randIndex = r.Next(0, titles.Count());
            HeaderModel model = new HeaderModel()
            {
                Title = titles[randIndex].Value,
                MainPages = data.GetMains().Where(item => item.Id >= 1 && item.Id <= 5)
            };
           

            return View("Default", model);
        }
    }
}
