using AutoMapper;
using ChatAppAPI.Data;
using ChatAppAPI.Dtos.Message;
using ChatAppAPI.Entities;
using ChatAppAPI.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppAPI.Services
{
    public interface IMessageService
    {
        Task<Message> CreateMessage(Guid userId, Message message);
        Task<IEnumerable<MessageDto>> GetMessageByConversationId(Guid conversationId);
    }
    public class MessageService : IMessageService
    {

        private DataContext _context;
        private readonly IMapper _mapper;
        private IHubContext<AccountHub> _accountHub;
        public MessageService(DataContext context, IMapper mapper, IHubContext<AccountHub> accountHub)
        {
            _context = context;
            _mapper = mapper;
            _accountHub = accountHub;
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
        public async Task<IEnumerable<MessageDto>> GetMessageByConversationId(Guid conversationId)
        {
            var participants = _context.ConversationParticipants.Where(x => x.ConversationId == conversationId).Select(p => p.Id);

            var messages = _context.Messages.Where(m => participants.Contains(m.ConversationParticipantId));

            var messageDtos = _mapper.Map<IEnumerable<MessageDto>>(messages);

            return messageDtos;
        }
    }
}
