﻿using AutoMapper;
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
        private readonly IAccountRepository _accountRepo;
        private readonly IMapper _mapper;

        public RatingsController(IRatingRepository ratingRepo, IMapper mapper, IAccountRepository accountRepo)
        {
            _ratingRepo = ratingRepo;
            _mapper = mapper;
            _accountRepo = accountRepo;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Rating>))]
        public IActionResult GetRatings([FromHeader] string ApiKey, string? ratingName)
        {
            // Endpoint:
            // GET: api/Movies
            // GET: api/Movies?ratingName

            if (!_accountRepo.ApiKeyVerified(ApiKey))
            {
                return StatusCode(401);
            }

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
        public IActionResult GetRating([FromHeader] string ApiKey, int ratingId)
        {
            if (!_accountRepo.ApiKeyVerified(ApiKey))
                return StatusCode(401);

            if (!_ratingRepo.RatingExists(ratingId))
                return NotFound();

            var rating = _mapper.Map<RatingDTO>(_ratingRepo.GetRating(ratingId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(rating);
        }

        [HttpGet("{ratingId}/movies")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Movie>))]
        public IActionResult GetMoviesByRating([FromHeader] string ApiKey, int ratingId)
        {
            if (!_accountRepo.ApiKeyVerified(ApiKey))
                return StatusCode(401);

            var listMovies = _mapper.Map<List<MovieDTO>>(_ratingRepo.GetMoviesByRating(ratingId));

            if (listMovies.IsNullOrEmpty())
                return NotFound();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(listMovies);
        }

        [HttpPost]
        public IActionResult CreateRating([FromHeader] string ApiKey, [FromBody] RatingDTO newRating)
        {
            if (!_accountRepo.ApiKeyVerified(ApiKey))
                return StatusCode(401);

            if (newRating == null)
                return BadRequest();

            // Check if there's a same Rating in the database.
            var sameRating = _ratingRepo.GetRatings()
                .Where(r => r.RatingName.Trim().ToUpper() == newRating.RatingName.Trim().ToUpper())
                .FirstOrDefault();

            if (sameRating != null)
            {
                ModelState.AddModelError("", "Rating already exists.");
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var ratingMap = _mapper.Map<Rating>(newRating);

            if (!_ratingRepo.CreateRating(ratingMap))
            {
                ModelState.AddModelError("", "Something went wrong.");
                return BadRequest();
            }

            return Ok("Data successfully added to the database.");
        }

        [HttpPut("{ratingId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateRating([FromHeader] string ApiKey, int ratingId, [FromBody] RatingDTO updateRating)
        {
            if (!_accountRepo.ApiKeyVerified(ApiKey))
                return StatusCode(401);

            if (updateRating == null)
                return BadRequest();

            if (ratingId != updateRating.Id)
                return BadRequest();

            if (!_ratingRepo.RatingExists(ratingId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var ratingMap = _mapper.Map<Rating>(updateRating);

            if (!_ratingRepo.UpdateRating(ratingMap))
                return BadRequest();

            return NoContent();
        }

        [HttpDelete("{ratingId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRating([FromHeader] string ApiKey, int ratingId)
        {
            if (!_accountRepo.ApiKeyVerified(ApiKey))
                return StatusCode(401);

            if (!_ratingRepo.RatingExists(ratingId))
                return NotFound();

            var deleteRating = _ratingRepo.GetRating(ratingId);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!_ratingRepo.DeleteRating(deleteRating))
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
