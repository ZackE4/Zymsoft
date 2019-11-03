using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScoreboardClient.Data.Entities.Concrete;
using RestSharp;
using Microsoft.Extensions.Configuration;
using ScoreboardClient.Models.Request.Local;
using ScoreboardClient.Models.Request.Client;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;
using ScoreboardClient.Hubs;
using ScoreboardClient.Models.Response.Host;

namespace ScoreboardClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreboardController : BaseController
    {
        private IHubContext<ScoreboardHub> HubContext { get; set; }

        public ScoreboardController(IConfiguration configuration, IHubContext<ScoreboardHub> hubContext) : base(configuration)
        {
            this.HubContext = hubContext;
        }

        public async Task<ActionResult> CheckConnection(string apiToken)
        {
            if (!await this.IsAPITokenValid(apiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }

            return new OkObjectResult("Success");
        }

        [HttpGet("GetGame")]
        public async Task<ActionResult> GetFullGameData(string apiToken)
        {
            if (!await this.CheckLoginStatus())
            {
                return new BadRequestObjectResult("Something Went Wrong");
            }

            if (!await this.IsAPITokenValid(apiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }

            if (Connector.Game == null)
            {
                return new BadRequestObjectResult("Game Not Available");
            }
            if (Connector.Game.GameComplete)
            {
                return new BadRequestObjectResult("Game Already Complete");
            }
            if (Connector.League == null)
            {
                return new BadRequestObjectResult("League Not Available");
            }
            if (Connector.HomeTeam == null || Connector.AwayTeam == null)
            {
                return new BadRequestObjectResult("Error getting teams.");
            }

            string errorMessage = "";
            var paramList = new Parameter[2];
            paramList[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            paramList[1] = new Parameter("teamId", Connector.HomeTeam.TeamId, ParameterType.QueryString);
            var homeTeamPlayerList = this.ApiClient.Get<List<Player>>("Players/ByTeam", paramList, ref errorMessage);

            paramList = new Parameter[2];
            paramList[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            paramList[1] = new Parameter("teamId", Connector.AwayTeam.TeamId, ParameterType.QueryString);
            var awayTeamPlayerList = this.ApiClient.Get<List<Player>>("Players/ByTeam", paramList, ref errorMessage);


            FullGameResponse response = new FullGameResponse()
            {
                HomeTeam = Connector.HomeTeam,
                AwayTeam = Connector.AwayTeam,
                GameScore = Connector.GameScore,
                HomeTeamRoster = homeTeamPlayerList,
                AwayTeamRoster = awayTeamPlayerList
            };

            return new OkObjectResult(response);

        }

        [HttpGet("GetTeam")]
        public async Task<ActionResult> GetTeamById(string apiToken, int id)
        {
            if (!await this.IsAPITokenValid(apiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }

            if (!await this.CheckLoginStatus())
            {
                return new BadRequestObjectResult("Unable to Login");
            }

            string errorMsg = "";
            Parameter[] paramList = new Parameter[2];
            paramList[0] = new Parameter("apiToken", Connector.CurrentApiToken, ParameterType.QueryString);
            paramList[1] = new Parameter("id", id, ParameterType.QueryString);

            var game = this.ApiClient.Get<Game>("Teams", paramList, ref errorMsg);

            if(game != null)
            {
                return new OkObjectResult(game);
            }

            return new BadRequestObjectResult(errorMsg);
        }

        [HttpPost("CallTimeout")]
        public async Task<ActionResult> CallTimeout(CallTimeoutRequest request)
        {
            if (!await this.IsAPITokenValid(request.ApiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }

            if (request.Side == "Home")
            {
                if (Connector.GameScore.HomeTeamTimeoutsRemaining > 0)
                {
                    Connector.GameScore.HomeTeamTimeoutsRemaining = Connector.GameScore.HomeTeamTimeoutsRemaining - 1;
                }
            }
            else if (request.Side == "Away")
            {
                if (Connector.GameScore.AwayTeamTimeoutsRemaining > 0)
                {
                    Connector.GameScore.AwayTeamTimeoutsRemaining = Connector.GameScore.AwayTeamTimeoutsRemaining - 1;
                }
            }

            await this.HubContext.Clients.All.SendAsync("ReceiveCallTimeout", request.Side);

            return new OkObjectResult("Success");
        }

        [HttpPost("SetTimeouts")]
        public async Task<ActionResult> SetTimeouts(SetTimeoutsRequest request)
        {
            if (!await this.IsAPITokenValid(request.ApiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }
            await this.HubContext.Clients.All.SendAsync("RecieveToggleTimer", "start");

            //whenever you start or stop the timer, return back to the current period (to store for recording fouls)
            int period = Connector.Game.GameComplete ? 4 : (int)(Connector.GameScore.GameTime.Minutes / 12) + 1; if (request.Side == "Home")
            {
                Connector.GameScore.HomeTeamTimeoutsRemaining = request.Timeouts;
            }
            else if (request.Side == "Away")
            {
                Connector.GameScore.AwayTeamTimeoutsRemaining = request.Timeouts;
            }

            await this.HubContext.Clients.All.SendAsync("ReceiveSetTimeout", request.Side, request.Timeouts);

            return new OkObjectResult(period);
        }

        [HttpPost("StartTimer")]
        public async Task<ActionResult> StartTimer(BasicLocalRequest request)
        {
            if (!await this.IsAPITokenValid(request.ApiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }
            await this.HubContext.Clients.All.SendAsync("RecieveToggleTimer", "start");

            //whenever you start or stop the timer, return back to the current period (to store for recording fouls)
            int period = Connector.Game.GameComplete ? 4 : (int)(Connector.GameScore.GameTime.Minutes / 12) + 1;
            return new OkObjectResult(period);
        }

        [HttpPost("StopTimer")]
        public async Task<ActionResult> StopTimer(BasicLocalRequest request)
        {
            if (!await this.IsAPITokenValid(request.ApiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }
            await this.HubContext.Clients.All.SendAsync("RecieveToggleTimer", "stop");

            //whenever you start or stop the timer, return back to the current period (to store for recording fouls)
            int period = Connector.Game.GameComplete ? 4 : (int)(Connector.GameScore.GameTime.Minutes / 12) + 1;
            return new OkObjectResult(period);
        }

        [HttpPost("ResetShotClock")]
        public async Task<ActionResult> ResetShotClock(BasicLocalRequest request)
        {
            if (!await this.IsAPITokenValid(request.ApiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }
            await this.HubContext.Clients.All.SendAsync("RecieveResetShotClock");
            return new OkObjectResult("Success");
        }


        [HttpPost("SetShotClock")]
        public async Task<ActionResult> SetShotClock(SetShotClockRequest request)
        {
            if (!await this.IsAPITokenValid(request.ApiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }
            await this.HubContext.Clients.All.SendAsync("RecieveSetShotClock", request.Value);
            return new OkObjectResult("Success");
        }

        [HttpPost("SetGameClock")]
        public async Task<ActionResult> SetGameClock(SetGameClockRequest request)
        {
            if (!await this.IsAPITokenValid(request.ApiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }
            await this.HubContext.Clients.All.SendAsync("RecieveSetShotClock", request.Minutes, request.Seconds);
            return new OkObjectResult("Success");
        }

        [HttpPost("PlayHorn")]
        public async Task<ActionResult> PlayHorn(BasicLocalRequest request)
        {
            if (!await this.IsAPITokenValid(request.ApiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }
            await this.HubContext.Clients.All.SendAsync("ReceivePlayHorn");
            return new OkObjectResult("Success");
        }

        [HttpPost("RecordScore")]
        public async Task<ActionResult> RecordScore(LocalRecordScoreRequest request)
        {
            if (!await this.CheckLoginStatus())
            {
                return new BadRequestObjectResult("Something Went Wrong");
            }

            if (!await this.IsAPITokenValid(request.ApiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }

            if (Connector.Game == null || Connector.Game.GameComplete)
            {
                return new BadRequestObjectResult("Game Not Available");
            }
            if(Connector.League == null)
            {
                return new BadRequestObjectResult("League Not Available");
            }

            //RecordScoreRequest apiRequest = new RecordScoreRequest
            //{
            //    ApiToken = Connector.CurrentApiToken,
            //    Points = request.Points,
            //    PlayerId = request.PlayerId,
            //    GameId = Connector.Game.GameId,
            //    LeagueKey = Connector.League.LeagueKey
            //};

            //string errorMsg = "";
            //var scoringLog = this.ApiClient.Post<ScoringLog>("Scoring/RecordScore", JsonConvert.SerializeObject(apiRequest), ref errorMsg);

            //if (scoringLog != null)
            //{
            if(request.Side.ToUpper() == "HOME")
            {
                Connector.GameScore.HomeTeamScore = Connector.GameScore.HomeTeamScore + request.Points;
            }
            else if(request.Side.ToUpper() == "AWAY")
            {
                Connector.GameScore.AwayTeamScore = Connector.GameScore.AwayTeamScore + request.Points;
            }

            await this.HubContext.Clients.All.SendAsync("updateScore", Connector.GameScore);
            await this.HubContext.Clients.All.SendAsync("saveScore", request.Points, request.PlayerId);
            return new OkObjectResult("Score Recorded");
            //}

            //return new BadRequestObjectResult(errorMsg);
        }

        [HttpPost("RecordFoul")]
        public async Task<ActionResult> RecordFoul(LocalRecordFoulRequest request)
        {
            if (!await this.CheckLoginStatus())
            {
                return new BadRequestObjectResult("Something Went Wrong");
            }

            if (!await this.IsAPITokenValid(request.ApiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }

            if (Connector.Game == null || Connector.Game.GameComplete)
            {
                return new BadRequestObjectResult("Game Not Available");
            }
            if (Connector.League == null)
            {
                return new BadRequestObjectResult("League Not Available");
            }

            //RecordFoulRequest apiRequest = new RecordFoulRequest
            //{
            //    ApiToken = Connector.CurrentApiToken,
            //    GameTime = request.GameTime,
            //    PlayerId = request.PlayerId,
            //    GameId = Connector.Game.GameId,
            //    LeagueKey = Connector.League.LeagueKey
            //};

            //string errorMsg = "";
            //var foulLog = this.ApiClient.Post<FoulLog>("Scoring/RecordFoul", JsonConvert.SerializeObject(apiRequest), ref errorMsg);

            //if (foulLog != null)
            //{
            if (request.Side.ToUpper() == "HOME")
            {
                switch(request.Period)
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
            else if (request.Side.ToUpper() == "AWAY")
            {
                switch (request.Period)
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

            await this.HubContext.Clients.All.SendAsync("updateScore", Connector.GameScore);
            await this.HubContext.Clients.All.SendAsync("saveFoul", request.PlayerId);
            return new OkObjectResult("Foul Recorded");
            //}

            //return new BadRequestObjectResult(errorMsg);
        }
    }
}