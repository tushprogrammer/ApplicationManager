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
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //это рандомные лозунги
            //два обращения, может переделать в одно ?
            Random r = new Random();
            IQueryable<MainTitle> titles = await data.GetMainTitlesAsync();
            List<MainTitle> titles_list = titles.ToList();
            int randIndex = r.Next(0, titles.Count());
            HeaderModel model = new HeaderModel()
            {
                //надо чтоб строго индекс был с 1 по количество 
                //Title = titles.First(i => i.Id == randIndex).Title,
                Title = titles_list[randIndex].Title,
                MainPages = await data.GetMainsHeaderAsync()
            };
           

            return View("Default", model);
        }
    }
}
