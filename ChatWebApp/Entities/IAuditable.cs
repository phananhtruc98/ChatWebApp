namespace ChatAppAPI.Entities
{
    public interface IAuditable
    {
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public User CreatedBy { get; set; }
        public User ModifiedBy { get; set;}
    }
}
