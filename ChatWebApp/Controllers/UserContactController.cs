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

namespace ChatAppAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserContactController : ControllerBase
    {
        private readonly DataContext _context;

        public UserContactController(DataContext context)
        {
            _context = context;
        }

        // GET: api/UserContact
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserContact>>> GetUserContacts()
        {
          if (_context.UserContacts == null)
          {
              return NotFound();
          }
            return await _context.UserContacts.ToListAsync();
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
        [HttpPost]
        public async Task<ActionResult<UserContact>> PostUserContact(UserContact userContact)
        {
          if (_context.UserContacts == null)
          {
              return Problem("Entity set 'DataContext.UserContacts'  is null.");
          }
            _context.UserContacts.Add(userContact);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserContact", new { id = userContact.Id }, userContact);
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
