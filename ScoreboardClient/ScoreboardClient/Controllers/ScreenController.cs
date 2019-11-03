using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ScoreboardClient.Data.Entities.Concrete;
using ScoreboardClient.Models.ViewModels;
using RestSharp;
using ScoreboardClient.Models.Request.Client;
using Newtonsoft.Json;

namespace ScoreboardClient.Controllers
{
    public class ScreenController : BaseController
    {
        public ScreenController(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IActionResult> Index()
        {
            if (!await this.CheckLoginStatus())
            {
                return RedirectToAction("Index", "Game", new { errorMsg = "Authorization Error" });
            }

            await this.SetupGame();

            if(Connector.Game == null)
            {
                return RedirectToAction("Index", "Game", new { errorMsg = "No Game Currently Loaded" });
            }

            if (Connector.Game.GameComplete)
            {
                return RedirectToAction("Index", "Game", new { errorMsg = "Loaded game already complete." });
            }

            var viewModel = new ScreenViewModel();
            viewModel.Game = Connector.Game;
            viewModel.HomeTeam = Connector.HomeTeam;
            viewModel.AwayTeam = Connector.AwayTeam;
            viewModel.GameScore = Connector.GameScore;
            viewModel.League = Connector.League;
            string gameTime = await SettingsUtil.GetSetting("GameTime");
            if (!String.IsNullOrEmpty(gameTime))
            {
                viewModel.GameTime = TimeSpan.Parse(gameTime);
            }
            else
            {
                viewModel.GameTime = new TimeSpan(0, 0, 0);
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveGameTime(int period, int minutes, int seconds)
        {
            var GameTime = new TimeSpan(0, (period-1) * 12, 0) + (new TimeSpan(0, 12, 0) - new TimeSpan(0,minutes,seconds));
            await SettingsUtil.SetSetting("GameTime", GameTime.ToString());
            Connector.GameScore.GameTime = GameTime;
            return new OkObjectResult("Game Time Saved");
        }

        [HttpPost]
        public async Task<IActionResult> SaveRecordedPoints(int period, int minutes, int seconds, int points, int playerId)
        {
            var GameTime = new TimeSpan(0, (period - 1) * 12, 0) + (new TimeSpan(0, 12, 0) - new TimeSpan(0, minutes, seconds));
            if (!await this.CheckLoginStatus())
            {
                return new BadRequestObjectResult("Something Went Wrong");
            }

            if (Connector.Game == null || Connector.Game.GameComplete)
            {
                return new BadRequestObjectResult("Game Not Available");
            }
            if (Connector.League == null)
            {
                return new BadRequestObjectResult("League Not Available");
            }

            RecordScoreRequest apiRequest = new RecordScoreRequest
            {
                ApiToken = Connector.CurrentApiToken,
                GameTime = GameTime,
                Points = points,
                PlayerId = playerId,
                GameId = Connector.Game.GameId,
                LeagueKey = Connector.League.LeagueKey
            };

            string errorMsg = "";
            var scoringLog = this.ApiClient.Post<ScoringLog>("Scoring/RecordScore", JsonConvert.SerializeObject(apiRequest), ref errorMsg);

            if (scoringLog != null)
            {
                return new OkObjectResult("Score Saved");
            }

            return new BadRequestObjectResult(errorMsg);
        }

        [HttpPost]
        public async Task<IActionResult> SaveRecordedFoul(int period, int minutes, int seconds, int playerId)
        {
            var GameTime = new TimeSpan(0, (period - 1) * 12, 0) + (new TimeSpan(0, 12, 0) - new TimeSpan(0, minutes, seconds));
            if (!await this.CheckLoginStatus())
            {
                return new BadRequestObjectResult("Something Went Wrong");
            }
            if (Connector.Game == null || Connector.Game.GameComplete)
            {
                return new BadRequestObjectResult("Game Not Available");
            }
            if (Connector.League == null)
            {
                return new BadRequestObjectResult("League Not Available");
            }

            RecordFoulRequest apiRequest = new RecordFoulRequest
            {
                ApiToken = Connector.CurrentApiToken,
                GameTime = GameTime,
                PlayerId = playerId,
                GameId = Connector.Game.GameId,
                LeagueKey = Connector.League.LeagueKey
            };

            string errorMsg = "";
            var foulLog = this.ApiClient.Post<FoulLog>("Scoring/RecordFoul", JsonConvert.SerializeObject(apiRequest), ref errorMsg);

            if (foulLog != null)
            {
                return new OkObjectResult("Foul Saved");
            }

            return new BadRequestObjectResult(errorMsg);
        }

        protected async Task SetupGame()
        {
            if(String.IsNullOrEmpty(await SettingsUtil.GetSetting("GameId")))
            {
                await SettingsUtil.SetSetting("GameId", "3");
            }

            string errorMsg = "";
            Parameter[] parameters = new Parameter[2];
            if (Connector.Game == null)
            {
                parameters[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
                parameters[1] = new Parameter("id", await SettingsUtil.GetSetting("GameId"), ParameterType.QueryString);
                var oldGame = this.ApiClient.Get<Game>("Game", parameters, ref errorMsg);
                if (oldGame.SeasonId == Connector.Season.SeasonId)
                {
                    Connector.Game = oldGame;
                }
            }

            if(Connector.Game != null)
            {
                parameters = new Parameter[2];
                parameters[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
                parameters[1] = new Parameter("id", Connector.Game.HomeTeamId, ParameterType.QueryString);
                Connector.HomeTeam = this.ApiClient.Get<Team>("Teams", parameters, ref errorMsg);

                parameters = new Parameter[2];
                parameters[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
                parameters[1] = new Parameter("id", Connector.Game.AwayTeamId, ParameterType.QueryString);
                Connector.AwayTeam = this.ApiClient.Get<Team>("Teams", parameters, ref errorMsg);
            }
        }
    }
}