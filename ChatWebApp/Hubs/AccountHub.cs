using ChatAppAPI.Dtos.UserContact;
using ChatAppAPI.Entities;
using ChatAppAPI.Models.Users;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppAPI.Hubs
{
    public interface IAccountHub
    {
        public Task UpdateProfile(UserContactDto userForUpdateDtos);
    }
    public class AccountHub: Hub<IAccountHub>
    {
       
    }
}
