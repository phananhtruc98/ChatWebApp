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
        Task<MessageForCreation> CreateMessage(Guid userId, MessageForCreation messageForCreation);
        Task<IEnumerable<MessageDto>> GetMessageByConversationId(Guid conversationId);
    }
    public class MessageService : IMessageService
    {

        private DataContext _context;
        private readonly IMapper _mapper;
        public MessageService(DataContext context, IMapper mapper, IHubContext<ChatHub> chatHub
            )
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<MessageForCreation> CreateMessage(Guid userId, MessageForCreation messageForCreation)
        {
            var message = _mapper.Map<Message>(messageForCreation);
            if (_context.Messages == null)
            {
                throw new ArgumentNullException("Messages is null");
            }
            message.CreatedDate = DateTime.Now;
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            message.CreatedBy = user;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            var resMessage = _mapper.Map<MessageForCreation>(message);
            return resMessage;
        }
        public async Task<IEnumerable<MessageDto>> GetMessageByConversationId(Guid conversationId)
        {
            var participants = _context.ConversationParticipants.Where(x => x.ConversationId == conversationId).Select(p => p.Id);

            var messages = _context.Messages.Include(x=>x.ConversationParticipant.User).Where(m => participants.Contains(m.ConversationParticipantId));

            var messageDtos = _mapper.Map<IEnumerable<MessageDto>>(messages);

            return messageDtos;
        }
    }
}
