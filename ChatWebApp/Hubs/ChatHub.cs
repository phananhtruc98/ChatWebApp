using ChatAppAPI.Dtos.Message;
using ChatAppAPI.Entities;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppAPI.Hubs
{
    public interface IChatHub
    {
        Task<Message> SendMessage(Guid userId, MessageForCreation messageForCreation);
    }
    public class ChatHub: Hub<IChatHub>
    {
    }
}
