using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ScoreboardClient.Data.Entities.Concrete;
using ScoreboardClient.Models.ViewModels;
using RestSharp;

namespace ScoreboardClient.Controllers
{
    public class ScreenController : BaseController
    {
        public ScreenController(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IActionResult> Index()
        {
            if (!this.CheckLoginStatus())
            {
                RedirectToAction("Home", "Error", null);
            }

            await this.SetupGameForDemo();

            if(Connector.Game == null)
            {
                RedirectToAction("Home", "Error", null);
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
            var GameTime = new TimeSpan(0, (period - 1) * 12 + (12 - minutes), seconds);
            await SettingsUtil.SetSetting("GameTime", GameTime.ToString());
            Connector.GameScore.GameTime = GameTime;
            return new OkObjectResult("Game Time Saved");
        }

        protected async Task SetupGameForDemo()
        {
            if(String.IsNullOrEmpty(await SettingsUtil.GetSetting("GameId")))
            {
                await SettingsUtil.SetSetting("GameId", "3");
            }

            string errorMsg = "";
            Parameter[] parameters = new Parameter[2];
            parameters[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            parameters[1] = new Parameter("id", await SettingsUtil.GetSetting("GameId"), ParameterType.QueryString);
            Connector.Game = this.ApiClient.Get<Game>("Game", parameters, ref errorMsg);

            parameters = new Parameter[2];
            parameters[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            parameters[1] = new Parameter("id", Connector.Game.HomeTeamId, ParameterType.QueryString);
            Connector.HomeTeam = this.ApiClient.Get<Team>("Teams", parameters, ref errorMsg);

            parameters = new Parameter[2];
            parameters[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            parameters[1] = new Parameter("id", Connector.Game.AwayTeamId, ParameterType.QueryString);
            Connector.AwayTeam = this.ApiClient.Get<Team>("Teams", parameters, ref errorMsg);

            if (String.IsNullOrEmpty(await SettingsUtil.GetSetting("SeasonId")))
            {
                await SettingsUtil.SetSetting("SeasonId", "1");
            }
        }
    }
}