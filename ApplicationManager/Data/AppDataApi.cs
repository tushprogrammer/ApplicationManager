using System;
using System.Text;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using ApplicationManager_ClassLibrary.Entitys;
using static System.Net.Mime.MediaTypeNames;
using ApplicationManager.Models;
using System.Collections;
using Microsoft.Data.SqlClient.Server;

namespace ApplicationManager.Data
{
    public class AppDataApi : IAppData
    {
        private HttpClient httpClient { get; set; }
        static readonly string url_main = $@"api/Main";
        static readonly string url_account = $@"api/Account";
        static readonly string url_project = $@"api/Project";
        static readonly string url_service = $@"api/Service";
        static readonly string url_blog = $@"api/Blog";
        static readonly string url_сontacts = $@"api/Contacts";

        public AppDataApi()
        {
            this.httpClient = new HttpClient();
            //тут настроить базовый адрес, но указать его не в коде, а в appsetting.json
            httpClient.BaseAddress = new Uri(@"https://localhost:7292/");
        }
        public async Task<IQueryable<MainPage>> GetMainsAsync()
        {
            //all
            //return Context.MainPage;
            try
            {
                string json = await httpClient.GetStringAsync(url_main + "/GetMains");
                return JsonConvert.DeserializeObject<IEnumerable<MainPage>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<MainPageUploadModel> GetMainsIndexPageAsync()
        {
            //Butt_main, Title, Image_main, Main_request
         
            try
            {
                //запрос к api на получение модели главной страницы
                HttpResponseMessage response = await httpClient.GetAsync($"{url_main}/GetMainsIndexPage");
                //обработка запроса
                if (response.IsSuccessStatusCode)
                {
                    //получение данных из запроса
                    var data = await response.Content.ReadAsStringAsync();
                    MainPageUploadModel model = JsonConvert.DeserializeObject<MainPageUploadModel>(data);
                    //переделка массива байтов в картинку для отображения на странице
                    var base64 = Convert.ToBase64String(model.Image_byte);
                    model.ImgSrc = String.Format("data:image/gif;base64,{0}", base64);
                    return model;
                }
                else return null;
            }
            catch (Exception ex)
            {
                var r = ex.Message;
                throw;
            }
        }
        public async Task<IQueryable<MainPage>> GetMainsAdminAsync()
        {
            //MainAdmin, ProjectAdmin, ServicesAdmin, BlogsAdmin, ContactsAdmin, Index
            try
            {
                string json = await httpClient.GetStringAsync(url_main + "/GetMainsAdmin");
                return JsonConvert.DeserializeObject<IEnumerable<MainPage>>(json).AsQueryable();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IQueryable<MainPage>> GetMainsHeaderAsync()
        {
            try
            {
                string json = await httpClient.GetStringAsync(url_main + "/GetMainsHeader");
                return JsonConvert.DeserializeObject<IEnumerable<MainPage>>(json).AsQueryable();
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public async Task<IQueryable<Service>> GetServicesAsync()
        {
            try
            {
                string json = await httpClient.GetStringAsync(url_service);
                return JsonConvert.DeserializeObject<IEnumerable<Service>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task<IQueryable<Contacts>> GetContactsAsync()
        {
            try
            {
                string json = await httpClient.GetStringAsync(url_сontacts);
                return JsonConvert.DeserializeObject<IEnumerable<Contacts>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ContactsModel> GetContactsModelAsync()
        {
            try
            {
                //запрос к api на получение модели 
                HttpResponseMessage response = await httpClient.GetAsync($"{url_сontacts}/GetContactsModel");
                //обработка запроса
                if (response.IsSuccessStatusCode)
                {
                    //получение данных из запроса
                    var data = response.Content.ReadAsStringAsync().Result;
                    ContactsModel model = JsonConvert.DeserializeObject<ContactsModel>(data);
                    //переделка массива байтов в картинку для отображения на странице
                    foreach (SocialNet_with_image item in model.Nets)
                    {
                        var base64 = Convert.ToBase64String(item.Image_byte);
                        item.ImgSrc = String.Format("data:image/gif;base64,{0}", base64);
                    }
                    var base64_m = Convert.ToBase64String(model.Image_byte);
                    model.ImgSrc = String.Format("data:image/gif;base64,{0}", base64_m);
                    
                    return model;
                }
                else return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IQueryable<SocialNet_with_image>> GetSocialNetAsync()
        {
            try
            {
                string json = await httpClient.GetStringAsync($"{url_сontacts}/GetSocialNet");
                List<SocialNet_with_image> nets = JsonConvert.DeserializeObject<List<SocialNet_with_image>>(json);
                foreach (SocialNet_with_image item in nets)
                {
                    var base64 = Convert.ToBase64String(item.Image_byte);
                    item.ImgSrc = String.Format("data:image/gif;base64,{0}", base64);
                }
                return nets.AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IQueryable<Request>> GetRequestsAsync()
        {
            try
            {
                string json = await httpClient.GetStringAsync($"{url_main}/GetRequests");
                return JsonConvert.DeserializeObject<IEnumerable<Request>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IQueryable<Request>> GetRequestsAsync(DateTime DateFor, DateTime DateTo)
        {
            try
            {
                string urlWithParams = $"{url_main}/GetRequestsDate?DateFor={DateFor:yyyy-MM-dd}&DateTo={DateTo:yyyy-MM-dd}";

                string json = await httpClient.GetStringAsync(urlWithParams);
                return JsonConvert.DeserializeObject<IEnumerable<Request>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IQueryable<Request>> GetRequestsStatusAsync(string statusName)
        {
            try
            {
                string urlWithParams = $"{url_main}/GetRequestsStatus?statusName={statusName}";
                string json = await httpClient.GetStringAsync(urlWithParams);
                return JsonConvert.DeserializeObject<IEnumerable<Request>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IQueryable<Request>> GetRequestsTodayAsync()
        {
            try
            {
                string json = await httpClient.GetStringAsync($"{url_main}/GetRequestsToday");
                return JsonConvert.DeserializeObject<IEnumerable<Request>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IQueryable<Request>> GetRequestsYesterdayAsync()
        {
            try
            {
                string json = await httpClient.GetStringAsync($"{url_main}/GetRequestsYesterday");
                return JsonConvert.DeserializeObject<IEnumerable<Request>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IQueryable<Request>> GetRequestsWeekAsync()
        {
            try
            {
                string json = await httpClient.GetStringAsync($"{url_main}/GetRequestsWeek");
                return JsonConvert.DeserializeObject<IEnumerable<Request>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IQueryable<Request>> GetRequestsMonthAsync()
        {
            try
            {
                string json = await httpClient.GetStringAsync($"{url_main}/GetRequestsMonth");
                return JsonConvert.DeserializeObject<IEnumerable<Request>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IQueryable<MainTitle>> GetMainTitlesAsync()
        {
            try
            {
                string json = await httpClient.GetStringAsync($"{url_main}/GetMainTitles");
                return JsonConvert.DeserializeObject<IEnumerable<MainTitle>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IQueryable<StatusRequest>> GetStatusesAsync()
        {
            try
            {
                string json = await httpClient.GetStringAsync($"{url_main}/GetStatuses");
                return JsonConvert.DeserializeObject<IEnumerable<StatusRequest>>(json).AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<MainPage> GetMainRequestAsync()
        {
            try
            {
                string json = await httpClient.GetStringAsync($"{url_main}/GetMainRequest");
                return JsonConvert.DeserializeObject<MainPage>(json);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task AddRequest(Request request)
        {
            using (var content = new MultipartFormDataContent())
            {
                var jsonForm = JsonConvert.SerializeObject(request);
                content.Add(new StringContent(jsonForm), "request");
                var response = await httpClient.PostAsync($"{url_main}/AddRequest", content);
            }
        }
        public async Task<int> CountRequestsAsync()
        {
            try
            {
                //string json = httpClient.GetStringAsync($"{url_main}/GetCountRequests").Result;
                //return JsonConvert.DeserializeObject<int>(json);
                return Convert.ToInt32(await httpClient.GetStringAsync($"{url_main}/GetCountRequests"));
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Request> GetRequestsNowAsync(string requestId)
        {
            try
            {
                string urlWithParams = $"{url_main}/GetRequestNow?requestId={requestId}";
                string json = await httpClient.GetStringAsync($"{urlWithParams}");
                return JsonConvert.DeserializeObject<Request>(json);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task SaveNewStatusRequest(Request reqNow)
        {
            //var json = JsonConvert.SerializeObject(reqNow);
            //var content = new StringContent(json, Encoding.UTF8, "application/json-patch+json");
            //var response = httpClient.PatchAsync($"{url_main}/SaveNewStatusRequest", content).Result;
            using (var content = new MultipartFormDataContent())
            {
                var jsonForm = JsonConvert.SerializeObject(reqNow);
                content.Add(new StringContent(jsonForm), "request");
                var response = await httpClient.PatchAsync($"{url_main}/SaveNewStatusRequest", content);
            }
        }
        public async Task EditMain(MainForm form, IFormFile image)
        {

            using (var content = new MultipartFormDataContent())
            {
                // Добавляем экземпляр класса в контент запроса как JSON
                var jsonForm = JsonConvert.SerializeObject(form);
                content.Add(new StringContent(jsonForm), "form");

                // Добавляем изображение в контент запроса
                if (image != null && image.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await image.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;
                        var streamContent = new StreamContent(memoryStream);
                        content.Add(streamContent, "image", image.FileName);
                        
                        var response = await httpClient.PostAsync($"{url_main}/EditMain", content);
                        //return response;
                    }
                }
                else
                {
                    var response = await httpClient.PostAsync($"{url_main}/EditMain", content);
                    //return response;
                }
                
                //if (image != null)
                //{
                //    using (var memoryStream = new MemoryStream())
                //    {

                //        await image.CopyToAsync(memoryStream);
                //        memoryStream.Position = 0;
                //        var streamContent = new StreamContent(memoryStream);
                //        content.Add(streamContent, "image", image.FileName);
                //    }
                //    //var streamContent = new StreamContent(image.OpenReadStream());
                //    //content.Add(streamContent, "image", image.FileName);
                //}

                // Отправляем запрос к API
                //var response = httpClient.PostAsync($"{url_main}/EditMain", content);

            }
        }

        public async Task AddProject(Project new_project, IFormFile image)
        {
            using (var content = new MultipartFormDataContent())
            {
                var jsonForm = JsonConvert.SerializeObject(new_project);
                content.Add(new StringContent(jsonForm), "new_project");
                if (image != null && image.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await image.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;
                        var streamContent = new StreamContent(memoryStream);
                        content.Add(streamContent, "image", image.FileName);

                        var response = await httpClient.PostAsync($"{url_project}/AddProject", content);
                    }
                    //var streamContent = new StreamContent(image.OpenReadStream());
                    //content.Add(streamContent, "image", image.FileName);
                }
                else
                {
                    var response = await httpClient.PostAsync($"{url_project}/AddProject", content);
                }
                

            }
        }
        public async Task<Project> GetProjectAsync(int id)
        {
            try
            {
                string urlWithParams = $"{url_project}/GetProject?id={id}";
                string json = await httpClient.GetStringAsync(urlWithParams);
                return JsonConvert.DeserializeObject<Project>(json);
                
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ProjectsModel> GetProjectsAsync()
        {
            try
            {
                //запрос к api на получение проектов и имени страницы project
                HttpResponseMessage response = await httpClient.GetAsync($"{url_project}/GetProjects");
                //обработка запроса
                if (response.IsSuccessStatusCode)
                {
                    //получение данных из запроса
                    var data = await response.Content.ReadAsStringAsync();
                    ProjectsModel model = JsonConvert.DeserializeObject<ProjectsModel>(data);
                    //переделка массива байтов в картинку для отображения на странице
                    foreach (Project_with_image item in model.Projects)
                    {
                        var base64 = Convert.ToBase64String(item.Image_byte);
                        item.ImgSrc = String.Format("data:image/gif;base64,{0}", base64);
                    }
                    return model;
                }
                else return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
     

        public async Task<ProjectModel> GetProjectModelAsync(int id)
        {
            try
            {
                string urlWithParams = $"{url_project}/GetProjectModel?id={id}";
                HttpResponseMessage response = await httpClient.GetAsync($"{urlWithParams}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    ProjectModel model = JsonConvert.DeserializeObject<ProjectModel>(data);

                    var base64 = Convert.ToBase64String(model.Project_with_image.Image_byte);
                    var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                    model.Project_with_image.ImgSrc = imgSrc;
                   
                    return model;
                }
                else return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task EditProject(Project edit_project, IFormFile image)
        {
            using (var content = new MultipartFormDataContent())
            {
                var jsonForm = JsonConvert.SerializeObject(edit_project);
                content.Add(new StringContent(jsonForm, Encoding.UTF8, "application/json"), "edit_project");
                // Перед добавлением изображения в контент запроса, дождитесь завершения копирования данных из файла
                if (image != null && image.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await image.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;
                        var streamContent = new StreamContent(memoryStream);
                        content.Add(streamContent, "image", image.FileName);

                        var response = await httpClient.PostAsync($"{url_project}/EditProject", content);
                    }
                }
                else
                {
                    var response = await httpClient.PostAsync($"{url_project}/EditProject", content);
                }
                //if (image != null)
                //{
                //    var streamContent = new StreamContent(image.OpenReadStream());
                //    content.Add(streamContent, "image", image.FileName);
                //}
                

            }
        }
        public async Task DeleteProject(int id)
        {
            try
            {
                string urlWithParams = $"{url_project}/DeleteProject?id={id}";
                var response = await httpClient.DeleteAsync($"{urlWithParams}");
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<DetailsServiceModel> GetServiceModelAsync(int id)
        {
            try
            {
                string urlWithParams = $"{url_service}/GetServiceModel?id={id}";
                string json = await httpClient.GetStringAsync($"{urlWithParams}");
                return JsonConvert.DeserializeObject<DetailsServiceModel>(json);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task AddService(Service newService)
        {
            var re = await httpClient.PostAsJsonAsync($"{url_service}/AddService", newService);
        }
        public async Task DeleteService(int id)
        {
            try
            {
                string urlWithParams = $"{url_service}/DeleteService?id={id}";
                var response = await httpClient.DeleteAsync($"{urlWithParams}");
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task EditService(Service service)
        {
            var re = await httpClient.PostAsJsonAsync($"{url_service}/EditService", service);
        }
        public async Task<Blog> GetBlogAsync(int id)
        {
            try
            {
                string urlWithParams = $"{url_blog}/GetBlog?id={id}";
                string json = await httpClient.GetStringAsync($"{urlWithParams}");
                return JsonConvert.DeserializeObject<Blog>(json);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<BlogsModel> GetBlogsAsync()
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"{url_blog}/GetBlogs");
                if (response.IsSuccessStatusCode)
                {
                    //получение данных из запроса
                    var data = await response.Content.ReadAsStringAsync();
                    BlogsModel model = JsonConvert.DeserializeObject<BlogsModel>(data);
                    //переделка массива байтов в картинку для отображения на странице
                    foreach (Blog_with_image item in model.Blogs)
                    {
                        var base64 = Convert.ToBase64String(item.Image_byte);
                        item.ImgSrc = String.Format("data:image/gif;base64,{0}", base64);
                    }
                    return model;
                }
                else return null;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<BlogModel> GetBlogModelAsync(int id)
        {
            try
            {
                string urlWithParams = $"{url_blog}/GetBlogModel?id={id}";
                HttpResponseMessage response = await httpClient.GetAsync($"{urlWithParams}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    BlogModel model = JsonConvert.DeserializeObject<BlogModel>(data);

                    var base64 = Convert.ToBase64String(model.blog_With_Image.Image_byte);
                    var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                    model.blog_With_Image.ImgSrc = imgSrc;
                    return model;
                }
                else return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task AddBlog(Blog new_blog, IFormFile image)
        {

            using (var content = new MultipartFormDataContent())
            {
                var jsonForm = JsonConvert.SerializeObject(new_blog);
                content.Add(new StringContent(jsonForm), "new_blog");
                if (image != null && image.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await image.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;
                        var streamContent = new StreamContent(memoryStream);
                        content.Add(streamContent, "image", image.FileName);

                        var response = await httpClient.PostAsync($"{url_blog}/AddBlog", content);
                    }
                }
                else
                {
                    var response = await httpClient.PostAsync($"{url_blog}/AddBlog", content);
                }


            }
        }
        public async Task EditBlog(Blog edit_blog, IFormFile image)
        {
            using (var content = new MultipartFormDataContent())
            {
                // Добавляем экземпляр класса в контент запроса как JSON
                var jsonForm = JsonConvert.SerializeObject(edit_blog);
                content.Add(new StringContent(jsonForm), "edit_blog");
                if (image != null && image.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await image.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;
                        var streamContent = new StreamContent(memoryStream);
                        content.Add(streamContent, "image", image.FileName);

                        var response = await httpClient.PostAsync($"{url_blog}/EditBlog", content);
                    }
                }
                else
                {
                    var response = await httpClient.PostAsync($"{url_blog}/EditBlog", content);
                }

                // Отправляем запрос к API
                
                // в переменной response ответ от api, успешно или нет
            }
        }
        public async Task DeleteBlog(int id)
        {
            try
            {
                string urlWithParams = $"{url_blog}/DeleteBlog?id={id}";
                var response = await httpClient.DeleteAsync($"{urlWithParams}");
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task SaveContacts(List<Contacts> contacts, IFormFile image)
        {
            using (var content = new MultipartFormDataContent())
            {
                // Добавляем экземпляр класса в контент запроса как JSON
                var jsonForm = JsonConvert.SerializeObject(contacts);
                content.Add(new StringContent(jsonForm), "contacts");
                //jsonForm = JsonConvert.SerializeObject(socialNets);
                //content.Add(new StringContent(jsonForm), "socialNets");

                // Добавляем изображение в контент запроса
                if (image != null && image.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await image.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;
                        var streamContent = new StreamContent(memoryStream);
                        content.Add(streamContent, "image", image.FileName);

                        var response = await httpClient.PostAsync($"{url_сontacts}/SaveContacts", content);
                    }
                }
                else
                {
                    var response = await httpClient.PostAsync($"{url_сontacts}/SaveContacts", content);
                }


                
            }
        }
        public async Task SaveSocialNets(List<IFormFile> files, List<SocialNet_with_image> SocialNets)
        {
            using (var content = new MultipartFormDataContent())
            {
                string jsonForm = JsonConvert.SerializeObject(SocialNets);
                content.Add(new StringContent(jsonForm), "SocialNets");
                // Добавляем файлы в контент
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        // Читаем содержимое файла в байтовый массив
                        using (var stream = file.OpenReadStream())
                        using (var ms = new MemoryStream())
                        {
                            await stream.CopyToAsync(ms);
                            var fileBytes = ms.ToArray();

                            // Создаем объект ByteArrayContent для передачи файла
                            var fileContent = new ByteArrayContent(fileBytes);

                            // Добавляем файл в контент
                            content.Add(fileContent, "files", file.FileName);
                        }
                    }
                    var response = await httpClient.PostAsync($"{url_сontacts}/SaveSocialNets", content);
                    //using (var memoryStream = new MemoryStream())
                    //{
                    //    await image.CopyToAsync(memoryStream);
                    //    memoryStream.Position = 0;
                    //    var streamContent = new StreamContent(memoryStream);
                    //    content.Add(streamContent, "image", image.FileName);

                    //    var response = await httpClient.PostAsync($"{url_сontacts}/SaveContacts", content);
                    //}
                }
                else
                {
                    var response = await httpClient.PostAsync($"{url_сontacts}/SaveSocialNets", content);
                }

                
                // Отправляем POST запрос на API
                
            }

            
        }
        public async Task SaveNamePages(List<MainPage> names, List<MainPage> NamesAdmin)
        {
            using (var content = new MultipartFormDataContent())
            {
                // Добавляем экземпляр класса в контент запроса как JSON
                var jsonForm = JsonConvert.SerializeObject(names);
                content.Add(new StringContent(jsonForm), "names");
                jsonForm = JsonConvert.SerializeObject(NamesAdmin);
                content.Add(new StringContent(jsonForm), "NamesAdmin");

                // Отправляем запрос к API
                var response = await httpClient.PostAsync($"{url_main}/SaveNamePages", content);
                // в переменной response ответ от api, успешно или нет
            }
        }
        public async Task<bool> Authenticate(string login, string password)
        {
            var loginData = new Dictionary<string, string>()
                {
                    { "username", login },
                    { "password", password }
                };

            var content = new FormUrlEncodedContent(loginData);
            var response = await httpClient.PostAsync($"{url_account}/authenticate", content);
            return response.IsSuccessStatusCode;
        }
    }
}
