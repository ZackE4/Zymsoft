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

            RecordScoreRequest apiRequest = new RecordScoreRequest
            {
                ApiToken = Connector.CurrentApiToken,
                GameTime = request.GameTime,
                Points = request.Points,
                PlayerId = request.PlayerId,
                GameId = Connector.Game.GameId,
                LeagueKey = Connector.League.LeagueKey
            };

            string errorMsg = "";
            var scoringLog = this.ApiClient.Post<ScoringLog>("Scoring/RecordScore", JsonConvert.SerializeObject(apiRequest), ref errorMsg);

            if (scoringLog != null)
            {
                if(request.Side.ToUpper() == "HOME")
                {
                    Connector.GameScore.HomeTeamScore = Connector.GameScore.HomeTeamScore + request.Points;
                }
                else if(request.Side.ToUpper() == "AWAY")
                {
                    Connector.GameScore.AwayTeamScore = Connector.GameScore.AwayTeamScore + request.Points;
                }

                await this.HubContext.Clients.All.SendAsync("updateScore", Connector.GameScore);
                return new OkObjectResult(scoringLog);
            }

            return new BadRequestObjectResult(errorMsg);
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

            RecordFoulRequest apiRequest = new RecordFoulRequest
            {
                ApiToken = Connector.CurrentApiToken,
                GameTime = request.GameTime,
                PlayerId = request.PlayerId,
                GameId = Connector.Game.GameId,
                LeagueKey = Connector.League.LeagueKey
            };

            string errorMsg = "";
            var foulLog = this.ApiClient.Post<FoulLog>("Scoring/RecordFoul", JsonConvert.SerializeObject(apiRequest), ref errorMsg);

            if (foulLog != null)
            {
                if (request.Side.ToUpper() == "HOME")
                {
                    switch((int)(request.GameTime.Minutes / 12))
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
                else if (request.Side.ToUpper() == "AWAY")
                {
                    switch ((int)(request.GameTime.Minutes / 12))
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

                await this.HubContext.Clients.All.SendAsync("updateScore", Connector.GameScore);
                return new OkObjectResult(foulLog);
            }

            return new BadRequestObjectResult(errorMsg);
        }
    }
}