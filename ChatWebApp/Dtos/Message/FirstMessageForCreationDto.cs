namespace ChatAppAPI.Dtos.Message
{
    public class FirstMessageForCreationDto
    {        
        public string Content { get; set; }
        public string? Name { get; set; }
        public List<string> Participants { get; set; }
        public string? Avatar { get; set; }
        public string? Sender { get; set; }
    }
}
