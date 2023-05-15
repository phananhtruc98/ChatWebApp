using ChatAppAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace ChatAppAPI.Models.Users
{
    public class CreateRequest
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
