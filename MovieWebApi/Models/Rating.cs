using System.ComponentModel.DataAnnotations;

namespace MovieWebApi.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        
        public string RatingName { get; set; }

        public ICollection<Movie>? Movies { get; set; }
    }
}
