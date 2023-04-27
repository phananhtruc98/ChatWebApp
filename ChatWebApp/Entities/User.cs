using System.Text.Json.Serialization;

namespace ChatAppAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public int RoleId { get; set; }
        public string? Avatar { get; set; }
        public string? Bio { get; set; }

        [JsonIgnore]
        public string? PasswordHash { get; set; }
    }
}
