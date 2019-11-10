using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ScoreboardClient.Client;
using RestSharp;
using ScoreboardClient.Models.Response.Client;
using ScoreboardClient.Data.Entities.Concrete;

namespace ScoreboardClient.Controllers
{
    public class BaseController : Controller
    {
        protected IConfiguration Configuration { get; set; }
        protected WebApiClient ApiClient { get; set; }

        public BaseController(IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.ApiClient = new WebApiClient(Configuration["Web.API.BasePath"]);
        }

        public async Task<bool> IsAPITokenValid(string ApiKey)
        {
            return ApiKey == Configuration["Local.API.Key"];
        }

        public async Task<bool> CheckLoginStatus()
        {
            if(Connector.CurrentApiToken == null || Connector.ApiTokenExpiry < DateTime.Now.AddMinutes(-2))
            {
                string errorMessage = "";
                Parameter[] paramList = new Parameter[2];
                paramList[0] = new Parameter("leagueKey", await SettingsUtil.GetSetting("SavedLeagueKey"), ParameterType.QueryString);
                paramList[1] = new Parameter("hashedPassword", await SettingsUtil.GetSetting("SavedHashedLeaguePassword"), ParameterType.QueryString);

                var loginResponse = this.ApiClient.Get<LoginResponse>("Login", paramList, ref errorMessage);
                if(loginResponse != null)
                {
                    Connector.CurrentApiToken = loginResponse.Login.LoginKey;
                    Connector.ApiTokenExpiry = loginResponse.Login.Expiry;
                    Connector.League = new League() { LeagueId = loginResponse.LeagueId, LeagueName = loginResponse.LeagueName, Logo = loginResponse.Logo, LeagueKey = loginResponse.LeagueKey };
                    return true;
                }
                return false;
            }

            return true;
        }
    }
}