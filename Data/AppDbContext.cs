using Microsoft.EntityFrameworkCore;
using StarWarsAPI.Models;


namespace StarWarsAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Starship> Starships { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Pilot> Pilots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Unique indexes to prevent duplicates
            modelBuilder.Entity<Film>()
                .HasIndex(f => f.Id)
                .IsUnique();

            modelBuilder.Entity<Pilot>()
                .HasIndex(p => p.Id)
                .IsUnique();

            modelBuilder.Entity<Starship>()
                .HasMany(s => s.Films)
                .WithMany(f => f.Starships)
                .UsingEntity(j => j.ToTable("StarshipFilms"));

            modelBuilder.Entity<Starship>()
                .HasMany(s => s.Pilots)
                .WithMany(p => p.Starships)
                .UsingEntity(j => j.ToTable("StarshipPilots"));
        }
    }

}
