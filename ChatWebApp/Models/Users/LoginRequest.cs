using System.ComponentModel.DataAnnotations;

namespace ChatAppAPI.Models.Users
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
