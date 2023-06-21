using ChatAppAPI.Data;
using ChatAppAPI.Dtos.Conversation;
using ChatAppAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatAppAPI.Services
{
    public interface IConversationService
    {
        Task<Conversation> CreateConversation(Conversation conversationForCreationDto);
    }
    public class ConversationService : IConversationService
    {
        private readonly DataContext _context;
        public ConversationService(DataContext context)
        {
            _context = context;
        }
        public async Task<Conversation> CreateConversation(Conversation conversation)
        {
            if (_context.Conversations == null)
            {
                throw new ArgumentNullException("Conversations is null");
            }
            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();
            return conversation;
        }        
    }
}
