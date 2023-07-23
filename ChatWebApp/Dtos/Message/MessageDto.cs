using ChatAppAPI.Entities;

namespace ChatAppAPI.Dtos.Message
{
    public class MessageDto
    {
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public User? CreatedBy { get; set; }
    }

    public class MessageForCreation: MessageDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid? ConversationParticipantId { get; set; }
        public Guid? ConversationId { get; set;}
    }
}
