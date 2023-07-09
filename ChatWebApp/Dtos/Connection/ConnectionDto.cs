namespace ChatAppAPI.Dtos.Connection
{
    public class ConnectionDto
    {
        public string ConnectionID { get; set; }
        public string UserAgent { get; set; }
        public bool Connected { get; set; }
        public Guid UserId { get; set; }
    }
}
