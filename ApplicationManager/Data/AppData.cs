using ApplicationManager.ContextFolder;
using ApplicationManager.Entitys;

namespace ApplicationManager.Data
{
    public class AppData : IAppData
    {
        private readonly ApplicationDbContext Context;
        public AppData(ApplicationDbContext context)
        {
            Context = context;
        }
        public List<MainPage> GetMains()
        {
            return Context.MainPage.Where(item => item.Id >= 6 && item.Id <= 9).ToList();
        }
        public MainPage GetMainRequest()
        {
            return Context.MainPage.First(item => item.Id == 9);
        }
        public void AddRequest(Request NewRequest)
        {

            Context.Requests.Add(NewRequest);
            //Context.ChangeTracker.DetectChanges();
            Context.SaveChanges();
        }
    }
}
