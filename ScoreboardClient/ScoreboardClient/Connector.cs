using ScoreboardClient.Data.Entities.Concrete;
using ScoreboardClient.Data.Entities.Interfaces;
using ScoreboardClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using ScoreboardClient.Models.Response.Client;
using ScoreboardClient.Client;

namespace ScoreboardClient
{
    public static class Connector
    {
        private static string BaseWebApiAddress { get; set; }
        private static GameScore _gameScore { get; set; }

        public static IGame Game { get; set; }
        public static ITeam HomeTeam { get; set; }
        public static ITeam AwayTeam { get; set; }

        public static GameScore GameScore {
            get
            {
                if(Game == null)
                {
                    return null;
                }

                if(_gameScore == null)
                {
                    _gameScore = GetGameScore(Game.GameId);
                }

                return _gameScore;
            }
            set
            {
                _gameScore = value;
            }
        }

        public static ILeague League { get; set; }
        public static ISeason Season { get; set; }

        public static string CurrentApiToken { get; set; }
        public static DateTime ApiTokenExpiry { get; set; }

        public static void SetBaseApiAddress(string baseApiAddress)
        {
            BaseWebApiAddress = baseApiAddress;
        }

        #region Private Methods

        private static GameScore GetGameScore(int gameId)
        {
            var ApiClient = new WebApiClient(BaseWebApiAddress);
            LoginIfNeccesary(ApiClient);

            string errorMsg = "";
            Parameter[] paramList = new Parameter[2];
            paramList[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            paramList[1] = new Parameter("gameId", gameId, ParameterType.QueryString);

            var score = ApiClient.Get<GameScoreResponse>("Scoring/Score", paramList, ref errorMsg);

            Parameter[] paramList2 = new Parameter[2];
            paramList2[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            paramList2[1] = new Parameter("gameId", gameId, ParameterType.QueryString);

            var fouls = ApiClient.Get<GameFoulsResponse>("Scoring/Fouls", paramList, ref errorMsg);

            if (score != null && fouls != null)
            {
                return new GameScore()
                {
                    HomeTeamFouls = fouls.HomeTeamFouls,
                    AwayTeamFouls = fouls.AwayTeamFouls,
                    HomeTeamScore = score.HomeTeamScore,
                    AwayTeamScore = score.AwayTeamScore
                };
            }

            return null;
        }

        private static void LoginIfNeccesary(WebApiClient ApiClient)
        {
            if (CurrentApiToken == null || ApiTokenExpiry < DateTime.Now.AddMinutes(-2))
            {
                string errorMessage = "";
                Parameter[] paramList = new Parameter[2];
                paramList[0] = new Parameter("leagueKey", "ABC123", ParameterType.QueryString);
                paramList[1] = new Parameter("hashedPassword", "2AC9CB7DC02B3C0083EB70898E549B63", ParameterType.QueryString);

                var loginResponse = ApiClient.Get<LoginResponse>("Login", paramList, ref errorMessage);
                if (loginResponse != null)
                {
                    CurrentApiToken = loginResponse.Login.LoginKey;
                    ApiTokenExpiry = loginResponse.Login.Expiry;
                    League = new League() { LeagueId = loginResponse.LeagueId, LeagueName = loginResponse.LeagueName, Logo = loginResponse.Logo };
                }
            }
        }

        #endregion
    }
}
