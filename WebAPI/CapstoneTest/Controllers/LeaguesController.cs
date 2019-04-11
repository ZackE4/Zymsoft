using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CapstoneTest.Models;

namespace CapstoneTest.Controllers
{
    public class LeaguesController : Controller
    {
        private readonly CapstoneContext _context;

        public LeaguesController(CapstoneContext context)
        {
            _context = context;
        }

        // GET: Leagues
        public async Task<IActionResult> Index()
        {
            return View(await _context.Leagues.ToListAsync());
        }

        // GET: Leagues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leagues = await _context.Leagues
                .FirstOrDefaultAsync(m => m.LeagueId == id);
            if (leagues == null)
            {
                return NotFound();
            }

            return View(leagues);
        }

        // GET: Leagues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Leagues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LeagueId,LeagueName,Logo,HashPassword,LeagueKey")] Leagues leagues)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leagues);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leagues);
        }

        // GET: Leagues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leagues = await _context.Leagues.FindAsync(id);
            if (leagues == null)
            {
                return NotFound();
            }
            return View(leagues);
        }

        // POST: Leagues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LeagueId,LeagueName,Logo,HashPassword,LeagueKey")] Leagues leagues)
        {
            if (id != leagues.LeagueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leagues);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaguesExists(leagues.LeagueId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(leagues);
        }

        // GET: Leagues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leagues = await _context.Leagues
                .FirstOrDefaultAsync(m => m.LeagueId == id);
            if (leagues == null)
            {
                return NotFound();
            }

            return View(leagues);
        }

        // POST: Leagues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leagues = await _context.Leagues.FindAsync(id);
            _context.Leagues.Remove(leagues);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaguesExists(int id)
        {
            return _context.Leagues.Any(e => e.LeagueId == id);
        }
    }
}
