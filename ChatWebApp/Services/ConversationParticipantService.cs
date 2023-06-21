using ChatAppAPI.Data;
using ChatAppAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatAppAPI.Services
{
    public interface IConversationParticipantService
    {
        Task<ConversationParticipant> CreateConversationParticipant(ConversationParticipant conversationParticipant);
    }
    public class ConversationParticipantService : IConversationParticipantService
    {
        private readonly DataContext _context;
        public ConversationParticipantService(DataContext context)
        {
            _context = context;
        }
        public async Task<ConversationParticipant> CreateConversationParticipant(ConversationParticipant conversationParticipant)
        {
            if (_context.ConversationParticipants == null)
            {
                throw new ArgumentNullException("ConversationParticipants is null");
            }
            _context.ConversationParticipants.Add(conversationParticipant);
            await _context.SaveChangesAsync();
            return conversationParticipant;
        }
    }
}
