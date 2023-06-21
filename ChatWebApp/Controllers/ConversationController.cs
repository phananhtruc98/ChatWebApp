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

namespace ChatAppAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationService _conversationService;
        private readonly IMessageService _messageService;

        public ConversationController(IConversationService conversationService, IMessageService messageService)
        {
            _conversationService = conversationService;
            _messageService = messageService;
        }


        [Route("conversations")]
        [HttpGet]
        public async Task<ActionResult> GetConversations()
        {
            var userId = HttpContext.User.FindFirstValue("userId");
            var rs = await _conversationService.GetConversations(Guid.Parse(userId));
            return Ok(rs);
        }


        [HttpGet("{conversationId}")]
        public async Task<ActionResult<Conversation>> GetConversation(Guid conversationId)
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
    }
}
