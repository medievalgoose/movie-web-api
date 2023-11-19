using AutoMapper;
using MovieWebApi.Models;
using MovieWebApi.DTO;

namespace MovieWebApi.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Movie, MovieDTO>();
            CreateMap<Rating, RatingDTO>();
        }
    }
}
