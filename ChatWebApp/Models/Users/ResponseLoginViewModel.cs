using System.ComponentModel.DataAnnotations;

namespace ChatAppAPI.Models.Users
{
    public class ResponseLoginViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
