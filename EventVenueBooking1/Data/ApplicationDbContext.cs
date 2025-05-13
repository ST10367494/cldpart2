using EventVenueBooking1.Models.Entities;

using Microsoft.EntityFrameworkCore;

namespace EventVenueBooking1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Venue> Venues { get; set; }

    }

}
