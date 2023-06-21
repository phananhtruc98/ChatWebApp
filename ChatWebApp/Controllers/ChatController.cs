using ChatAppAPI.Data;
using ChatAppAPI.Dtos;
using ChatAppAPI.Dtos.Message;
using ChatAppAPI.Entities;
using ChatAppAPI.Services;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data.OleDb;
using System.Security.Claims;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

namespace ChatAppAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConversationService _conversationService;
        private readonly IConversationParticipantService _conversationParticipantService;
        private readonly IMessageService _messageService;
        public ChatController(IUserService userService, IConversationService conversationService, IConversationParticipantService conversationParticipantService, IMessageService messageService)
        {
            _userService = userService;
            _conversationService = conversationService;
            _conversationParticipantService = conversationParticipantService;
            _messageService = messageService;
        }

        [HttpPost]
        public async Task<ActionResult<Message>> FirstMessage(FirstMessageForCreationDto firstMessageForCreationDto)
        {
            try
            {
                var userId = Guid.Parse(HttpContext.User.FindFirstValue("userId"));
                // create conversation
                var conversation = new Conversation();
                if (string.IsNullOrEmpty(firstMessageForCreationDto.Name))
                {
                    // WILL CONSIDER WHERE TO HANLE IT 
                    //var contact = _userService.GetById(firstMessageForCreationDto.ContactId);
                    //var user = _userService.GetById(userId);
                    //string nameChat = $"{contact.FullName}, {user.FullName}";
                    //conversation.Name = nameChat;
                }
                conversation.Name = firstMessageForCreationDto.Name;
                if (string.IsNullOrEmpty(firstMessageForCreationDto.Avatar))
                {
                    conversation.Avatar = "default-avatar.png";
                }
                var createdConversation = await _conversationService.CreateConversation(conversation);

                List<(string, Guid)> userList = new List<(string, Guid)>();

                // Create ConversationParticipant
                foreach (var participantUserId in firstMessageForCreationDto.Participants)
                {
                    var userParticipant = new ConversationParticipant();
                    userParticipant.ConversationId = createdConversation.Id;
                    userParticipant.UserId = Guid.Parse(participantUserId);
                    var createdParticipant = await _conversationParticipantService.CreateConversationParticipant(userParticipant);
                    var rs = (participantUserId, createdParticipant.Id);
                    userList.Add(rs);
                }
                // Create message
                var senderParticipantId = userList.Find(x => x.Item1 == firstMessageForCreationDto.Sender).Item2;
                var message = new Message();
                message.ConversationParticipantId = senderParticipantId;
                message.Content = firstMessageForCreationDto.Content;
                var createdMessage = await _messageService.CreateMessage(userId, message);
                return Ok(createdMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
