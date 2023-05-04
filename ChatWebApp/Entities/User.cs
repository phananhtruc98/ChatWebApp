using Azure;
using System.Text.Json.Serialization;

namespace ChatAppAPI.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? isFemale { get; set; }
        public string? Email { get; set; }
        public string? Avatar { get; set; }
        public string? Bio { get; set; }

        [JsonIgnore]
        public string? PasswordHash { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }


        public List<ConversationParticipant> ConversationParticipants { get; } = new();

        public List<UserContact> Contacts { get; set; }

    }
}
