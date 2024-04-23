using Microsoft.EntityFrameworkCore;
using TelemedicinePlatform.Models;

namespace TelemedicinePlatform
{
    public class APIDbContext : DbContext
    {
        public APIDbContext(DbContextOptions<APIDbContext>options):base(options) { }
        public DbSet<Doctor> Doctors { get; set; }
    }
}
