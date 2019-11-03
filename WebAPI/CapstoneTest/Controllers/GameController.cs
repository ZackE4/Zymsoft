using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneTest.Data.Entities.Concrete;
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
    public class GameController : BaseAPIController
    {
        protected IGameRepository GameRepository { get; set; }
        protected ITeamRepository TeamRepository { get; set; }

        public GameController(IConfiguration configuration) : base(configuration)
        {
            this.GameRepository = new GameRepository(this.ConnectionString);
            this.TeamRepository = new TeamRepository(this.ConnectionString);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> CreateGame(CreateGameRequest request)
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

            if (await this.TeamRepository.GetAsync(request.HomeTeamId) == null)
            {
                return new BadRequestObjectResult("Home Team Not Found");
            }

            if (await this.TeamRepository.GetAsync(request.AwayTeamId) == null)
            {
                return new BadRequestObjectResult("Away Team Not Found");
            }

            var newGame = await this.GameRepository.CreateAsync(request.HomeTeamId, request.AwayTeamId, request.SeasonId);

            return new OkObjectResult(newGame);
        }

        [HttpGet]
        public async Task<ActionResult> GetById(string apiToken, int id)
        {
            if (!await this.IsAPITokenValid(apiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }

            return new OkObjectResult(await this.GameRepository.GetAsync(id));
        }

        [HttpPost("CompleteGame")]
        public async Task<ActionResult> CompleteGame(CompleteGameRequest request)
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

            var game = await this.GameRepository.GetAsync(request.GameId);

            if(game == null)
            {
                return new BadRequestObjectResult("Game Not Found");
            }

            game.GameComplete = true;

            return new OkObjectResult(await this.GameRepository.UpdateAsync(game));
        }
    }
}