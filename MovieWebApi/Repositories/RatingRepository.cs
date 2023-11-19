using MovieWebApi.Data;
using MovieWebApi.Interfaces;
using MovieWebApi.Models;

namespace MovieWebApi.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly MovieWebApiContext _context;

        public RatingRepository(MovieWebApiContext context)
        {
            _context = context; 
        }

        public Rating GetRating(int id)
        {
            return _context.Ratings.Where(r => r.Id == id).FirstOrDefault();
        }

        public ICollection<Rating> GetRatings()
        {
            return _context.Ratings.OrderBy(r => r.Id).ToList();
        }

        public bool RatingExists(int id)
        {
            return _context.Ratings.Any(r => r.Id == id);
        }

        public Rating GetRatingByName(string ratingName)
        {
            return _context.Ratings.Where(r => r.RatingName.Contains(ratingName)).FirstOrDefault();
        }

        public ICollection<Movie> GetMoviesByRating(int ratingId)
        {
            return _context.Ratings.Where(r => r.Id == ratingId).SelectMany(r => r.Movies).ToList();
        }

        public bool CreateRating(Rating rating)
        {
            _context.Ratings.Add(rating);
            return Save();
        }

        public bool UpdateRating(Rating rating)
        {
            _context.Ratings.Update(rating);
            return Save();
        }

        public bool DeleteRating(Rating rating)
        {
            // Keep in mind that rating is closely tied to the data in Movies.
            // Deleting one rating might result in the deletion of more than one data in Movies.

            // List all movies that use this rating as the FK.
            var affectedMovies = GetMoviesByRating(rating.Id);

            foreach (var movie in affectedMovies)
            {
                _context.Movies.Remove(movie);
            }

            _context.Ratings.Remove(rating);
            return Save();
        }

        public bool Save()
        {
            var changesSaved = _context.SaveChanges();
            return changesSaved > 0 ? true : false;
        }
    }
}
