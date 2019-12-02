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
        protected ICompleteGameRepository CompleteGameRepository { get; set; }
        protected IScoringLogRepository ScoringLogRepository { get; set; }

        public GameController(IConfiguration configuration) : base(configuration)
        {
            this.GameRepository = new GameRepository(this.ConnectionString);
            this.TeamRepository = new TeamRepository(this.ConnectionString);
            this.CompleteGameRepository = new CompleteGameRepository(this.ConnectionString);
            this.ScoringLogRepository = new ScoringLogRepository(this.ConnectionString);
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

        [HttpGet("CompleteBySeason")]
        public async Task<ActionResult> GetCompleteGamesBySeason(string apiToken, int seasonId)
        {
            if (!await this.IsAPITokenValid(apiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }

            return new OkObjectResult(await this.CompleteGameRepository.GetCompleteGamesBySeason(seasonId));
        }

        [HttpGet("CompleteByTeam")]
        public async Task<ActionResult> GetCompleteGamesByTeamAndSeason(string apiToken, int teamId, int seasonId)
        {
            if (!await this.IsAPITokenValid(apiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }

            return new OkObjectResult(await this.CompleteGameRepository.GetCompleteGamesByTeamAndSeason(teamId, seasonId));
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

            if (!game.GameComplete)
            {

                var homeTeamScoringLogs = await this.ScoringLogRepository.GetByTeamAndGameAsync(game.HomeTeamId, game.GameId);
                var awayTeamScoringLogs = await this.ScoringLogRepository.GetByTeamAndGameAsync(game.AwayTeamId, game.GameId);

                var homeTeamScore = homeTeamScoringLogs.Sum(x => x.Points);
                var awayTeamScore = awayTeamScoringLogs.Sum(x => x.Points);

                var completeGame = new CompleteGame()
                {
                    GameId = game.GameId,
                    Date = game.Date,
                    TieFlag = false
                };
            
                if(homeTeamScore > awayTeamScore)
                {
                    completeGame.WinningTeamId = game.HomeTeamId;
                    completeGame.LosingTeamId = game.AwayTeamId;
                }
                else if(awayTeamScore > homeTeamScore)
                {
                    completeGame.WinningTeamId = game.AwayTeamId;
                    completeGame.LosingTeamId = game.HomeTeamId;
                }
                else
                {
                    completeGame.WinningTeamId = game.HomeTeamId;
                    completeGame.LosingTeamId = game.AwayTeamId;
                    completeGame.TieFlag = true;
                }

                var newCompleteGame = await CompleteGameRepository.CompleteGame(completeGame);

                if(newCompleteGame == null)
                {
                    return new BadRequestObjectResult("Error Completing Game");
                }
            }

            game.GameComplete = true;

            return new OkObjectResult(await this.GameRepository.UpdateAsync(game));
        }
    }
}