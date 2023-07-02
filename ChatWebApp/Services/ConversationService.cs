using AutoMapper;
using ChatAppAPI.Authorization;
using ChatAppAPI.Data;
using ChatAppAPI.Dtos.Conversation;
using ChatAppAPI.Entities;
using ChatAppAPI.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
namespace ChatAppAPI.Services
{
    public interface IConversationService
    {
        Task<Conversation> CreateConversation(Conversation conversationForCreationDto);
        Task<IEnumerable<ConversationDto>> GetConversations(Guid userId);
        Task<ConversationInfoDto> GetConversation(Guid id);
    }
    public class ConversationService : IConversationService
    {
        private DataContext _context;
        private readonly IMapper _mapper;
        private IHubContext<AccountHub> _accountHub;
        public ConversationService(DataContext context, IMapper mapper, IHubContext<AccountHub> accountHub)
        {
            _context = context;
            _mapper = mapper;
            _accountHub = accountHub;
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
        private bool ConversationExists(Guid id)
        {
            return (_context.Conversations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public async Task<ConversationInfoDto> GetConversation(Guid id)
        {
            var conversation = _context.Conversations?.FirstOrDefault(e => e.Id == id);
            if (conversation == null) throw new ArgumentNullException("Conversation is null");
            var participants = _context.ConversationParticipants.Where(x => x.ConversationId == conversation.Id);
            ConversationInfoDto conversationInfoDto = new ConversationInfoDto();
            conversationInfoDto.Id = conversation.Id;
            conversationInfoDto.Name = conversation.Name;
            conversationInfoDto.Participants = participants.ToList();
            return conversationInfoDto;
        }
        public async Task<IEnumerable<ConversationDto>> GetConversations(Guid userId)
        {
            if (_context.Conversations == null)
            {
                throw new ArgumentNullException("Conversations is null");
            }

            var user = _context.Users.Include(x => x.ConversationParticipants).SingleOrDefault(x => x.Id == userId);
            var conversationIds = user.ConversationParticipants?.Select(c => c.ConversationId).ToList();
            var conversations = _context.Conversations.Where(x => conversationIds.Contains(x.Id));
            var conversationDtos = new List<ConversationDto>();
            foreach (var conversation in conversations)
            {
                var conversationDto = _mapper.Map<ConversationDto>(conversation);
                var lastMessage = _context.Messages.Where(x=>x.ConversationParticipant.ConversationId == conversationDto.Id).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                conversationDto.LastMessage = lastMessage.Content;
                conversationDto.LastSender = lastMessage.CreatedBy.FullName;
                conversationDtos.Add(conversationDto);
            }

            return conversationDtos;
        }
    }
}
