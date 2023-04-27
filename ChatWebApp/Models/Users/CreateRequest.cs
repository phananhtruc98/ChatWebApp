using ChatAppAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace ChatAppAPI.Models.Users
{
    public class CreateRequest
    {
        [Required]
        public string? Title { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MinLength(6)]
        public string? Password { get; set; }
        public string? Avatar { get; set; }
        public string? Bio { get; set; }

        [Required]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
    }
}
