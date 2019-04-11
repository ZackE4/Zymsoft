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
    public class ScoringLogsController : ControllerBase
    {
        private readonly CapstoneContext _context;

        public ScoringLogsController(CapstoneContext context)
        {
            _context = context;
        }

        // GET: api/ScoringLogs
        [HttpGet]
        public IEnumerable<ScoringLogs> GetScoringLogs()
        {
            return _context.ScoringLogs;
        }

        // GET: api/ScoringLogs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetScoringLogs([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var scoringLogs = await _context.ScoringLogs.FindAsync(id);

            if (scoringLogs == null)
            {
                return NotFound();
            }

            return Ok(scoringLogs);
        }

        // PUT: api/ScoringLogs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutScoringLogs([FromRoute] int id, [FromBody] ScoringLogs scoringLogs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != scoringLogs.ScoringLogId)
            {
                return BadRequest();
            }

            _context.Entry(scoringLogs).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScoringLogsExists(id))
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

        // POST: api/ScoringLogs
        [HttpPost]
        public async Task<IActionResult> PostScoringLogs([FromBody] ScoringLogs scoringLogs)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ScoringLogs.Add(scoringLogs);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetScoringLogs", new { id = scoringLogs.ScoringLogId }, scoringLogs);
        }

        // DELETE: api/ScoringLogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScoringLogs([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var scoringLogs = await _context.ScoringLogs.FindAsync(id);
            if (scoringLogs == null)
            {
                return NotFound();
            }

            _context.ScoringLogs.Remove(scoringLogs);
            await _context.SaveChangesAsync();

            return Ok(scoringLogs);
        }

        private bool ScoringLogsExists(int id)
        {
            return _context.ScoringLogs.Any(e => e.ScoringLogId == id);
        }
    }
}