using Microsoft.EntityFrameworkCore;
using VueAppMvc.Server.Models;

namespace VueAppMvc.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        //Add your tables
        public DbSet<BookingModel>? bookings { get; set; }
        public DbSet<AddressModel>? addresses { get; set; }
    }
}
