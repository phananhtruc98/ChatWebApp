using ChatAppAPI.Entities;

namespace ChatAppAPI.Dtos.Message
{
    public class MessageDto
    {
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public User? CreatedBy { get; set; }
    }
}
