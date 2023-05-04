namespace ChatAppAPI.Entities
{
    public class UserContact
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ContactId { get; set; }
        public User Contact { get; set; }
    }
}
