using ChatAppAPI.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ChatAppAPI.Models.Users
{
    public class UserForUpdateDto
    {
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? IsFemale { get; set; }
        public string? Email { get; set; }
        public string? Bio { get; set; }

        private string? _password;
        [MinLength(6)]
        public string? Password
        {
            get => _password;
            set => _password = replaceEmptyWithNull(value);
        }

        private string? _confirmPassword;
        [Compare("Password")]
        public string? ConfirmPassword
        {
            get => _confirmPassword;
            set => _confirmPassword = replaceEmptyWithNull(value);
        }

        // helpers

        private string? replaceEmptyWithNull(string? value)
        {
            // replace empty string with null to make field optional
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }
}
