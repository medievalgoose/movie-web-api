using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieWebApi.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Duration { get; set; }
        
        public int RatingId { get; set; }
        [ForeignKey("RatingId")]
        public Rating? Rating { get; set; }
    }
}
