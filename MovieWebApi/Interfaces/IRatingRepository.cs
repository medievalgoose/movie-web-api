using MovieWebApi.Models;

namespace MovieWebApi.Interfaces
{
    public interface IRatingRepository
    {
        ICollection<Rating> GetRatings();

        Rating GetRating(int id);

        Rating GetRatingByName(string ratingName);

        ICollection<Movie> GetMoviesByRating(int ratingId);

        bool RatingExists(int id);

        bool CreateRating(Rating rating);

        bool UpdateRating(Rating rating);

        bool DeleteRating(Rating rating);

        bool Save();
    }
}
