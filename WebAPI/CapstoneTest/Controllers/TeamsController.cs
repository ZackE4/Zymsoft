using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CapstoneTest.Models;
using Microsoft.Extensions.Configuration;
using CapstoneTest.Data.Repositories.Concrete;
using CapstoneTest.Models.Request;
using CapstoneTest.Data.Entities.Concrete;
using CapstoneTest.Data.Repositories.Interfaces;

namespace CapstoneTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : BaseAPIController
    {
        private readonly CapstoneContext _context;

        protected ITeamRepository TeamRepositry { get; set; }

        public TeamsController(IConfiguration configuration) : base(configuration)
        {
            this.TeamRepositry = new TeamRepository(this.ConnectionString);
        }

        [HttpGet]
        public async Task<ActionResult> GetById(string apiToken, int id)
        {
            if (!await this.IsAPITokenValid(apiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }

            return new OkObjectResult(await this.TeamRepositry.GetAsync(id));
        }

        [HttpGet("ByLeague")]
        public async Task<ActionResult> GetByLeague(string apiToken, string leagueKey)
        {
            if (!await this.IsAPITokenValid(apiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }

            string leagueId = await this.GetLeagueId(leagueKey);

            if (leagueId == null)
            {
                return new BadRequestObjectResult("League Not Found");
            }

            return new OkObjectResult(await this.TeamRepositry.GetByLeagueAsync(Convert.ToInt32(leagueId)));
        }

        [HttpPost("Add")]
        public async Task<ActionResult> AddTeam(AddTeamRequest request)
        {
            if (!await this.IsAPITokenValid(request.ApiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }

            string leagueId = await this.GetLeagueId(request.LeagueKey);

            if (leagueId == null)
            {
                return new BadRequestObjectResult("League Not Found");
            }

            Team team = new Team
            {
                TeamName = request.TeamName,
                CoachName = request.CoachName,
                Logo = String.IsNullOrEmpty(request.Logo) ? Configuration.GetValue<string>("DefaultTeamLogo") : request.Logo,
                LeagueId = Convert.ToInt32(leagueId)
            };

            var newTeam = await this.TeamRepositry.AddAsync(team);

            return new OkObjectResult(newTeam);
        }

        [HttpPost("Edit")]
        public async Task<ActionResult> UpdateTeam(UpdateTeamRequest request)
        {
            if (!await this.IsAPITokenValid(request.ApiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }

            string leagueId = await this.GetLeagueId(request.LeagueKey);

            if (leagueId == null)
            {
                return new BadRequestObjectResult("League Not Found");
            }

            var team = await this.TeamRepositry.GetAsync(request.TeamId);

            if (team == null)
            {
                return new BadRequestObjectResult("Team Not Found");
            }

            team.TeamName = request.TeamName;
            team.CoachName = request.CoachName;
            team.Logo = String.IsNullOrEmpty(request.Logo) ? Configuration.GetValue<string>("DefaultTeamLogo") : request.Logo;

            var updatedTeam = await this.TeamRepositry.UpdateAsync(team);

            return new OkObjectResult(updatedTeam);
        }
    }
}