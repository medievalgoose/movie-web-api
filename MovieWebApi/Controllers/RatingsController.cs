using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieWebApi.DTO;
using MovieWebApi.Interfaces;
using MovieWebApi.Models;

namespace MovieWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        // Dependency injection
        private readonly IRatingRepository _ratingRepo;
        private readonly IMapper _mapper;

        public RatingsController(IRatingRepository ratingRepo, IMapper mapper)
        {
            _ratingRepo = ratingRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Rating>))]
        public IActionResult GetRatings(string? ratingName)
        {
            // Endpoint:
            // GET: api/Movies
            // GET: api/Movies?ratingName

            if (!String.IsNullOrEmpty(ratingName))
            {
                var rating = _mapper.Map<RatingDTO>(_ratingRepo.GetRatingByName(ratingName));

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(rating);
            }
            else
            {
                var ratings = _mapper.Map<List<RatingDTO>>(_ratingRepo.GetRatings());

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                return Ok(ratings);
            }

        }

        [HttpGet("{ratingId}")]
        [ProducesResponseType(200, Type = typeof(Rating))]
        public IActionResult GetRating(int ratingId)
        {
            if (!_ratingRepo.RatingExists(ratingId))
                return NotFound();

            var rating = _mapper.Map<RatingDTO>(_ratingRepo.GetRating(ratingId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(rating);
        }

        [HttpGet("{ratingId}/movies")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Movie>))]
        public IActionResult GetMoviesByRating(int ratingId)
        {
            var listMovies = _mapper.Map<List<MovieDTO>>(_ratingRepo.GetMoviesByRating(ratingId));

            if (listMovies.IsNullOrEmpty())
                return NotFound();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(listMovies);
        }
    }
}
