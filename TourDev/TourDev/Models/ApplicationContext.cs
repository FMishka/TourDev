using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TourDev.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Tourist>? Tourist { get; set; }
        public DbSet<Hotel>? Hotel { get; set; }
        public DbSet<Man>? Man { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<Excursion> Excursion { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Bag> Bag { get; set; }
        public DbSet<Flight> Flight { get; set; }
        public DbSet<Agent> Agent { get; set; }
        public DbSet<Tour> Tour { get; set; }
        public DbSet<Cost_for_bag_group> Cost_for_bag_group { get; set; }
        public DbSet<ManNew> ManNew { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
