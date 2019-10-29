using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ScoreboardClient.Models.ViewModels;
using RestSharp;
using ScoreboardClient.Data.Entities.Concrete;
using System.Linq;
using ScoreboardClient.Models;
using ScoreboardClient.Models.Request.Client;
using Newtonsoft.Json;

namespace ScoreboardClient.Controllers
{
    public class GameController : BaseController
    {
        public GameController(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IActionResult> Index(string errorMsg, string actionMsg)
        {
            if (!await this.CheckLoginStatus())
            {
                return RedirectToAction("Index", "Home", new { errorMsg = "You must join a league first." });
            }
            if (Connector.League == null)
            {
                return RedirectToAction("Index", "Home", new { errorMsg = "You must join a league first." });
            }
            if(Connector.Season == null)
            {
                return RedirectToAction("Index", "Home", new { errorMsg = "Your league must be a season to start games" });
            }
            var viewModel = new GameViewModel();
            viewModel.League = Connector.League;
            viewModel.Game = Connector.Game;
            if (!string.IsNullOrEmpty(errorMsg))
            {
                viewModel.Messages.Add(new PageMessage() { Message = errorMsg, Type = MessageType.Error });
            }
            if (!string.IsNullOrEmpty(actionMsg))
            {
                viewModel.Messages.Add(new PageMessage() { Message = actionMsg, Type = MessageType.Success });
            }

            string errorMessage = "";
            Parameter[] parameters = new Parameter[2];
            parameters[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            parameters[1] = new Parameter("leagueKey", Connector.League.LeagueKey, ParameterType.QueryString);
            viewModel.LeagueTeamList = this.ApiClient.Get<List<Team>>("Teams/ByLeague", parameters, ref errorMessage);

            if (Connector.Game != null && viewModel.LeagueTeamList.Count > 0)
            {
                var homeTeamQuery = from team in viewModel.LeagueTeamList
                                    where team.TeamId == Connector.Game.HomeTeamId
                                    select team;
                viewModel.HomeTeam = homeTeamQuery.FirstOrDefault();
                Connector.HomeTeam = homeTeamQuery.FirstOrDefault();
                var awayTeamQuery = from team in viewModel.LeagueTeamList
                                    where team.TeamId == Connector.Game.AwayTeamId
                                    select team;
                viewModel.AwayTeam = awayTeamQuery.FirstOrDefault();
                Connector.AwayTeam = homeTeamQuery.FirstOrDefault();
            }

            if (Connector.GameScore != null)
            {
                if(Connector.GameScore.HomeTeamScore + Connector.GameScore.AwayTeamScore + Connector.GameScore.HomeTeamFouls.Sum() + Connector.GameScore.AwayTeamFouls.Sum() > 0
                    || !string.IsNullOrEmpty(await SettingsUtil.GetSetting("GameTime")))
                {
                    viewModel.gameHasStarted = true;
                }
            }

            viewModel.SavedGameAvailable = !string.IsNullOrEmpty(await SettingsUtil.GetSetting("GameId"));

            return View(viewModel);
        }

        public async Task<IActionResult> LoadLastSavedGamed()
        {
            string errorMsg = "";
            Parameter[] parameters = new Parameter[2];
            if (Connector.Game == null)
            {
                parameters[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
                parameters[1] = new Parameter("id", await SettingsUtil.GetSetting("GameId"), ParameterType.QueryString);
                Connector.Game = this.ApiClient.Get<Game>("Game", parameters, ref errorMsg);
            }

            if(Connector.Game != null)
            {
                SetupConnectorAndSettingsForNewGame();
                return RedirectToAction("Index", "Game", new { actionMsg = $"Recovered saved game!" });
            }
            return RedirectToAction("Index", "Game", new { errorMsg = "Error recovering game." });
        }

        [HttpPost]
        public IActionResult CreateGame(CreateGameModel request)
        {
            CreateGameRequest apiRequest = new CreateGameRequest
            {
                ApiToken = Connector.CurrentApiToken,
                LeagueKey = Connector.League.LeagueKey,
                HomeTeamId = request.HomeTeamId,
                AwayTeamId = request.AwayTeamId,
                SeasonId = Connector.Season.SeasonId
            };

            string errorMessage = "";
            Connector.Game = this.ApiClient.Post<Game>("Game/Create", JsonConvert.SerializeObject(apiRequest), ref errorMessage);

            if (Connector.Game != null)
            {
                SetupConnectorAndSettingsForNewGame();
                return RedirectToAction("Index", "Game", new { actionMsg = $"New game created!" });
            }

            return RedirectToAction("Index", "Game", new { errorMsg = $"Error creating game: {errorMessage}" });
        }

        private async void SetupConnectorAndSettingsForNewGame()
        {
            Connector.HomeTeam = null;
            Connector.AwayTeam = null;
            await SettingsUtil.SetSetting("GameId", Connector.Game.GameId.ToString());
            await SettingsUtil.SetSetting("GameTime", "");
        }
    }
}