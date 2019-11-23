using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ScoreboardClient.Models.ViewModels;
using RestSharp;
using ScoreboardClient.Data.Entities.Concrete;
using ScoreboardClient.Data.Entities.Interfaces;
using ScoreboardClient.Models;
using ScoreboardClient.Models.Request.Client;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ScoreboardClient.Controllers
{
    public class TeamsController : BaseController
    {
        private readonly IHostingEnvironment _env;

        public TeamsController(IConfiguration configuration, IHostingEnvironment env) : base(configuration)
        {
            _env = env;
        }

        public async Task<IActionResult> Index(string errorMsg, string actionMsg)
        {
            if(!await this.CheckLoginStatus())
            {
                return RedirectToAction("Index", "Home", new { errorMsg = "You must join a league first." });
            }
            if(Connector.League == null)
            {
                return RedirectToAction("Index", "Home", new { errorMsg = "You must join a league first." });
            }

            var viewModel = new TeamsViewModel();
            viewModel.League = Connector.League;
            if (!string.IsNullOrEmpty(errorMsg))
            {
                viewModel.Messages.Add(new PageMessage() { Message = errorMsg, Type = MessageType.Error });
            }
            if (!string.IsNullOrEmpty(actionMsg))
            {
                viewModel.Messages.Add(new PageMessage() { Message = actionMsg, Type = MessageType.Success });
            }

            string errorMessage = "";
            Parameter[] paramList = new Parameter[2];
            paramList[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            paramList[1] = new Parameter("leagueKey", Connector.League.LeagueKey, ParameterType.QueryString);

            viewModel.Teams = this.ApiClient.Get<List<Team>>("Teams/ByLeague", paramList, ref errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                viewModel.Messages.Add(new PageMessage() { Message = $"Error loading leagues: {errorMessage}", Type = MessageType.Error });
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            if (!await this.CheckLoginStatus())
            {
                return RedirectToAction("Index", "Home", new { errorMsg = "You must join a league first." });
            }
            if (Connector.League == null)
            {
                return RedirectToAction("Index", "Home", new { errorMsg = "You must join a league first." });
            }

            var viewModel = new AddEditTeamViewModel();
            viewModel.AddEdit = AddEditTeam.Add;
            viewModel.League = Connector.League;
            viewModel.Team = new Team();

            string webRootPath = _env.WebRootPath;
            string imageFolderDirector = "teams";
            var filePath = Path.Combine(webRootPath, imageFolderDirector);

            viewModel.TeamImages = new List<ImageFile>();
            var rawMediaFiles = System.IO.Directory.GetFiles(filePath);
            var imageFileNames = new List<string>();

            foreach (var file in rawMediaFiles)
            {
                if (Path.GetExtension(file).ToUpper() == ".APNG" ||
                    Path.GetExtension(file).ToUpper() == ".BMP" ||
                    Path.GetExtension(file).ToUpper() == ".JPG" ||
                    Path.GetExtension(file).ToUpper() == ".JPEG" ||
                    Path.GetExtension(file).ToUpper() == ".PNG" ||
                    Path.GetExtension(file).ToUpper() == ".SVG" ||
                    Path.GetExtension(file).ToUpper() == ".WEBP")
                {
                    imageFileNames.Add(file);
                }
            }
            foreach (var imageFile in imageFileNames)
            {
                double fileSizeInMb = Convert.ToInt32(new System.IO.FileInfo(Path.Combine(filePath, imageFile)).Length) / 1000000.0;
                viewModel.TeamImages.Add(new ImageFile()
                {
                    FileSize = fileSizeInMb.ToString("0.##"),
                    FileName = Path.GetFileName(imageFile)
                });
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Team team)
        {
            string errorMessage = "";
            Parameter[] paramList = new Parameter[2];
            paramList[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            paramList[1] = new Parameter("leagueKey", Connector.League.LeagueKey, ParameterType.QueryString);

            var teamList = this.ApiClient.Get<List<Team>>("Teams/ByLeague", paramList, ref errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return RedirectToAction("Index", "Teams", new { errorMsg = "Error updating team." });
            }

            if (teamList != null && teamList.Where(x => x.TeamName.ToUpper() == team.TeamName.ToUpper()).FirstOrDefault() != null)
            {
                return RedirectToAction("Index", "Teams", new { errorMsg = "A team with that name already exists" });
            }

            AddTeamRequest apiRequest = new AddTeamRequest
            {
                ApiToken = Connector.CurrentApiToken,
                LeagueKey = Connector.League.LeagueKey,
                TeamName = team.TeamName,
                CoachName = team.CoachName,
                Logo = team.Logo
            };

            errorMessage = "";
            var newTeam = this.ApiClient.Post<Team>("Teams/Add", JsonConvert.SerializeObject(apiRequest), ref errorMessage);

            if(newTeam != null)
            {
                return RedirectToAction("Index", "Teams", new { actionMsg = $"{newTeam.TeamName} created in league {Connector.League.LeagueName}" });
            }
            return RedirectToAction("Index", "Teams", new { errorMsg = "Error adding new team." });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!await this.CheckLoginStatus())
            {
                return RedirectToAction("Index", "Home", new { errorMsg = "You must join a league first." });
            }
            if (Connector.League == null)
            {
                return RedirectToAction("Index", "Home", new { errorMsg = "You must join a league first." });
            }

            var viewModel = new AddEditTeamViewModel();
            viewModel.AddEdit = AddEditTeam.Edit;
            viewModel.League = Connector.League;

            string errorMessage = "";
            Parameter[] paramList = new Parameter[2];
            paramList[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            paramList[1] = new Parameter("Id", id, ParameterType.QueryString);

            viewModel.Team = this.ApiClient.Get<Team>("Teams", paramList, ref errorMessage);
            if (viewModel.Team == null)
            {
                return RedirectToAction("Index", "Teams", new { errorMsg = $"Error loading team: {errorMessage}" });
            }

            errorMessage = "";
            paramList = new Parameter[2];
            paramList[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            paramList[1] = new Parameter("teamId", id, ParameterType.QueryString);
            viewModel.Players = this.ApiClient.Get<List<Player>>("Players/ByTeam", paramList, ref errorMessage);

            errorMessage = "";
            paramList = new Parameter[3];
            paramList[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            paramList[1] = new Parameter("teamId", id, ParameterType.QueryString);
            paramList[2] = new Parameter("seasonId", Connector.Season.SeasonId, ParameterType.QueryString);

            viewModel.TeamHistory = this.ApiClient.Get<List<CompleteGame>>("Game/CompleteByTeam", paramList, ref errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return RedirectToAction("Index", "Teams", new { errorMsg = $"Error getting team stats: {errorMessage}" });
            }

            viewModel.TeamHistory = viewModel.TeamHistory.OrderByDescending(x => x.Date).ToList();

            string webRootPath = _env.WebRootPath;
            string imageFolderDirector = "teams";
            var filePath = Path.Combine(webRootPath, imageFolderDirector);

            viewModel.TeamImages = new List<ImageFile>();
            var rawMediaFiles = System.IO.Directory.GetFiles(filePath);
            var imageFileNames = new List<string>();

            foreach (var file in rawMediaFiles)
            {
                if (Path.GetExtension(file).ToUpper() == ".APNG" ||
                    Path.GetExtension(file).ToUpper() == ".BMP" ||
                    Path.GetExtension(file).ToUpper() == ".JPG" ||
                    Path.GetExtension(file).ToUpper() == ".JPEG" ||
                    Path.GetExtension(file).ToUpper() == ".PNG" ||
                    Path.GetExtension(file).ToUpper() == ".SVG" ||
                    Path.GetExtension(file).ToUpper() == ".WEBP")
                {
                    imageFileNames.Add(file);
                }
            }
            foreach (var imageFile in imageFileNames)
            {
                double fileSizeInMb = Convert.ToInt32(new System.IO.FileInfo(Path.Combine(filePath, imageFile)).Length) / 1000000.0;
                viewModel.TeamImages.Add(new ImageFile()
                {
                    FileSize = fileSizeInMb.ToString("0.##"),
                    FileName = Path.GetFileName(imageFile)
                });
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Team team)
        {
            string errorMessage = "";
            Parameter[] paramList = new Parameter[2];
            paramList[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            paramList[1] = new Parameter("leagueKey", Connector.League.LeagueKey, ParameterType.QueryString);

            var teamList = this.ApiClient.Get<List<Team>>("Teams/ByLeague", paramList, ref errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                return RedirectToAction("Index", "Teams", new { errorMsg = "Error updating team." });
            }

            if(teamList != null && teamList.Where(x=>x.TeamName.ToUpper() == team.TeamName.ToUpper() && x.TeamId != team.TeamId).FirstOrDefault() != null)
            {
                return RedirectToAction("Index", "Teams", new { errorMsg = "A team with that name already exists" });
            }

            UpdateTeamRequest apiRequest = new UpdateTeamRequest
            {
                ApiToken = Connector.CurrentApiToken,
                LeagueKey = Connector.League.LeagueKey,
                TeamName = team.TeamName,
                CoachName = team.CoachName,
                Logo = team.Logo,
                TeamId = team.TeamId
            };

            errorMessage = "";
            var updatedTeam = this.ApiClient.Post<Team>("Teams/Edit", JsonConvert.SerializeObject(apiRequest), ref errorMessage);

            if (updatedTeam != null)
            {
                return RedirectToAction("Index", "Teams", new { actionMsg = $"{updatedTeam.TeamName} updated." });
            }

            return RedirectToAction("Index", "Teams", new { errorMsg = "Error updating team." });

        }

        [HttpGet]
        public async Task<IActionResult> AddPlayer(int teamId)
        {
            if (!await this.CheckLoginStatus())
            {
                return RedirectToAction("Index", "Home", new { errorMsg = "You must join a league first." });
            }
            if (Connector.League == null)
            {
                return RedirectToAction("Index", "Home", new { errorMsg = "You must join a league first." });
            }

            var viewModel = new AddEditPlayerViewModel();
            viewModel.AddEdit = AddEditPlayer.Add;
            viewModel.Player = new Player() { TeamId = teamId };

            string webRootPath = _env.WebRootPath;
            string imageFolderDirector = "players";
            var filePath = Path.Combine(webRootPath, imageFolderDirector);

            viewModel.PlayerImages = new List<ImageFile>();
            var rawMediaFiles = System.IO.Directory.GetFiles(filePath);
            var imageFileNames = new List<string>();

            foreach (var file in rawMediaFiles)
            {
                if (Path.GetExtension(file).ToUpper() == ".APNG" ||
                    Path.GetExtension(file).ToUpper() == ".BMP" ||
                    Path.GetExtension(file).ToUpper() == ".JPG" ||
                    Path.GetExtension(file).ToUpper() == ".JPEG" ||
                    Path.GetExtension(file).ToUpper() == ".PNG" ||
                    Path.GetExtension(file).ToUpper() == ".SVG" ||
                    Path.GetExtension(file).ToUpper() == ".WEBP")
                {
                    imageFileNames.Add(file);
                }
            }
            foreach (var imageFile in imageFileNames)
            {
                double fileSizeInMb = Convert.ToInt32(new System.IO.FileInfo(Path.Combine(filePath, imageFile)).Length) / 1000000.0;
                viewModel.PlayerImages.Add(new ImageFile()
                {
                    FileSize = fileSizeInMb.ToString("0.##"),
                    FileName = Path.GetFileName(imageFile)
                });
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddPlayer(Player player)
        {
            AddPlayerRequest apiRequest = new AddPlayerRequest
            {
                ApiToken = Connector.CurrentApiToken,
                LeagueKey = Connector.League.LeagueKey,
                TeamId = player.TeamId,
                FirstName = player.FirstName,
                LastName = player.LastName,
                PlayerNum = player.PlayerNum,
                Position = player.Position,
                Picture = player.Picture
            };

            string errorMessage = "";
            var newPlayer = this.ApiClient.Post<Player>("Players/Add", JsonConvert.SerializeObject(apiRequest), ref errorMessage);

            if (newPlayer != null)
            {
                return RedirectToAction("Edit", "Teams", new { id = player.TeamId });
            }

            return RedirectToAction("Index", "Teams", new { errorMsg = "Error adding new player." });
        }

        [HttpGet]
        public async Task<IActionResult> EditPlayer(int id)
        {
            if (!await this.CheckLoginStatus())
            {
                return RedirectToAction("Index", "Home", new { errorMsg = "You must join a league first." });
            }
            if (Connector.League == null)
            {
                return RedirectToAction("Index", "Home", new { errorMsg = "You must join a league first." });
            }

            var viewModel = new AddEditPlayerViewModel();
            viewModel.AddEdit = AddEditPlayer.Edit;
            string errorMessage = "";
            Parameter[] paramList = new Parameter[2];
            paramList[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            paramList[1] = new Parameter("Id", id, ParameterType.QueryString);

            viewModel.Player = this.ApiClient.Get<Player>("Players", paramList, ref errorMessage);
            if (viewModel.Player == null)
            {
                return RedirectToAction("Index", "Teams", new { errorMsg = $"Error retrieving player info: {errorMessage}" });
            }

            string webRootPath = _env.WebRootPath;
            string imageFolderDirector = "players";
            var filePath = Path.Combine(webRootPath, imageFolderDirector);

            viewModel.PlayerImages = new List<ImageFile>();
            var rawMediaFiles = System.IO.Directory.GetFiles(filePath);
            var imageFileNames = new List<string>();

            foreach (var file in rawMediaFiles)
            {
                if (Path.GetExtension(file).ToUpper() == ".APNG" ||
                    Path.GetExtension(file).ToUpper() == ".BMP" ||
                    Path.GetExtension(file).ToUpper() == ".JPG" ||
                    Path.GetExtension(file).ToUpper() == ".JPEG" ||
                    Path.GetExtension(file).ToUpper() == ".PNG" ||
                    Path.GetExtension(file).ToUpper() == ".SVG" ||
                    Path.GetExtension(file).ToUpper() == ".WEBP")
                {
                    imageFileNames.Add(file);
                }
            }
            foreach (var imageFile in imageFileNames)
            {
                double fileSizeInMb = Convert.ToInt32(new System.IO.FileInfo(Path.Combine(filePath, imageFile)).Length) / 1000000.0;
                viewModel.PlayerImages.Add(new ImageFile()
                {
                    FileSize = fileSizeInMb.ToString("0.##"),
                    FileName = Path.GetFileName(imageFile)
                });
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditPlayer(Player player)
        {
            UpdatePlayerRequest apiRequest = new UpdatePlayerRequest
            {
                ApiToken = Connector.CurrentApiToken,
                LeagueKey = Connector.League.LeagueKey,
                TeamId = player.TeamId,
                FirstName = player.FirstName,
                LastName = player.LastName,
                PlayerNum = player.PlayerNum,
                Position = player.Position,
                Picture = player.Picture,
                PlayerId = player.PlayerId
            };

            string errorMessage = "";
            var updatedPlayer = this.ApiClient.Post<Player>("Players/Edit", JsonConvert.SerializeObject(apiRequest), ref errorMessage);

            if (updatedPlayer != null)
            {
                return RedirectToAction("Edit", "Teams", new { id = player.TeamId });
            }

            return RedirectToAction("Index", "Teams", new { errorMsg = $"Error updating player: {player.FirstName} {player.LastName}" });
        }

        public async Task<IActionResult> PlayerReport(int playerId)
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
                return RedirectToAction("Index", "Home", new { errorMsg = "Your league must be in a season to view seasonal reports" });
            }

            var viewModel = new PlayerReportViewModel();
            viewModel.League = Connector.League;

            string errorMessage = "";
            Parameter[] parameters = new Parameter[2];
            parameters[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            parameters[1] = new Parameter("id", playerId, ParameterType.QueryString);
            var player = this.ApiClient.Get<Player>("Players", parameters, ref errorMessage);

            if (player == null)
            {
                return RedirectToAction("Index", "Home", new { errorMsg = $"Unable to retrieve player report: {errorMessage}" });
            }
            viewModel.Player = player;

            parameters = new Parameter[3];
            parameters[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            parameters[1] = new Parameter("playerId", playerId, ParameterType.QueryString);
            parameters[2] = new Parameter("seasonId", Connector.Season.SeasonId, ParameterType.QueryString);
            var scoringLogs = this.ApiClient.Get<List<ScoringLog>>("Scoring/PlayerScoreLogs", parameters, ref errorMessage);

            if (scoringLogs == null)
            {
                return RedirectToAction("Index", "Home", new { errorMsg = $"Unable to retrieve player report: {errorMessage}" });
            }
            viewModel.SeasonScoringLogs = scoringLogs;
            viewModel.SeasonScoringLogs = viewModel.SeasonScoringLogs.OrderBy(x => x.GameId).ToList();

            parameters = new Parameter[3];
            parameters[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            parameters[1] = new Parameter("playerId", playerId, ParameterType.QueryString);
            parameters[2] = new Parameter("seasonId", Connector.Season.SeasonId, ParameterType.QueryString);
            var foulLogs = this.ApiClient.Get<List<FoulLog>>("Scoring/PlayerFoulLogs", parameters, ref errorMessage);

            if (foulLogs == null)
            {
                return RedirectToAction("Index", "Home", new { errorMsg = $"Unable to retrieve player report: {errorMessage}" });
            }
            viewModel.SeasonFoulLogs = foulLogs;
            viewModel.SeasonFoulLogs = viewModel.SeasonFoulLogs.OrderBy(x => x.GameId).ToList();

            var paramList = new Parameter[2];
            paramList[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            paramList[1] = new Parameter("Id", player.TeamId, ParameterType.QueryString);
            var team = this.ApiClient.Get<Team>("Teams", paramList, ref errorMessage);

            if (team == null)
            {
                return RedirectToAction("Index", "Home", new { errorMsg = $"Unable to retrieve game report: {errorMessage}" });
            }
            viewModel.Team = team;

            List<int> scoringGameIds = viewModel.SeasonScoringLogs.Select(x => x.GameId).Distinct().ToList();
            List<int> foulGameIds = viewModel.SeasonFoulLogs.Select(x => x.GameId).Distinct().ToList();

            foreach(var gameId in scoringGameIds)
            {
                if (!foulGameIds.Contains(gameId))
                {
                    foulGameIds.Add(gameId);
                }
            }

            viewModel.SeasonGamesPlayedIn = new List<Game>();
            foreach(var gameId in foulGameIds)
            {
                paramList = new Parameter[2];
                paramList[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
                paramList[1] = new Parameter("Id", gameId, ParameterType.QueryString);
                var game = this.ApiClient.Get<Game>("Game", paramList, ref errorMessage);

                if(game != null)
                {
                    viewModel.SeasonGamesPlayedIn.Add(game);
                }
            }

            viewModel.SeasonGamesPlayedIn = viewModel.SeasonGamesPlayedIn.OrderByDescending(x => x.Date).ToList();

            return View(viewModel);
        }
    }
}