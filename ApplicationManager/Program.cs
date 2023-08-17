using ApplicationManager.AuthApp;
using ApplicationManager.ContextFolder;
using ApplicationManager.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IAppData, AppData>(); // в контроллере есть экзмепляр интерфейса IAppData,
                                                    // через который идет обращение к api (или пока что к внутренней бд)
builder.Services.AddMvc();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(@"Server = (localdb)\MSSQLLocalDB; 
                                            DataBase = DataBaseApplication; 
                                            Trusted_connection = true;"));
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 5; // минимальное количество знаков в пароле
    options.Password.RequireDigit = false; // Должна ли в пароле быть цифра
    options.Password.RequireUppercase = false; // Должен ли в пароле быть символ в верхнем регистре 
    options.Password.RequireNonAlphanumeric = false; // Должен ли в пароле быть символ не цифра или буква (пример -)
    options.Lockout.MaxFailedAccessAttempts = 10; // количество попыток о блокировки
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    options.Lockout.AllowedForNewUsers = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    //options.Cookie.Expiration = TimeSpan.FromMinutes(30);
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.SlidingExpiration = true;
});


var app = builder.Build();

app.UseAuthentication();
// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

using (var scope = app.Services.CreateScope())
{
    var s = scope.ServiceProvider;
    var c = s.GetRequiredService<ApplicationDbContext>();
    DbInitializer.Initialize(c);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.Run();

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated(); //создать, если еще не создана
                                          //со стандартными таблицами user 
                                          //и со всеми таблицами, что описаны в классе контекста
        //RoleManager<IdentityRole> roleMgr = new RoleStore<IdentityRole>(context);

        //тут же можно запихнуть базового админа admin-admin
        //и тут по умолчанию создать роль админа
        //вот только как это сделать, если для этого нужны менеджеры
        //если есть записи в основной таблице, то выйти


    }
}