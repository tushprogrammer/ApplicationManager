using ApplicationManager.AuthApp;
using ApplicationManager.ContextFolder;
using ApplicationManager.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IAppData, AppData>(); // � ����������� ���� ��������� ���������� IAppData,
                                                    // ����� ������� ���� ��������� � api (��� ���� ��� � ���������� ��)
builder.Services.AddMvc();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(@"Server = (localdb)\MSSQLLocalDB; 
                                            DataBase = DataBaseApplication; 
                                            Trusted_connection = true;"));
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 5; // ����������� ���������� ������ � ������
    options.Password.RequireDigit = false; // ������ �� � ������ ���� �����
    options.Password.RequireUppercase = false; // ������ �� � ������ ���� ������ � ������� �������� 
    options.Password.RequireNonAlphanumeric = false; // ������ �� � ������ ���� ������ �� ����� ��� ����� (������ -)
    options.Lockout.MaxFailedAccessAttempts = 10; // ���������� ������� � ����������
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
    var _RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var _UserManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();



    var s = scope.ServiceProvider;
    var c = s.GetRequiredService<ApplicationDbContext>();
    DbInitializer.Initialize(c, _UserManager, _RoleManager);
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
    public static void Initialize(ApplicationDbContext context, UserManager<User> _UserManager, RoleManager<IdentityRole> _RoleManager)
    {
        context.Database.EnsureCreated(); //�������, ���� ��� �� �������
                                          //�� ������������ ��������� user 
                                          //� �� ����� ���������, ��� ������� � ������ ���������

        if (context.Users.Any()) return; //���� ���� ���� ���� ������������, �� �������� �� ���������:
        var res = _RoleManager.CreateAsync(new IdentityRole("Admin")).Result; //�������� ���� � �������
        User admin = new User { UserName = "admin" }; //������� �������� ������
        var createResult = _UserManager.CreateAsync(admin, "admin").Result; //���������������� ������ � ��
        res = _UserManager.AddToRoleAsync(admin, "Admin").Result; //������ ������ ���� ������


        //��� �� ��������� ����������� �������� ������ 


    }
}