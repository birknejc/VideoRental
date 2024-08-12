using Microsoft.EntityFrameworkCore;
using MovieRental.Models;

namespace MovieRental.DBContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Order> Orders { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Order>()
                .HasKey(o => o.Id);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.Movies)
                .WithMany(m => m.Orders)
                .UsingEntity(j => j.ToTable("OrderMovies")); // Join table
        }
    }
}
