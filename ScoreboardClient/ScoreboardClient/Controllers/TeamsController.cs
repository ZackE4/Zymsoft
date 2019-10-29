﻿using System;
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

namespace ScoreboardClient.Controllers
{
    public class TeamsController : BaseController
    {
        public TeamsController(IConfiguration configuration) : base(configuration)
        {
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

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Team team)
        {
            AddTeamRequest apiRequest = new AddTeamRequest
            {
                ApiToken = Connector.CurrentApiToken,
                LeagueKey = Connector.League.LeagueKey,
                TeamName = team.TeamName,
                CoachName = team.CoachName,
                Logo = team.Logo
            };

            string errorMessage = "";
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

            paramList = new Parameter[2];
            paramList[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            paramList[1] = new Parameter("teamId", id, ParameterType.QueryString);
            viewModel.Players = this.ApiClient.Get<List<Player>>("Players/ByTeam", paramList, ref errorMessage);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Team team)
        {
            UpdateTeamRequest apiRequest = new UpdateTeamRequest
            {
                ApiToken = Connector.CurrentApiToken,
                LeagueKey = Connector.League.LeagueKey,
                TeamName = team.TeamName,
                CoachName = team.CoachName,
                Logo = team.Logo,
                TeamId = team.TeamId
            };

            string errorMessage = "";
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
    }
}