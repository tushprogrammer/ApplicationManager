using Newtonsoft.Json;
using System;
using ApplicationManager_ClassLibrary.Entitys;
using System.Text;

namespace ApplicationManager.Data
{
    public class AppDataApi /*: IAppData*/
    {
        private HttpClient httpClient { get; set; }
        static readonly string url_main = $@"api/Main";
        static readonly string url_project = $@"api/Project";
        static readonly string url_service = $@"api/Service";
        static readonly string url_blog = $@"api/Blog";
        static readonly string url_сontacts = $@"api/Contacts";

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
                string json = httpClient.GetStringAsync(url_main + "/GetMains").Result;
                return JsonConvert.DeserializeObject<IEnumerable<MainPage>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public MainForm GetMainsIndexPage()
        {
            //Butt_main, Title, Image_main, Main_request
            try
            {
                string json = httpClient.GetStringAsync(url_main + "/GetMainsIndexPage").Result;
                return JsonConvert.DeserializeObject<MainForm>(json);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IQueryable<MainPage> GetMainsAdmin()
        {
            //MainAdmin, ProjectAdmin, ServicesAdmin, BlogsAdmin, ContactsAdmin, Index
            try
            {
                string json = httpClient.GetStringAsync(url_main + "/GetMainsAdmin").Result;
                return JsonConvert.DeserializeObject<IEnumerable<MainPage>>(json).AsQueryable();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IQueryable<MainPage> GetMainsHeader()
        {
            try
            {
                string json = httpClient.GetStringAsync(url_main + "/GetMainsHeader").Result;
                return JsonConvert.DeserializeObject<IEnumerable<MainPage>>(json).AsQueryable();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IQueryable<Project> GetProjects()
        {
            try
            {
                string json = httpClient.GetStringAsync(url_project).Result;
                return JsonConvert.DeserializeObject<IEnumerable<Project>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<Service> GetServices()
        {
            try
            {
                string json = httpClient.GetStringAsync(url_service).Result;
                return JsonConvert.DeserializeObject<IEnumerable<Service>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<Blog> GetBlogs()
        {
            try
            {
                string json = httpClient.GetStringAsync(url_blog).Result;
                return JsonConvert.DeserializeObject<IEnumerable<Blog>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<Contacts> GetContacts()
        {
            try
            {
                string json = httpClient.GetStringAsync(url_сontacts).Result;
                return JsonConvert.DeserializeObject<IEnumerable<Contacts>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<SocialNet> GetSocialNet()
        {
            try
            {
                string json = httpClient.GetStringAsync(url_сontacts + "GetSocialNet").Result;
                return JsonConvert.DeserializeObject<IEnumerable<SocialNet>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<Request> GetRequests(DateTime DateFor, DateTime DateTo)
        {
            try
            {
                string urlWithParams = $"{url_main}?DateFor={DateFor:yyyy-MM-dd}&DateTo={DateTo:yyyy-MM-dd}";

                string json = httpClient.GetStringAsync(urlWithParams).Result;
                return JsonConvert.DeserializeObject<IEnumerable<Request>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<Request> GetRequestsStatus(string statusName)
        {
            try
            {
                string urlWithParams = $"{url_main}?statusName={statusName}";
                string json = httpClient.GetStringAsync(urlWithParams).Result;
                return JsonConvert.DeserializeObject<IEnumerable<Request>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<Request> GetRequestsToday()
        {
            try
            {
                string json = httpClient.GetStringAsync($"{url_main}/GetRequestsToday").Result;
                return JsonConvert.DeserializeObject<IEnumerable<Request>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<Request> GetRequestsYesterday()
        {
            try
            {
                string json = httpClient.GetStringAsync($"{url_main}/GetRequestsYesterday").Result;
                return JsonConvert.DeserializeObject<IEnumerable<Request>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<Request> GetRequestsWeek()
        {
            try
            {
                string json = httpClient.GetStringAsync($"{url_main}/GetRequestsWeek").Result;
                return JsonConvert.DeserializeObject<IEnumerable<Request>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<Request> GetRequestsMonth()
        {
            try
            {
                string json = httpClient.GetStringAsync($"{url_main}/GetRequestsMonth").Result;
                return JsonConvert.DeserializeObject<IEnumerable<Request>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<MainTitle> GetMainTitles()
        {
            try
            {
                string json = httpClient.GetStringAsync($"{url_main}/GetMainTitles").Result;
                return JsonConvert.DeserializeObject<IEnumerable<MainTitle>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IQueryable<StatusRequest> GetStatuses()
        {
            try
            {
                string json = httpClient.GetStringAsync($"{url_main}/GetStatuses").Result;
                return JsonConvert.DeserializeObject<IEnumerable<StatusRequest>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public MainPage GetMainRequest()
        {
            try
            {
                string json = httpClient.GetStringAsync($"{url_main}/GetMainRequest").Result;
                return JsonConvert.DeserializeObject<MainPage>(json);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void AddRequest(Request request)
        {
            var re = httpClient.PostAsJsonAsync($"{url_main}/AddRequest", request).Result;
        }
        public int CountRequests()
        {
            try
            {
                string json = httpClient.GetStringAsync($"{url_main}/GetCountRequests").Result;
                return JsonConvert.DeserializeObject<int>(json);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Request GetRequestsNow(string requestId)
        {
            try
            {
                string urlWithParams = $"{url_main}?requestId={requestId}";
                string json = httpClient.GetStringAsync($"{url_main}/GetRequestsNow").Result;
                return JsonConvert.DeserializeObject<Request>(json);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SaveNewStatusRequest(Request reqNow)
        {
            //проверить детально
            var json = JsonConvert.SerializeObject(reqNow);
            var content = new StringContent(json, Encoding.UTF8, "application/json-patch+json");
            var response = httpClient.PatchAsync($"{url_main}/SaveNewStatusRequest", content).Result;
        }

    }
}
