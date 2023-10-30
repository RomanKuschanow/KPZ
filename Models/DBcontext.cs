using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class DBContext : DbContext
    {
        public string DBPath => "database.db";
        public DbSet<Address> Addresses { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Statement> Statements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseSqlite($"Data Source = {DBPath}");
        }
    }
}