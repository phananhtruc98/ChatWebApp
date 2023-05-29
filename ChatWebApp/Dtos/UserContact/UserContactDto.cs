using ChatAppAPI.Entities;

namespace ChatAppAPI.Dtos.UserContact
{
    public class UserContactDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Bio { get; set; }
    }
}
