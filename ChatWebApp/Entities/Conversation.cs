namespace ChatAppAPI.Entities
{
    public class Conversation: IAuditable
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Avatar { get; set; }
        DateTime IAuditable.CreatedDate { get; set; }
        DateTime IAuditable.ModifiedDate { get; set; }
        User IAuditable.CreatedBy { get; set; }
        User IAuditable.ModifiedBy { get; set; }
    }
}
