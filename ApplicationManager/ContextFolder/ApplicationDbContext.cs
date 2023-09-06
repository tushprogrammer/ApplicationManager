using ApplicationManager.AuthApp;
using ApplicationManager.Entitys;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApplicationManager.ContextFolder
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<MainPage> MainPage { get; set; }
        public DbSet<MainTitle> Titles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<SocialNet> SocialNets { get; set; }
        public DbSet<StatusRequest> Statuses { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
    }
}
