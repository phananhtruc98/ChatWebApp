using ChatAppAPI.Dtos.Message;
using ChatAppAPI.Entities;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppAPI.Hubs
{
    public interface IChatHub
    {
        Task SendMessage(MessageForCreation createdMessage);
    }
    public class ChatHub: Hub<IChatHub>
    {
    }
}
