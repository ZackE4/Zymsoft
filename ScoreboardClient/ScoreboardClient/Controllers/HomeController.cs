using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScoreboardClient.Models;
using RestSharp;
using Microsoft.Extensions.Configuration;
using ScoreboardClient.Client;
using ScoreboardClient.Models.Response.Client;
using ScoreboardClient.Data.Entities.Concrete;
using System.Text;
using System.Security.Cryptography;
using ScoreboardClient.Models.ViewModels;
using ScoreboardClient.Models.Request.Client;
using Newtonsoft.Json;

namespace ScoreboardClient.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IActionResult> Index(string errorMsg, string actionMsg)
        {
            await this.CheckLoginStatus();

            var viewModel = new LeagueHomeViewModel();
            if (!string.IsNullOrEmpty(errorMsg))
            {
                viewModel.Messages.Add(new PageMessage() { Message = errorMsg, Type = MessageType.Error });
            }
            if (!string.IsNullOrEmpty(actionMsg))
            {
                viewModel.Messages.Add(new PageMessage() { Message = actionMsg, Type = MessageType.Success });
            }
            viewModel.League = Connector.League;

            if(Connector.League != null)
            {
                this.GetCurrentActiveSeason();
                viewModel.Season = Connector.Season;
            }

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult TestPage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> JoinLeague(string leagueKey, string password)
        {
            string errorMessage = "";
            Parameter[] paramList = new Parameter[2];
            paramList[0] = new Parameter("leagueKey", leagueKey, ParameterType.QueryString);
            paramList[1] = new Parameter("hashedPassword", HashPassword(password), ParameterType.QueryString);

            var loginResponse = this.ApiClient.Get<LoginResponse>("Login", paramList, ref errorMessage);
            if (loginResponse != null)
            {
                this.ResetConnectorForNewLeague();
                Connector.CurrentApiToken = loginResponse.Login.LoginKey;
                Connector.ApiTokenExpiry = loginResponse.Login.Expiry;
                Connector.League = new League() { LeagueId = loginResponse.LeagueId, LeagueName = loginResponse.LeagueName, Logo = loginResponse.Logo, LeagueKey = loginResponse.LeagueKey };
                await SettingsUtil.SetSetting("SavedLeagueKey", leagueKey);
                await SettingsUtil.SetSetting("SavedHashedLeaguePassword", HashPassword(password));
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return RedirectToAction("Index", new { errorMsg = $"Unable to Join League: {errorMessage}" });
            }

            return RedirectToAction("Index", new { actionMsg = $"Joined League: {Connector.League.LeagueName}"});
        }

        [HttpPost]
        public async Task<IActionResult> NewSeason(DateTime endDate)
        {
            NewSeasonRequest apiRequest = new NewSeasonRequest
            {
                ApiToken = Connector.CurrentApiToken,
                LeagueKey = Connector.League.LeagueKey,
                EndDate = endDate
            };

            string errorMessage = "";
            var season = this.ApiClient.Post<Season>("Season/New", JsonConvert.SerializeObject(apiRequest), ref errorMessage);

            if(season != null)
            {
                Connector.Season = season;
                return RedirectToAction("Index", new { actionMsg = "New Season Created!" });
            }

            return RedirectToAction("Index", new { errorMsg = errorMessage });

        }


        #region private methods

        private void GetCurrentActiveSeason()
        {
            string errorMessage = "";
            Parameter[] paramList = new Parameter[2];
            paramList[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            paramList[1] = new Parameter("leagueKey", Connector.League.LeagueKey, ParameterType.QueryString);

            var season = this.ApiClient.Get<Season>("Season/Current", paramList, ref errorMessage);
            if(season != null)
            {
                if(season.SeasonEnd > DateTime.Now)
                {
                    Connector.Season = season;
                }
            }
        }

        private string HashPassword(string password)
        {
            string sSourceData = password;
            byte[] tmpSource = ASCIIEncoding.ASCII.GetBytes(sSourceData);
            byte[] tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);


            return ByteArrayToString(tmpHash);
        }

        private static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }

        private void ResetConnectorForNewLeague()
        {
            Connector.Game = null;
            Connector.HomeTeam = null;
            Connector.AwayTeam = null;
            Connector.GameScore = null;
            Connector.Season = null;
        }

        #endregion
    }
}
