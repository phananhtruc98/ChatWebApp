using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatAppAPI.Data;
using ChatAppAPI.Entities;

namespace ChatAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationParticipantController : ControllerBase
    {
        private readonly DataContext _context;

        public ConversationParticipantController(DataContext context)
        {
            _context = context;
        }

        // GET: api/ConversationParticipant
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConversationParticipant>>> GetConversationParticipants()
        {
          if (_context.ConversationParticipants == null)
          {
              return NotFound();
          }
            return await _context.ConversationParticipants.ToListAsync();
        }

        // GET: api/ConversationParticipant/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConversationParticipant>> GetConversationParticipant(Guid id)
        {
          if (_context.ConversationParticipants == null)
          {
              return NotFound();
          }
            var conversationParticipant = await _context.ConversationParticipants.FindAsync(id);

            if (conversationParticipant == null)
            {
                return NotFound();
            }

            return conversationParticipant;
        }

        // PUT: api/ConversationParticipant/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConversationParticipant(Guid id, ConversationParticipant conversationParticipant)
        {
            if (id != conversationParticipant.Id)
            {
                return BadRequest();
            }

            _context.Entry(conversationParticipant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConversationParticipantExists(id))
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

        // POST: api/ConversationParticipant
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ConversationParticipant>> PostConversationParticipant(ConversationParticipant conversationParticipant)
        {
          if (_context.ConversationParticipants == null)
          {
              return Problem("Entity set 'DataContext.ConversationParticipants'  is null.");
          }
            _context.ConversationParticipants.Add(conversationParticipant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConversationParticipant", new { id = conversationParticipant.Id }, conversationParticipant);
        }

        // DELETE: api/ConversationParticipant/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConversationParticipant(Guid id)
        {
            if (_context.ConversationParticipants == null)
            {
                return NotFound();
            }
            var conversationParticipant = await _context.ConversationParticipants.FindAsync(id);
            if (conversationParticipant == null)
            {
                return NotFound();
            }

            _context.ConversationParticipants.Remove(conversationParticipant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConversationParticipantExists(Guid id)
        {
            return (_context.ConversationParticipants?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
