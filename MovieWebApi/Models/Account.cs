using System.ComponentModel.DataAnnotations;

namespace MovieWebApi.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string? ApiKey { get; set; }
    }
}
