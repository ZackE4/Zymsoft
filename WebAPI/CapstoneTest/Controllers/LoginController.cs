using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CapstoneTest.Models;
using Newtonsoft.Json;

namespace CapstoneTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly CapstoneContext _context;

        public LoginController(CapstoneContext context)
        {
            _context = context;
        }

        //// GET: api/Login
        //[HttpGet]
        //public IEnumerable<Logins> GetLogins()
        //{
        //    return _context.Logins;
        //}

        //GET: api/Login?leagueKey=a&hashedPassword=b
        [HttpGet]
        public async Task<LoginObject> Login(string leagueKey, string hashedPassword)
        {
            //get the league, if not exist return notfound
            var leagues = await _context.Leagues.FirstAsync(l => l.LeagueKey == leagueKey);
            if (leagues == null)
            {
                return null;
            }

            //check hashed password matches if not return error
            if(leagues.HashPassword != hashedPassword)
            {
                return null;
            }

            //generate login key, insert into db
            var token = Guid.NewGuid().ToString();

            Logins login = new Logins();
            login.LeagueLeagueId = leagues.LeagueId;
            login.LoginTimestamp = DateTime.Now;
            login.Expiry = DateTime.Now.AddHours(1);
            login.LoginKey = token;


            if (LoginExistsForLeague(leagues.LeagueId))
            {
                try
                {
                    Logins oldLogin = await _context.Logins.FirstAsync(l => l.LeagueLeagueId == login.LeagueLeagueId);
                    oldLogin.LeagueLeagueId = leagues.LeagueId;
                    oldLogin.LoginTimestamp = DateTime.Now;
                    oldLogin.Expiry = DateTime.Now.AddHours(1);
                    oldLogin.LoginKey = token;

                    _context.Logins.Update(oldLogin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!LoginExistsForLeague(leagues.LeagueId))
                    {
                        return null;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                _context.Logins.Add(login);
                await _context.SaveChangesAsync();
            }
            // return login key
            LoginObject loginSuccess = new LoginObject(login);
            loginSuccess.LeagueId = leagues.LeagueId;
            loginSuccess.LeagueName = leagues.LeagueName;
            loginSuccess.LeagueKey = leagues.LeagueKey;
            loginSuccess.Logo = leagues.Logo;
            return loginSuccess;
        }

        //    // GET: api/Login/5
        //    [HttpGet("{id}")]
        //    public async Task<IActionResult> GetLogins([FromRoute] int id)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        var logins = await _context.Logins.FindAsync(id);

        //        if (logins == null)
        //        {
        //            return NotFound();
        //        }

        //        return Ok(logins);
        //    }

        //    // PUT: api/Login/5
        //    [HttpPut("{id}")]
        //    public async Task<IActionResult> PutLogins([FromRoute] int id, [FromBody] Logins logins)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        if (id != logins.LoginId)
        //        {
        //            return BadRequest();
        //        }

        //        _context.Entry(logins).State = EntityState.Modified;

        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!LoginsExists(id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }

        //        return NoContent();
        //    }

        //    // POST: api/Login
        //    [HttpPost]
        //    public async Task<IActionResult> PostLogins([FromBody] Logins logins)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        _context.Logins.Add(logins);
        //        await _context.SaveChangesAsync();

        //        return CreatedAtAction("GetLogins", new { id = logins.LoginId }, logins);
        //    }

        //    // DELETE: api/Login/5
        //    [HttpDelete("{id}")]
        //    public async Task<IActionResult> DeleteLogins([FromRoute] int id)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        var logins = await _context.Logins.FindAsync(id);
        //        if (logins == null)
        //        {
        //            return NotFound();
        //        }

        //        _context.Logins.Remove(logins);
        //        await _context.SaveChangesAsync();

        //        return Ok(logins);
        //    }

        private bool LoginsExists(int id)
        {
            return _context.Logins.Any(e => e.LoginId == id);
        }

        private bool LoginExistsForLeague(int leaguId)
        {
            return _context.Logins.Any(e => e.LeagueLeagueId == leaguId);
        }
    }
}