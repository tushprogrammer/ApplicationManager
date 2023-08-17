using ApplicationManager.Entitys;

namespace ApplicationManager.Data
{
    public interface IAppData
    {
        public List<MainPage> GetMains();
        public MainPage GetMainRequest();
        public void AddRequest(Request request);
    }
}
