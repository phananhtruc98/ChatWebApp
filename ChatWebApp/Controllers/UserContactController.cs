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
using ChatAppAPI.Services;
using ChatAppAPI.Dtos.UserContact;
using System.Security.Claims;

namespace ChatAppAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserContactController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;
        private readonly IUserContactService _userContactService;

        public UserContactController(DataContext context, IUserService userService, IUserContactService userContactService)
        {
            _context = context;
            _userService = userService;
            _userContactService = userContactService;
        }

        // GET: api/UserContact
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserContactDto>>> GetUserContacts()
        {
            var userId = HttpContext.User.FindFirstValue("userId");
            if(userId != null)
            {
                return Ok(_userContactService.GetUserContactDtos(userId));
            }
            return NoContent();
        }

        // GET: api/UserContact/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserContact>> GetUserContact(Guid id)
        {
            if (_context.UserContacts == null)
            {
                return NotFound();
            }
            var userContact = await _context.UserContacts.FindAsync(id);

            if (userContact == null)
            {
                return NotFound();
            }

            return userContact;
        }

        // PUT: api/UserContact/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserContact(Guid id, UserContact userContact)
        {
            if (id != userContact.Id)
            {
                return BadRequest();
            }

            _context.Entry(userContact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserContactExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserContact
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<UserContact>> PostUserContact(UserContactForCreationDto userContactRequest)
        {
            var userId = HttpContext.User.FindFirstValue("userId");
            var rs = await _userContactService.AddUserContact(userId, userContactRequest.ContactId.ToString());
            if(rs != null)
            {
                return Ok(rs);
            }
            return NoContent();
        }

        // DELETE: api/UserContact/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserContact(Guid id)
        {
            if (_context.UserContacts == null)
            {
                return NotFound();
            }
            var userContact = await _context.UserContacts.FindAsync(id);
            if (userContact == null)
            {
                return NotFound();
            }

            _context.UserContacts.Remove(userContact);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserContactExists(Guid id)
        {
            return (_context.UserContacts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
