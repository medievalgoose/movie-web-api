using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieWebApi.DTO;
using MovieWebApi.Interfaces;
using MovieWebApi.Models;

namespace MovieWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _moviesRepo;
        private readonly IMapper _mapper;

        public MoviesController(IMovieRepository moviesRepo, IMapper mapper)
        {
            _moviesRepo = moviesRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Movie>))]
        public IActionResult GetMovies(string? movieName)
        {

            // This endpoint has an optional parameter called movieName.
            // If the parameter is null, then send a list of all movies.
            // If the parameter is not null, then send a movie entity that contains the string in movieName.

            // E.g.:
            // GET: api/Movies -> returns a list.
            // GET: api/Movies?movieName=abc -> returns individual movie entity that contains "abc" in the title.

            if (!String.IsNullOrEmpty(movieName))
            {
                var movie = _mapper.Map<MovieDTO>(_moviesRepo.GetMovieByName(movieName));
                
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(movie);
            }
            else
            {
                var movies = _mapper.Map<List<MovieDTO>>(_moviesRepo.GetMovies());

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(movies);
            }
        }

        [HttpGet("{movieId}")]
        [ProducesResponseType(200, Type = typeof(Movie))]
        public IActionResult GetMovie(int movieId)
        {
            // If the movie doesn't exist, return the NotFound validation.
            if (!_moviesRepo.MovieExists(movieId))
            {
                return NotFound();
            }

            var movie = _mapper.Map<MovieDTO>(_moviesRepo.GetMovie(movieId));

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(movie);
        }

        [HttpGet("ByRating/{ratingId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Movie>))]
        public IActionResult GetMovieByRating(int ratingId)
        {
            var movies = _mapper.Map<List<MovieDTO>>(_moviesRepo.GetMoviesByRating(ratingId));

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(movies);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult CreateMovie([FromBody] MovieDTO newMovie)
        {
            // If the newMovie object is null, then return the BadRequest response.
            if (newMovie == null)
            {
                return BadRequest(ModelState);
            }

            // Check if the movie already exist.
            var checkSameMovie = _moviesRepo.GetMovies().Where(m => m.Name.Trim().ToUpper() == newMovie.Name.Trim().ToUpper()).FirstOrDefault();

            // If the movie already exist, then return the statuscode 422 with the error message.
            if (checkSameMovie != null)
            {
                ModelState.AddModelError("", "Movie already exist");
                return StatusCode(422, ModelState);
            }

            // If the modelstate is invalid, then return the badrequest response.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Map the MovieDTO object into Movie.
            var movieMap = _mapper.Map<Movie>(newMovie);

            // If the CreateMovie method returns false, then return StatusCode 500.
            if (!_moviesRepo.CreateMovie(movieMap))
            {
                ModelState.AddModelError("", "Something went wrong.");
                return StatusCode(500, ModelState);
            }

            // If everything went smoothly, then return the Ok message.
            return Ok("New movie added to the list");
        }
    }
}
