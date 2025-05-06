using Microsoft.EntityFrameworkCore;
using VideoSurveillanceApp.Shared;

namespace VideoSurveillanceApp.Server.Data
{
    public class VideoContext : DbContext
    {
        public DbSet<Camera> Cameras { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=cameras.db");
        }
    }
}