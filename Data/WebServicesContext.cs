using Microsoft.EntityFrameworkCore;

namespace WebTeploobmenApp.Data
{
    public class WebServicesContext : DbContext
    {
        public DbSet<News> News { get; set; }

        public WebServicesContext(DbContextOptions<WebServicesContext> options) : base(options) { }
    }
}
