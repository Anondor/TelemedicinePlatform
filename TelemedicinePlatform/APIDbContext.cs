using Microsoft.EntityFrameworkCore;
using TelemedicinePlatform.Models;

namespace TelemedicinePlatform
{
    public class APIDbContext : DbContext
    {
        public APIDbContext(DbContextOptions<APIDbContext>options):base(options) { }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ServicesList> ServicesLists { get;set; }



    }
}
