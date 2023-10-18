using ApplicationManager.Data;
using ApplicationManager_ClassLibrary.Entitys;
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
            //это рандомные лозунги
            Random r = new Random();
            List<MainTitle> titles = data.GetMainTitles().ToList();
            int randIndex = r.Next(0, titles.Count());
            HeaderModel model = new HeaderModel()
            {
                Title = titles[randIndex].Title,
                MainPages = data.GetMainsHeader()
            };
           

            return View("Default", model);
        }
    }
}
