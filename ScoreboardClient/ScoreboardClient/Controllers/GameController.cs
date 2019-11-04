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
using Microsoft.AspNetCore.Http.Features;
using ScoreboardClient.Data.Entities.Interfaces;

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
                return RedirectToAction("Index", "Home", new { errorMsg = "Your league must be in a season to start games" });
            }
            var viewModel = new GameViewModel();
            viewModel.LocalAPIKey = Configuration["Local.API.Key"];
            var localIpAddress = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            viewModel.LocalAPIAddress = $"{localIpAddress}/api/";
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

            if (Connector.Game != null && viewModel.LeagueTeamList!= null && viewModel.LeagueTeamList.Count > 0)
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
            if(Connector.Game != null)
            {
                if(Convert.ToInt32(await SettingsUtil.GetSetting("GameId")) == Connector.Game.GameId)
                {
                    var gameTime = await SettingsUtil.GetSetting("GameTime");
                    if (!String.IsNullOrEmpty(gameTime))
                    {

                        viewModel.CompleteGameAvaialble = TimeSpan.Parse(gameTime) >= new TimeSpan(0, 48, 0);
                    }
                }
            }

            return View(viewModel);
        }

        public async Task<IActionResult> CompleteGame()
        {
            if (Connector.Game != null)
            {
                if (Convert.ToInt32(await SettingsUtil.GetSetting("GameId")) == Connector.Game.GameId)
                {
                    var gameTime = await SettingsUtil.GetSetting("GameTime");
                    if (!String.IsNullOrEmpty(gameTime))
                    {
                        if(TimeSpan.Parse(gameTime) < new TimeSpan(0, 48, 0))
                        {
                            return RedirectToAction("Index", "Game", new { errorMsg = "Game not eligible for completion" });
                        }

                        var request = new CompleteGameRequest()
                        {
                            ApiToken = Connector.CurrentApiToken,
                            LeagueKey = Connector.League.LeagueKey,
                            GameId = Connector.Game.GameId
                        };

                        string errorMessage = "";
                        var finishedGame = this.ApiClient.Post<Game>("Game/CompleteGame", JsonConvert.SerializeObject(request), ref errorMessage);

                        if(finishedGame != null)
                        {
                            Connector.Game = finishedGame;
                            return RedirectToAction("Index", "Game", new { actionMsg = "Game has been completed and stats recorded" });
                        }
                    }
                }
            }
            return RedirectToAction("Index", "Game", new { errorMsg = "Erro Completing Game" });
        }

        public async Task<IActionResult> GameReport(int gameId)
        {
            if (!await this.CheckLoginStatus())
            {
                return RedirectToAction("Index", "Home", new { errorMsg = "You must join a league first." });
            }
            if (Connector.League == null)
            {
                return RedirectToAction("Index", "Home", new { errorMsg = "You must join a league first." });
            }
            if (Connector.Season == null)
            {
                return RedirectToAction("Index", "Home", new { errorMsg = "Your league must be in a season to start games" });
            }

            var viewModel = new GameReportViewModel();
            viewModel.League = Connector.League;

            string errorMessage = "";
            Parameter[] parameters = new Parameter[2];
            parameters[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            parameters[1] = new Parameter("id", gameId, ParameterType.QueryString);
            var game = this.ApiClient.Get<Game>("Game", parameters, ref errorMessage);

            if(game == null)
            {
                return RedirectToAction("Index", "Home", new { errorMsg = $"Unable to retrieve game report: {errorMessage}" });
            }
            if (!game.GameComplete)
            {
                return RedirectToAction("Index", "Home", new { errorMsg = $"Unable to retrieve game report: Game not yet complete!" });
            }
            viewModel.Game = game;

            parameters = new Parameter[2];
            parameters[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            parameters[1] = new Parameter("gameId", gameId, ParameterType.QueryString);
            var scoringLogs = this.ApiClient.Get<List<ScoringLog>>("Scoring/ScoreLogs", parameters, ref errorMessage);

            if(scoringLogs == null)
            {
                return RedirectToAction("Index", "Home", new { errorMsg = $"Unable to retrieve game report: {errorMessage}" });
            }
            viewModel.ScoringLogs = scoringLogs;

            parameters = new Parameter[2];
            parameters[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            parameters[1] = new Parameter("gameId", gameId, ParameterType.QueryString);
            var foulLogs = this.ApiClient.Get<List<FoulLog>>("Scoring/FoulLogs", parameters, ref errorMessage);

            if (foulLogs == null)
            {
                return RedirectToAction("Index", "Home", new { errorMsg = $"Unable to retrieve game report: {errorMessage}" });
            }
            viewModel.FoulLogs = foulLogs;

            var paramList = new Parameter[2];
            paramList[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            paramList[1] = new Parameter("Id", game.HomeTeamId, ParameterType.QueryString);
            var homeTeam = this.ApiClient.Get<Team>("Teams", paramList, ref errorMessage);

            if (homeTeam == null)
            {
                return RedirectToAction("Index", "Home", new { errorMsg = $"Unable to retrieve game report: {errorMessage}" });
            }
            viewModel.HomeTeam = homeTeam;

            paramList = new Parameter[2];
            paramList[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            paramList[1] = new Parameter("Id", game.AwayTeamId, ParameterType.QueryString);
            var awayTeam = this.ApiClient.Get<Team>("Teams", paramList, ref errorMessage);

            if (homeTeam == null)
            {
                return RedirectToAction("Index", "Home", new { errorMsg = $"Unable to retrieve game report: {errorMessage}" });
            }
            viewModel.AwayTeam = awayTeam;

            paramList = new Parameter[2];
            paramList[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            paramList[1] = new Parameter("teamId", game.HomeTeamId, ParameterType.QueryString);
            var homeTeamPlayers = this.ApiClient.Get<List<Player>>("Players/ByTeam", paramList, ref errorMessage);

            if (homeTeamPlayers == null)
            {
                return RedirectToAction("Index", "Home", new { errorMsg = $"Unable to retrieve game report: {errorMessage}" });
            }
            viewModel.HomeTeamPlayerList = homeTeamPlayers;

            paramList = new Parameter[2];
            paramList[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            paramList[1] = new Parameter("teamId", game.AwayTeamId, ParameterType.QueryString);
            var awayTeamPlayers = this.ApiClient.Get<List<Player>>("Players/ByTeam", paramList, ref errorMessage);

            if (awayTeamPlayers == null)
            {
                return RedirectToAction("Index", "Home", new { errorMsg = $"Unable to retrieve game report: {errorMessage}" });
            }
            viewModel.AwayTeamPlayerList = awayTeamPlayers;

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
                var oldGame = this.ApiClient.Get<Game>("Game", parameters, ref errorMsg);
                if(oldGame.SeasonId == Connector.Season.SeasonId)
                {
                    Connector.Game = oldGame;
                    Connector.GameScore = null;
                }
                else
                {
                    return RedirectToAction("Index", "Game", new { errorMsg = "Last saved game does not belong to this league/season. Create a new game to continue" });
                }
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
            var oldGameId = Convert.ToInt32(await SettingsUtil.GetSetting("GameId"));
            if(oldGameId != Connector.Game.GameId)
            {
                await SettingsUtil.SetSetting("GameTime", "");
                await SettingsUtil.SetSetting("HomeTeamTimeouts", "5");
                await SettingsUtil.SetSetting("AwayTeamTimeouts", "5");
            }
            await SettingsUtil.SetSetting("GameId", Connector.Game.GameId.ToString());
        }
    }
}