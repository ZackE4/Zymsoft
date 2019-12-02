using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CapstoneTest.Models;
using Microsoft.Extensions.Configuration;
using CapstoneTest.Data.Repositories.Interfaces;
using CapstoneTest.Data.Repositories.Concrete;
using CapstoneTest.Models.Request;
using CapstoneTest.Data.Entities.Concrete;

namespace CapstoneTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : BaseAPIController
    {
        private readonly CapstoneContext _context;

        protected IPlayerRepository PlayerRepository { get; set; }
        protected ITeamRepository TeamRepository { get; set; }

        public PlayersController(IConfiguration configuration) : base(configuration)
        {
            this.PlayerRepository = new PlayerRepository(this.ConnectionString);
            this.TeamRepository = new TeamRepository(this.ConnectionString);
        }

        [HttpGet]
        public async Task<ActionResult> GetById(string apiToken, int id)
        {
            if (!await this.IsAPITokenValid(apiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }

            return new OkObjectResult(await this.PlayerRepository.GetAsync(id));
        }

        [HttpGet("TopScoring")]
        public async Task<ActionResult> GetTopScoringPlayerByIdAndSeason(string apiToken, int seasonId)
        {
            if (!await this.IsAPITokenValid(apiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }

            return new OkObjectResult(await this.PlayerRepository.GetTopScorerAsync(seasonId));
        }

        [HttpGet("TopFouling")]
        public async Task<ActionResult> GetTopFoulingPlayerByIdAndSeason(string apiToken, int seasonId)
        {
            if (!await this.IsAPITokenValid(apiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }

            return new OkObjectResult(await this.PlayerRepository.GetTopFoulerAsync(seasonId));
        }

        [HttpGet("ByTeam")]
        public async Task<ActionResult> GetByTeam(string apiToken, int teamId)
        {
            if (!await this.IsAPITokenValid(apiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }

            if (await this.TeamRepository.GetAsync(teamId) == null)
            {
                return new BadRequestObjectResult("Team Not Found");
            }

            return new OkObjectResult(await this.PlayerRepository.GetByTeamAsync(teamId));
        }

        [HttpPost("Add")]
        public async Task<ActionResult> AddPlayer(AddPlayerRequest request)
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

            if(await this.TeamRepository.GetAsync(request.TeamId) == null)
            {
                return new BadRequestObjectResult("Team Not Found");
            }

            Player player = new Player
            {
                PlayerNum = request.PlayerNum,
                Position = request.Position,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Picture = String.IsNullOrEmpty(request.Picture) ? Configuration.GetValue<string>("DefaultPlayerLogo") : request.Picture,
                TeamId = request.TeamId
            };

            var newPlayer = await this.PlayerRepository.AddAsync(player);

            return new OkObjectResult(newPlayer);
        }

        [HttpPost("Edit")]
        public async Task<ActionResult> UpdatePlayer(UpdatePlayerRequest request)
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

            if (await this.TeamRepository.GetAsync(request.TeamId) == null)
            {
                return new BadRequestObjectResult("Team Not Found");
            }

            var player = await this.PlayerRepository.GetAsync(request.PlayerId);

            if (player == null)
            {
                return new BadRequestObjectResult("Player Not Found");
            }

            player.PlayerNum = request.PlayerNum;
            player.Position = request.Position;
            player.FirstName = request.FirstName;
            player.LastName = request.LastName;
            player.Picture = String.IsNullOrEmpty(request.Picture) ? Configuration.GetValue<string>("DefaultPlayerLogo") : request.Picture;
            player.TeamId = request.TeamId;

            var updatedPlayer = await this.PlayerRepository.UpdateAsync(player);

            return new OkObjectResult(updatedPlayer);
        }

    }
}