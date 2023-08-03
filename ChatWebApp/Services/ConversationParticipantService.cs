using ChatAppAPI.Data;
using ChatAppAPI.Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ChatAppAPI.Services
{
    public interface IConversationParticipantService
    {
        Task<ConversationParticipant> CreateConversationParticipant(ConversationParticipant conversationParticipant);
        Task<Conversation> GetConversationByParticipants(List<Guid> userIds);
        Task<ConversationParticipant> GetParticipantIdByConversationIdAndUserId(Guid conversationId, Guid userId);
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

        public async Task<Conversation> GetConversationByParticipants(List<Guid> userIds)
        {
            var convIds = _context.ConversationParticipants.Select(x=>x.ConversationId).Distinct().ToList();
            foreach(var convId in convIds)
            {
                var c = _context.ConversationParticipants.Where(x=>x.ConversationId == convId && userIds.Contains(x.UserId));
                var p = userIds.Count;
                if(c.Count() == p)
                {
                    return c.Select(x=>x.Conversation).FirstOrDefault();
                }
            }
            return null;
        }

        public async Task<ConversationParticipant> GetParticipantIdByConversationIdAndUserId(Guid conversationId, Guid userId)
        {
            return await _context.ConversationParticipants.FirstOrDefaultAsync(x => x.ConversationId == conversationId && x.UserId == userId);
        }
    }
}
