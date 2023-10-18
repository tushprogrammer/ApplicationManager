using Newtonsoft.Json;
using System;
using ApplicationManager_ClassLibrary.Entitys;

namespace ApplicationManager.Data
{
    public class AppDataApi /*: IAppData*/
    {
        private HttpClient httpClient { get; set; }
        static readonly string url = $@"api/Person";

        public AppDataApi()
        {
            this.httpClient = new HttpClient();
            //тут настроить базовый адрес, но указать его не в коде, а в appsetting.json
            //httpClient.BaseAddress = new Uri(@"https://localhost:7068/");
        }
        public IQueryable<MainPage> GetMains()
        {
            //all
            //return Context.MainPage;
            try
            {
                string json = httpClient.GetStringAsync(url).Result;
                return JsonConvert.DeserializeObject<IEnumerable<MainPage>>(json).AsQueryable();
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
