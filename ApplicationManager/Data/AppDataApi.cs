﻿using System;
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
            catch (Exception ex)
            {
                var r = ex.Message;
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
        public IQueryable<Request> GetRequests()
        {
            try
            {
                string json = httpClient.GetStringAsync($"{url_main}/GetRequests").Result;
                return JsonConvert.DeserializeObject<IEnumerable<Request>>(json).AsQueryable();
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
                string urlWithParams = $"{url_main}/GetRequestsDate?DateFor={DateFor:yyyy-MM-dd}&DateTo={DateTo:yyyy-MM-dd}";

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
        public void EditMain(MainForm form, IFormFile image)
        {
            using (var content = new MultipartFormDataContent())
            {
                // Добавляем экземпляр класса в контент запроса как JSON
                var jsonForm = JsonConvert.SerializeObject(form);
                content.Add(new StringContent(jsonForm), "form");

                // Добавляем изображение в контент запроса
                using (var imageStream = new MemoryStream())
                {
                    image.CopyToAsync(imageStream);
                    content.Add(new StreamContent(imageStream), "image", image.FileName);
                }

                // Отправляем запрос к API
                var response = httpClient.PostAsync($"{url_main}/EditMain", content).Result;

            }
        }
        public void AddProject(Project new_project, IFormFile image)
        {
            using (var content = new MultipartFormDataContent())
            {
                var jsonForm = JsonConvert.SerializeObject(new_project);
                content.Add(new StringContent(jsonForm), "new_project");
                var streamContent = new StreamContent(image.OpenReadStream());
                content.Add(streamContent, "image", image.FileName);

                var response = httpClient.PostAsync($"{url_project}/AddProject", content).Result;
            }
        }
        public Project GetProject(int id)
        {
            try
            {
                string urlWithParams = $"{url_project}/GetProject?id={id}";
                string json = httpClient.GetStringAsync(urlWithParams).Result;
                return JsonConvert.DeserializeObject<Project>(json);
                
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ProjectsModel GetProjects()
        {
            try
            {
                //запрос к api на получение проектов и имени страницы project
                HttpResponseMessage response = httpClient.GetAsync($"{url_project}/GetProjects").Result;
                //обработка запроса
                if (response.IsSuccessStatusCode)
                {
                    //получение данных из запроса
                    var data = response.Content.ReadAsStringAsync().Result;
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
        public BlogsModel GetBlogs()
        {
            //осталось это поменять
            try
            {
                HttpResponseMessage response = httpClient.GetAsync($"{url_blog}/GetBlogs").Result;
                if (response.IsSuccessStatusCode)
                {
                    //получение данных из запроса
                    var data = response.Content.ReadAsStringAsync().Result;
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

        public DetailsProjectModel GetProjectModel(int id)
        {
            try
            {
                string urlWithParams = $"{url_project}/GetProjectModel?id={id}";
                HttpResponseMessage response = httpClient.GetAsync($"{urlWithParams}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    ProjectModel model = JsonConvert.DeserializeObject<ProjectModel>(data);

                    var base64 = Convert.ToBase64String(model.Project_with_image.Image_byte);
                    var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                    DetailsProjectModel projectModel = new()
                        {
                            Id = model.Project_with_image.Id,
                            NameCompany = model.Project_with_image.NameCompany,
                            Title = model.Project_with_image.Title,
                            Description = model.Project_with_image.Description,
                            ImgSrc = imgSrc,
                            Name_page = model.Name_page,
                        };
                    return projectModel;
                }
                else return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DetailsBlogModel GetBlogModel(int id)
        {
            try
            {
                string urlWithParams = $"{url_blog}/GetBlogModel?id={id}";
                HttpResponseMessage response = httpClient.GetAsync($"{urlWithParams}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    BlogModel model = JsonConvert.DeserializeObject<BlogModel>(data);

                    var base64 = Convert.ToBase64String(model.blog_With_Image.Image_byte);
                    var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                    DetailsBlogModel blogModel = new()
                    {
                        Id = model.blog_With_Image.Id,
                        Title = model.blog_With_Image.Title,
                        Description = model.blog_With_Image.Description,
                        ImgSrc = imgSrc,
                        Name_page = model.Name_page,
                        Created = model.blog_With_Image.Created,
                    };
                    return blogModel;
                }
                else return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public   void EditProject(Project edit_project, IFormFile image)
        {
            using (var content = new MultipartFormDataContent())
            {
                var jsonForm = JsonConvert.SerializeObject(edit_project);
                content.Add(new StringContent(jsonForm, Encoding.UTF8, "application/json"), "edit_project");
                // Перед добавлением изображения в контент запроса, дождитесь завершения копирования данных из файла
                var streamContent = new StreamContent(image.OpenReadStream());
                content.Add(streamContent, "image", image.FileName);

                var response = httpClient.PostAsync($"{url_project}/EditProject", content).Result;
            }
        }
        public void DeleteProject(int id)
        {
            try
            {
                string urlWithParams = $"{url_project}/DeleteProject?id={id}";
                var response = httpClient.DeleteAsync($"{urlWithParams}").Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public Service GetService(int id)
        {
            try
            {
                string urlWithParams = $"{url_service}/GetService?id={id}";
                string json = httpClient.GetStringAsync($"{urlWithParams}").Result;
                return JsonConvert.DeserializeObject<Service>(json);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void AddService(Service newService)
        {
            var re = httpClient.PostAsJsonAsync($"{url_service}/AddService", newService).Result;
        }
        public void DeleteService(int id)
        {
            try
            {
                string urlWithParams = $"{url_service}/DeleteService?id={id}";
                var response = httpClient.DeleteAsync($"{urlWithParams}").Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void EditService(Service service)
        {
            var re = httpClient.PostAsJsonAsync($"{url_service}/EditService", service).Result;
        }
        public Blog GetBlog(int id)
        {
            try
            {
                string urlWithParams = $"{url_blog}/GetBlog?id={id}";
                string json = httpClient.GetStringAsync($"{urlWithParams}").Result;
                return JsonConvert.DeserializeObject<Blog>(json);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void AddBlog(Blog new_blog, IFormFile image)
        {

            using (var content = new MultipartFormDataContent())
            {
                var jsonForm = JsonConvert.SerializeObject(new_blog);
                content.Add(new StringContent(jsonForm), "new_blog");
                var streamContent = new StreamContent(image.OpenReadStream());
                content.Add(streamContent, "image", image.FileName);

                var response = httpClient.PostAsync($"{url_blog}/AddBlog", content).Result;
            }
        }
        public void EditBlog(Blog edit_blog, IFormFile image)
        {
            using (var content = new MultipartFormDataContent())
            {
                // Добавляем экземпляр класса в контент запроса как JSON
                var jsonForm = JsonConvert.SerializeObject(edit_blog);
                content.Add(new StringContent(jsonForm), "edit_blog");
                var streamContent = new StreamContent(image.OpenReadStream());
                content.Add(streamContent, "image", image.FileName);

                // Отправляем запрос к API
                var response = httpClient.PostAsync($"{url_blog}/EditBlog", content).Result;
                // в переменной response ответ от api, успешно или нет
            }
        }
        public void DeleteBlog(int id)
        {
            try
            {
                string urlWithParams = $"{url_blog}/DeleteBlog?id={id}";
                var response = httpClient.DeleteAsync($"{urlWithParams}").Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SaveContacts(List<Contacts> contacts, List<SocialNet> socialNets, IFormFile image)
        {
            using (var content = new MultipartFormDataContent())
            {
                // Добавляем экземпляр класса в контент запроса как JSON
                var jsonForm = JsonConvert.SerializeObject(contacts);
                content.Add(new StringContent(jsonForm), "contacts");
                jsonForm = JsonConvert.SerializeObject(socialNets);
                content.Add(new StringContent(jsonForm), "socialNets");

                // Добавляем изображение в контент запроса
                using (var imageStream = new MemoryStream())
                {
                    image.CopyToAsync(imageStream);
                    content.Add(new StreamContent(imageStream), "image", image.FileName);
                }

                // Отправляем запрос к API
                var response = httpClient.PostAsync($"{url_сontacts}/SaveContacts", content).Result;
                // в переменной response ответ от api, успешно или нет
            }
        }
        public void SaveNewImageSocialNets(List<IFormFile> files)
        {
            //вот тут вопросики
            // Создаем объект MultipartFormDataContent
            var content = new MultipartFormDataContent();

            // Добавляем файлы в контент
            foreach (var file in files)
            {
                // Читаем содержимое файла в байтовый массив
                using (var stream = file.OpenReadStream())
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    var fileBytes = ms.ToArray();

                    // Создаем объект ByteArrayContent для передачи файла
                    var fileContent = new ByteArrayContent(fileBytes);

                    // Добавляем файл в контент
                    content.Add(fileContent, "files", file.FileName);
                }
            }
            // Отправляем POST запрос на API
            var response = httpClient.PostAsync($"{url_сontacts}/SaveNewImageSocialNets", content).Result;
        }
        public void SaveNamePages(List<MainPage> names, List<MainPage> NamesAdmin)
        {
            using (var content = new MultipartFormDataContent())
            {
                // Добавляем экземпляр класса в контент запроса как JSON
                var jsonForm = JsonConvert.SerializeObject(names);
                content.Add(new StringContent(jsonForm), "names");
                jsonForm = JsonConvert.SerializeObject(NamesAdmin);
                content.Add(new StringContent(jsonForm), "NamesAdmin");

                // Отправляем запрос к API
                var response = httpClient.PostAsync($"{url_main}/SaveNamePages", content).Result;
                // в переменной response ответ от api, успешно или нет
            }
        }
    }
}
