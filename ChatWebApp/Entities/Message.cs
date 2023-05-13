namespace ChatAppAPI.Entities
{
    public class Message : IAuditable
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid ConversationParticipantId { get; set; }
        public ConversationParticipant ConversationParticipant{ get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public User? CreatedBy { get; set; }
        public User? ModifiedBy { get; set; }
    }
}
