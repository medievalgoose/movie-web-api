using AutoMapper.Configuration.Conventions;
using Microsoft.EntityFrameworkCore.Metadata;
using MovieWebApi.Models;

namespace MovieWebApi.Interfaces
{
    public interface IMovieRepository
    {
        ICollection<Movie> GetMovies();

        ICollection<Movie> GetMoviesByRating(int ratingId);

        Movie GetMovie(int id);

        Movie GetMovieByName(string movieName);

        bool MovieExists(int id);

        bool CreateMovie(Movie movie);

        bool UpdateMovie(Movie movie);

        bool DeleteMovie(Movie movie);

        bool Save();
    }
}
