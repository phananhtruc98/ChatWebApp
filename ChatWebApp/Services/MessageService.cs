using ChatAppAPI.Data;
using ChatAppAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatAppAPI.Services
{
    public interface IMessageService
    {
        Task<Message> CreateMessage(Guid userId, Message message);
    }
    public class MessageService : IMessageService
    {
        private readonly DataContext _context;
        public MessageService(DataContext context)
        {
            _context = context;
        }
        public async Task<Message> CreateMessage(Guid userId, Message message)
        {
            if (_context.Messages == null)
            {
                throw new ArgumentNullException("Messages is null");
            }
            message.CreatedDate = DateTime.Now;
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            message.CreatedBy = user;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }
    }
}
