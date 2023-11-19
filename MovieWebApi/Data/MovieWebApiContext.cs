using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MovieWebApi.Models;

namespace MovieWebApi.Data
{
    public class MovieWebApiContext : DbContext 
    {
        public MovieWebApiContext(DbContextOptions<MovieWebApiContext> options) : base(options)
        {
            
        }

        public DbSet<Movie> Movies { get; set; }
        
        public DbSet<Rating> Ratings { get; set; }
    }
}
