using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using ScoreboardClient.Client;
using ScoreboardClient.Data.Entities.Concrete;
using ScoreboardClient.Models.Request.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Hubs
{
    public class ScoreboardHub: Hub
    {
        public async Task ToggleTimer(string action)
        {
            await Clients.All.SendAsync("RecieveToggleTimer", action);
        }

        public async Task RecordScore(string gameTime, string Points, string PlayerId, string side)
        {
            var ApiClient = new WebApiClient("http://142.55.32.86:50291/api");

            RecordScoreRequest apiRequest = new RecordScoreRequest
            {
                ApiToken = Connector.CurrentApiToken,
                GameTime = TimeSpan.Parse(gameTime),
                Points = Convert.ToInt32(Points),
                PlayerId = Convert.ToInt32(PlayerId),
                GameId = Connector.Game.GameId,
                LeagueKey = Connector.League.LeagueKey
            };

            string errorMsg = "";
            var scoringLog = ApiClient.Post<ScoringLog>("Scoring/RecordScore", JsonConvert.SerializeObject(apiRequest), ref errorMsg);

            if (scoringLog != null)
            {
                if (side.ToUpper() == "HOME")
                {
                    Connector.GameScore.HomeTeamScore = Connector.GameScore.HomeTeamScore + Convert.ToInt32(Points);
                }
                else if (side.ToUpper() == "AWAY")
                {
                    Connector.GameScore.AwayTeamScore = Connector.GameScore.AwayTeamScore + Convert.ToInt32(Points);
                }

                await Clients.All.SendAsync("updateScore", Connector.GameScore);
            }
        }
        public async Task RecordFoul(string gameTime, string PlayerId, string side)
        {
            var ApiClient = new WebApiClient("http://142.55.32.86:50291/api");

            RecordFoulRequest apiRequest = new RecordFoulRequest
            {
                ApiToken = Connector.CurrentApiToken,
                GameTime = TimeSpan.Parse(gameTime),
                PlayerId = Convert.ToInt32(PlayerId),
                GameId = Connector.Game.GameId,
                LeagueKey = Connector.League.LeagueKey
            };

            string errorMsg = "";
            var foulLog = ApiClient.Post<FoulLog>("Scoring/RecordFoul", JsonConvert.SerializeObject(apiRequest), ref errorMsg);

            if (foulLog != null)
            {
                if (side.ToUpper() == "HOME")
                {
                    switch ((int)(TimeSpan.Parse(gameTime).Minutes / 12))
                    {
                        case 0:
                            Connector.GameScore.HomeTeamFouls[0]++;
                            break;
                        case 1:
                            Connector.GameScore.HomeTeamFouls[1]++;
                            break;
                        case 2:
                            Connector.GameScore.HomeTeamFouls[2]++;
                            break;
                        case 3:
                            Connector.GameScore.HomeTeamFouls[3]++;
                            break;
                    }
                }
                else if (side.ToUpper() == "AWAY")
                {
                    switch ((int)(TimeSpan.Parse(gameTime).Minutes / 12))
                    {
                        case 0:
                            Connector.GameScore.AwayTeamFouls[0]++;
                            break;
                        case 1:
                            Connector.GameScore.AwayTeamFouls[1]++;
                            break;
                        case 2:
                            Connector.GameScore.AwayTeamFouls[2]++;
                            break;
                        case 3:
                            Connector.GameScore.AwayTeamFouls[3]++;
                            break;
                    }
                }

                await Clients.All.SendAsync("updateScore", Connector.GameScore);
            }
        }
    }
}
