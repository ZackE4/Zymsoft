using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CapstoneTest.Data.Entities.Concrete;
using CapstoneTest.Data.Repositories.Concrete;
using CapstoneTest.Data.Repositories.Interfaces;
using CapstoneTest.Models.Request;
using CapstoneTest.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CapstoneTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoringController : BaseAPIController
    {
        protected IScoringLogRepository ScoringLogRepository { get; set; }
        protected IFoulLogRepository FoulLogRepository { get; set; }
        protected ITeamRepository TeamRepository { get; set; }
        protected IPlayerRepository PlayerRepository { get; set; }
        protected IGameRepository GameRepository { get; set; }

        public ScoringController(IConfiguration configuration) : base(configuration)
        {
            this.ScoringLogRepository = new ScoringLogRepository(this.ConnectionString);
            this.FoulLogRepository = new FoulLogRepository(this.ConnectionString);
            this.TeamRepository = new TeamRepository(this.ConnectionString);
            this.PlayerRepository = new PlayerRepository(this.ConnectionString);
            this.GameRepository = new GameRepository(this.ConnectionString);
        }

        [HttpGet("Score")]
        public async Task<ActionResult> GetScoreByGame(string apiToken, int gameId)
        {
            if (!await this.IsAPITokenValid(apiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }

            var game = await this.GameRepository.GetAsync(gameId);

            if (game == null)
            {
                return new BadRequestObjectResult("Game Not Found");
            }

            var homeTeamScores = await this.ScoringLogRepository.GetByTeamAndGameAsync(game.HomeTeamId, gameId);
            var awayTeamScores = await this.ScoringLogRepository.GetByTeamAndGameAsync(game.AwayTeamId, gameId);

            var hometeamQuery = from score in homeTeamScores
                                group score by score.GameId into scoreGroup
                                select scoreGroup.Sum(x => x.Points);
            var awayTeamQuery = from score in awayTeamScores
                                group score by score.GameId into scoreGroup
                                select scoreGroup.Sum(x => x.Points);

            var resposne = new GameScoreResponse
            {
                HomeTeamScore = hometeamQuery.FirstOrDefault(),
                AwayTeamScore = awayTeamQuery.FirstOrDefault()
            };

            return new OkObjectResult(resposne);
        }

        [HttpGet("Fouls")]
        public async Task<ActionResult> GetFoulsByGame(string apiToken, int gameId)
        {
            if (!await this.IsAPITokenValid(apiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }

            var game = await this.GameRepository.GetAsync(gameId);

            if (game == null)
            {
                return new BadRequestObjectResult("Game Not Found");
            }

            var homeTeamFouls = await this.FoulLogRepository.GetByTeamAndGameAsync(game.HomeTeamId, gameId);
            var awayTeamFouls = await this.FoulLogRepository.GetByTeamAndGameAsync(game.AwayTeamId, gameId);

            var hometeamQuery = from score in homeTeamFouls
                                group score by score.GameId into scoreGroup
                                select scoreGroup.Count();
            var awayTeamQuery = from score in awayTeamFouls
                                group score by score.GameId into scoreGroup
                                select scoreGroup.Count();

            var resposne = new GameFoulsResponse
            {
                HomeTeamFouls = hometeamQuery.FirstOrDefault(),
                AwayTeamFouls = awayTeamQuery.FirstOrDefault()
            };

            return new OkObjectResult(resposne);
        }

        [HttpPost("RecordScore")]
        public async Task<ActionResult> RecordScore(RecordScoreRequest request)
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

            if (await this.GameRepository.GetAsync(request.GameId) == null)
            {
                return new BadRequestObjectResult("Game Not Found");
            }

            if (await this.PlayerRepository.GetAsync(request.PlayerId) == null)
            {
                return new BadRequestObjectResult("Player Not Found");
            }

            ScoringLog log = new ScoringLog
            {
                GameTime = request.GameTime,
                Points = request.Points,
                PlayerId = request.PlayerId,
                GameId = request.GameId
            };

            var newScoringLog = await this.ScoringLogRepository.RecordScore(log);

            return new OkObjectResult(newScoringLog);
        }

        [HttpPost("RecordFoul")]
        public async Task<ActionResult> RecordFoul(RecordFoulRequest request)
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

            if (await this.GameRepository.GetAsync(request.GameId) == null)
            {
                return new BadRequestObjectResult("Game Not Found");
            }

            if (await this.PlayerRepository.GetAsync(request.PlayerId) == null)
            {
                return new BadRequestObjectResult("Player Not Found");
            }

            FoulLog log = new FoulLog
            {
                GameTime = request.GameTime,
                PlayerId = request.PlayerId,
                GameId = request.GameId
            };

            var newScoringLog = await this.FoulLogRepository.RecordFoul(log);

            return new OkObjectResult(newScoringLog);
        }
    }
}