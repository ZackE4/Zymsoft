using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CapstoneTest.Models;

namespace CapstoneTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly CapstoneContext _context;

        public PlayersController(CapstoneContext context)
        {
            _context = context;
        }

        // GET: api/Players
        [HttpGet]
        public IEnumerable<Players> GetPlayers()
        {
            return _context.Players;
        }

        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var players = await _context.Players.FindAsync(id);

            if (players == null)
            {
                return NotFound();
            }

            return Ok(players);
        }

        // GET: api/Players/5
        [HttpGet("PlayerStats/{id}")]
        public async Task<IActionResult> GetPlayerWithStats([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var players = await _context.Players.FindAsync(id);

            if (players == null)
            {
                return NotFound();
            }

            var playerWithStats = new PlayerStats(players, _context);

            return Ok(playerWithStats);
        }

        // POST: api/Players/5
        [HttpPost("{id}")]
        public async Task<IActionResult> UpdatePlayer([FromRoute] int id, [FromBody] Players players)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != players.PlayerId)
            {
                return BadRequest();
            }

            _context.Entry(players).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayersExists(id))
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

        // POST: api/Players
        [HttpPost]
        public async Task<IActionResult> AddPlayer([FromBody] Players players)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Players.Add(players);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlayers", new { id = players.PlayerId }, players);
        }

        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var players = await _context.Players.FindAsync(id);
            if (players == null)
            {
                return NotFound();
            }

            _context.Players.Remove(players);
            await _context.SaveChangesAsync();

            return Ok(players);
        }

        private bool PlayersExists(int id)
        {
            return _context.Players.Any(e => e.PlayerId == id);
        }
    }
}