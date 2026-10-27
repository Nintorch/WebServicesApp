using Microsoft.EntityFrameworkCore;

namespace WebTeploobmenAppServer.Data
{
    public class WebServicesContext : DbContext
    {
        public DbSet<News> News { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public WebServicesContext(DbContextOptions<WebServicesContext> options) : base(options) { }
    }
}
