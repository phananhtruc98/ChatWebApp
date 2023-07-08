using ChatAppAPI.Entities;

namespace ChatAppAPI.Dtos.Conversation
{
    public class ConversationDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Avatar { get; set; }
        public string? LastMessage { get; set; }
        public string? LastSender { get; set; }
        public DateTime?  LastSent { get; set; }
    }

    public class ConversationInfoDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Avatar { get; set; }
        public List<ConversationParticipant> Participants { get; set; }

    }

}
