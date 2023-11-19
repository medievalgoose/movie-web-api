using Microsoft.EntityFrameworkCore;
using MovieWebApi.Data;
using MovieWebApi.Interfaces;
using MovieWebApi.Models;

namespace MovieWebApi.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieWebApiContext _context;

        public MovieRepository(MovieWebApiContext context)
        {
            _context = context;
        }

        public ICollection<Movie> GetMovies()
        {
            // Return all movies.
            return _context.Movies.OrderBy(m => m.Id).ToList();
        }

        public Movie GetMovie(int id)
        {
            // Return the movie object.
            return _context.Movies.Where(m => m.Id == id).FirstOrDefault();
        }

        public bool MovieExists(int id)
        {
            // Check if the requested movie exist.
            return _context.Movies.Any(m => m.Id == id);
        }

        public Movie GetMovieByName(string movieName)
        {
            // Get a movie with name that contains the string in "movieName"
            // Similar to LIKE statement.
            return _context.Movies.Where(m => m.Name.Contains(movieName)).FirstOrDefault();
        }

        public ICollection<Movie> GetMoviesByRating(int ratingId)
        {
            // Returns a list of movies that have the specified Rating ID.
            return _context.Ratings.Where(r => r.Id == ratingId).SelectMany(r => r.Movies).ToList();
        }

        public bool CreateMovie(Movie movie)
        {
            var ratingFKValid = _context.Ratings.Any(r => r.Id == movie.RatingId);
            
            if (!ratingFKValid)
                return false;

            _context.Movies.Add(movie);
            return Save();
        }

        public bool UpdateMovie(Movie movie)
        {
            var validRatingFK = _context.Ratings.Any(r => r.Id == movie.RatingId);

            if (!validRatingFK)
                return false;

            _context.Movies.Update(movie);
            return Save();
        }

        public bool Save()
        {
            var changeSaved = _context.SaveChanges();

            // If the change is successfully saved, return true. Otherwise, return false.
            return changeSaved > 0 ? true : false;
        }
    }
}
