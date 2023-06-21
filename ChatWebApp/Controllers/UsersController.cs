using ChatAppAPI.Authorization;
using ChatAppAPI.Dtos.File;
using ChatAppAPI.Entities;
using ChatAppAPI.Models.Users;
using ChatAppAPI.Services;
using Imagekit.Models;
using Imagekit.Sdk;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using System.IO;
namespace ChatAppAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
        [Route("suggestions")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetSuggestions()
        {
            var userId = HttpContext.User.FindFirstValue("userId");
            var suggestions = _userService.GetSuggestions(userId);
            return Ok(suggestions);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = _userService.GetById(id);
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, UserForUpdateDto model)
        {
            _userService.Update(id, model);
            return Ok(new { message = "User updated successfully" });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> PostUser(UserForCreationDto model)
        {
            _userService.Register(model);
            return Ok(new { message = "Registration successful" });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginDto user)
        {
            var result = _userService.Login(user.Email, user.Password);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            _userService.Delete(id);
            return Ok(new { message = "User deleted successfully" });
        }

        [Route("avatar")]
        [HttpPost]
        public async Task<ActionResult> PostFile(IFormFile file)
        {
            var userId = HttpContext.User.FindFirstValue("userId");

            if (userId != null)
            {
                var uploadedAvatar = await _userService.SaveAvatar(file);
                var user = await _userService.UpdateAvatar(Guid.Parse(userId), uploadedAvatar.name);
                return Ok(user);
            }
            return NoContent();
        }

        [Route("conversations")]
        [HttpGet]
        public async Task<ActionResult> GetConversations()
        {
            var userId = HttpContext.User.FindFirstValue("userId");
            var rs = await _userService.GetConversations(Guid.Parse(userId));
            return Ok(rs);
        }
    }
}
