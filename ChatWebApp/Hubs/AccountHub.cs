using ChatAppAPI.Authorization;
using ChatAppAPI.Dtos.Connection;
using ChatAppAPI.Dtos.UserContact;
using ChatAppAPI.Entities;
using ChatAppAPI.Models.Users;
using ChatAppAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;

namespace ChatAppAPI.Hubs
{
    public interface IAccountHub
    {
        public Task UpdateProfile(UserContactDto userForUpdateDtos);
    }
    public class AccountHub : Hub<IAccountHub>
    {
        private readonly IUserService _userService;
        private readonly IConnectionService _connectionService;
        private readonly IJwtUtils _jwtUtils;
        public AccountHub(IUserService userService, IJwtUtils jwtUtils, IConnectionService connectionService)
        {
            _userService = userService;
            _jwtUtils = jwtUtils;
            _connectionService = connectionService;
        }
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext != null)
            {
                var token = httpContext.Request.Query["access_token"].ToString();
                var userId = token != null ? _jwtUtils.ValidateToken(token) : null;
                if (!string.IsNullOrEmpty(userId.ToString()))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, "ChatUsers");
                    await _userService.UpdateOnlineStatus(Guid.Parse(userId.ToString()), true, new ConnectionDto { ConnectionID = Context.ConnectionId, UserAgent = httpContext.Request.Headers["User-Agent"], Connected = true });
                    await base.OnConnectedAsync();
                }
            }

        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                await _connectionService.DeleteAsync(Context.ConnectionId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, "ChatUsers");
                await base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
