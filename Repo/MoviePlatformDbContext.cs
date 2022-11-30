using Microsoft.EntityFrameworkCore;
using MoviePlatform.Entities;
using System.Linq.Expressions;
using MoviePlatform.ViewModels.Users;

namespace MoviePlatform.Repo
{
    public class MoviePlatformDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<ActorsToMovies> ActorsToMovies { get; set; }
        public DbSet<Favourite> Favourites { get; set; }

        public MoviePlatformDbContext()
        {
            Users = this.Set<User>();
            Movies = this.Set<Movie>();
            Actors = this.Set<Actor>();
            ActorsToMovies = this.Set<ActorsToMovies>();
            Favourites = this.Set<Favourite>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=GEORGI_VIDEV;Database=MoviePlatformDb;User Id=gvidev;Password=adminpass;");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = 1,
                    Username = "gvidev",
                    Password = "adminpass",
                    FirstName = "Georgi",
                    LastName = "Videv"
                }
                );
        }

       

       
    }
}
