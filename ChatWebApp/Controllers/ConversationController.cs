using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatAppAPI.Data;
using ChatAppAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ChatAppAPI.Services;
using ChatAppAPI.Dtos.Message;
using ChatAppAPI.Dtos.Conversation;
using ChatAppAPI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationService _conversationService;
        private readonly IMessageService _messageService;
        private readonly IConversationParticipantService _conversationParticipantService;
        private IHubContext<ChatHub> _chatHub;
        public ConversationController(IConversationService conversationService, IMessageService messageService, IConversationParticipantService conversationParticipantService, IHubContext<ChatHub> chatHub)
        {
            _conversationService = conversationService;
            _messageService = messageService;
            _conversationParticipantService = conversationParticipantService;
            _chatHub = chatHub;
        }

        [HttpGet("conversations")]
        public async Task<ActionResult> GetConversations()
        {
            var userId = HttpContext.User.FindFirstValue("userId");
            var rs = await _conversationService.GetConversations(Guid.Parse(userId));
            return Ok(rs);
        }


        [HttpGet("{conversationId}")]
        public async Task<ActionResult<ConversationInfoDto>> GetConversation(Guid conversationId)
        {
            var rs = await _conversationService.GetConversation(conversationId);
            return Ok(rs);
        }

        [HttpGet("{conversationId}/messages")]
        public async Task<ActionResult<Conversation>> GetMessagesInConversation(Guid conversationId)
        {
            var rs = await _messageService.GetMessageByConversationId(conversationId);
            return Ok(rs);
        }
        
        [HttpPost("message")]
        public async Task<ActionResult<Message>> SendMessage(MessageForCreation messageForCreation)
        {
            try
            {
                var userId = Guid.Parse(HttpContext.User.FindFirstValue("userId"));
                
                var createdMessage = await _messageService.CreateMessage(userId, messageForCreation);

                var conversationId = await _conversationService.GetConversationIdByMessageId(createdMessage.Id);
                createdMessage.ConversationId = conversationId;
                await _chatHub.Clients.All.SendAsync("SendMessage", createdMessage);

                return Ok(createdMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("first-message")]
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
                var message = new MessageForCreation();
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
