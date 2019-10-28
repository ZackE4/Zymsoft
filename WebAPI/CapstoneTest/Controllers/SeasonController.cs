using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneTest.Data.Repositories.Concrete;
using CapstoneTest.Data.Repositories.Interfaces;
using CapstoneTest.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CapstoneTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeasonController : BaseAPIController
    {
        protected ISeasonRepository SeasonRepository { get; set; }
        protected ILeagueRepository leagueRepository { get; set; }

        public SeasonController(IConfiguration configuration) : base(configuration)
        {
            this.SeasonRepository = new SeasonRepository(this.ConnectionString);
            this.leagueRepository = new LeagueRepository(this.ConnectionString);
        }

        [HttpGet("ByLeague")]
        public async Task<ActionResult> GetSeasonsByLeague(string apiToken, string leagueKey)
        {
            if (!await this.IsAPITokenValid(apiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }

            var league = await this.leagueRepository.GetAsync(leagueKey);

            if (league == null)
            {
                return new BadRequestObjectResult("League Not Found");
            }

            return new OkObjectResult(await this.SeasonRepository.GetByLeagueAsync(league.LeagueId));
        }

        [HttpGet("Current")]
        public async Task<ActionResult> CurrentSeason(string apiToken, string leagueKey)
        {
            if (!await this.IsAPITokenValid(apiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }

            var league = await this.leagueRepository.GetAsync(leagueKey);

            if (league == null)
            {
                return new BadRequestObjectResult("League Not Found");
            }

            return new OkObjectResult(await this.SeasonRepository.GetCurrentAsync(league.LeagueId));
        }

        [HttpPost("New")]
        public async Task<ActionResult> NewSeason(NewSeasonRequest request)
        {
            if (!await this.IsAPITokenValid(request.ApiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }

            var league = await this.leagueRepository.GetAsync(request.LeagueKey);

            if (league == null)
            {
                return new BadRequestObjectResult("League Not Found");
            }

            var lastSeason = await this.SeasonRepository.GetCurrentAsync(league.LeagueId);

            if (lastSeason != null && lastSeason.SeasonEnd > DateTime.Now)
            {
                return new BadRequestObjectResult("Current Season Not Ended");
            }

            return new OkObjectResult(await this.SeasonRepository.CreateAsync(league.LeagueId, DateTime.Now, request.EndDate));
        }
    }
}