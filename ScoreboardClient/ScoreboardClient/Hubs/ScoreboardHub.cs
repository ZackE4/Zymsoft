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

        public async Task ResetShotClock()
        {
            await Clients.All.SendAsync("RecieveResetShotClock");
        }

        public async Task SetShotClock(int value)
        {
            await Clients.All.SendAsync("RecieveSetShotClock", value);
        }

        public async Task SetGameClock(int mins, int seconds)
        {
            await Clients.All.SendAsync("RecieveSetGameClock", mins, seconds);
        }

        public async Task PlayHorn()
        {
            await Clients.All.SendAsync("ReceivePlayHorn");
        }

        public async Task CallTimeout(string side)
        {
            if(side == "Home")
            {
                if(Connector.GameScore.HomeTeamTimeoutsRemaining > 0)
                {
                    Connector.GameScore.HomeTeamTimeoutsRemaining = Connector.GameScore.HomeTeamTimeoutsRemaining - 1;
                }
            }else if(side == "Away")
            {
                if (Connector.GameScore.AwayTeamTimeoutsRemaining > 0)
                {
                    Connector.GameScore.AwayTeamTimeoutsRemaining = Connector.GameScore.AwayTeamTimeoutsRemaining - 1;
                }
            }

            await Clients.All.SendAsync("ReceiveCallTimeout", side);
        }

        public async Task SetTimeouts(string side, int timeouts)
        {
            if (side == "Home")
            {
                Connector.GameScore.HomeTeamTimeoutsRemaining = timeouts;
            }
            else if (side == "Away")
            {
                Connector.GameScore.AwayTeamTimeoutsRemaining = timeouts;
            }

            await Clients.All.SendAsync("ReceiveSetTimeout", side, timeouts);
        }

        public async Task RecordScore(string Points, string PlayerId, string side)
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
            await Clients.All.SendAsync("saveScore", Points, PlayerId);
        }

        public async Task RecordFoul(string period, string PlayerId, string side)
        {
            if (side.ToUpper() == "HOME")
            {
                switch (Convert.ToInt32(period))
                {
                    case 1:
                        Connector.GameScore.HomeTeamFouls[0]++;
                        break;
                    case 2:
                        Connector.GameScore.HomeTeamFouls[1]++;
                        break;
                    case 3:
                        Connector.GameScore.HomeTeamFouls[2]++;
                        break;
                    case 4:
                        Connector.GameScore.HomeTeamFouls[3]++;
                        break;
                }
            }
            else if (side.ToUpper() == "AWAY")
            {
                switch (Convert.ToInt32(period))
                {
                    case 1:
                        Connector.GameScore.AwayTeamFouls[0]++;
                        break;
                    case 2:
                        Connector.GameScore.AwayTeamFouls[1]++;
                        break;
                    case 3:
                        Connector.GameScore.AwayTeamFouls[2]++;
                        break;
                    case 4:
                        Connector.GameScore.AwayTeamFouls[3]++;
                        break;
                }
            }

            await Clients.All.SendAsync("updateScore", Connector.GameScore);
            await Clients.All.SendAsync("saveFoul", PlayerId);
        }

        public async Task PlayVideo(string videoFileName)
        {
            await Clients.All.SendAsync("RecievePlayVideo", videoFileName);
        }

        public async Task ShowImage(string imageFileName)
        {
            await Clients.All.SendAsync("RecieveShowImage", imageFileName);
        }

        public async Task SwitchMediaPage()
        {
            await Clients.All.SendAsync("RecieveSwitchMediaPage");
        }

        public async Task SwitchScoreboardPage()
        {
            await Clients.All.SendAsync("RecieveSwitchScoreboardPage");
        }
    }
}
