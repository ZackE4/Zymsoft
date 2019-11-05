using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using ScoreboardClient.Hubs;
using ScoreboardClient.Models.Request.Local;
using ScoreboardClient.Models.Response.Host;
using Xabe.FFmpeg;

namespace ScoreboardClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaControlController : BaseController
    {
        private readonly IHostingEnvironment _env;
        private IHubContext<ScoreboardHub> HubContext { get; set; }

        public MediaControlController(IConfiguration configuration, IHostingEnvironment env, IHubContext<ScoreboardHub> hubContext) : base(configuration)
        {
            _env = env;
            this.HubContext = hubContext;
        }

        [HttpGet("AvailableMedia")]
        public async Task<IActionResult> GetAvailableMedia(string apiToken)
        {
            if (!await this.IsAPITokenValid(apiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }

            var response = new AvailableMediaResponse();
            try
            {
                //Set directory where app should look for FFmpeg 
                FFmpeg.ExecutablesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FFmpeg");
                //Get latest version of FFmpeg. It's great idea if you don't know if you had installed FFmpeg (required for getting video file info).
                await FFmpeg.GetLatestVersion();

                string webRootPath = _env.WebRootPath;
                string videoFolderDirectory = "media";
                var filePath = Path.Combine(webRootPath, videoFolderDirectory);

                var videos = new List<AvailableVideo>();
                var rawMediaFiles = System.IO.Directory.GetFiles(filePath);
                var videoFileNames = new List<string>();
                var imageFileNames = new List<string>();

                foreach (var file in rawMediaFiles)
                {
                    if (Path.GetExtension(file).ToLower() == ".mp4")
                    {
                        videoFileNames.Add(file);
                    }
                    else if (Path.GetExtension(file).ToUpper() == ".APNG" ||
                        Path.GetExtension(file).ToUpper() == ".BMP" ||
                        Path.GetExtension(file).ToUpper() == ".JPG" ||
                        Path.GetExtension(file).ToUpper() == ".JPEG" ||
                        Path.GetExtension(file).ToUpper() == ".PNG" ||
                        Path.GetExtension(file).ToUpper() == ".SVG" ||
                        Path.GetExtension(file).ToUpper() == ".WEBP")
                    {
                        imageFileNames.Add(Path.GetFileName(file));
                    }
                }
                foreach (var videoFile in videoFileNames)
                {
                    IMediaInfo mediaInfo = await MediaInfo.Get(videoFile);
                    var videoDuration = mediaInfo.VideoStreams.First().Duration;
                    videos.Add(new AvailableVideo()
                    {
                        Duration = videoDuration,
                        Filename = Path.GetFileName(videoFile)
                    });
                }

                response = new AvailableMediaResponse()
                {
                    AvailableVideos = videos,
                    AvailableImages = imageFileNames
                };
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult($"Error on Server: {ex.Message}");
            }
            

            return new OkObjectResult(response);
        }

        [HttpPost("PlayVideo")]
        public async Task<ActionResult> PlayVideo(PlayVideoRequest request)
        {
            if (!await this.IsAPITokenValid(request.ApiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }
            if (Connector.Game == null)
            {
                return new BadRequestObjectResult("Game Not Available");
            }

            await this.HubContext.Clients.All.SendAsync("RecievePlayVideo", request.FileName);

            return new OkObjectResult("Success");
        }

        [HttpPost("ShowImage")]
        public async Task<ActionResult> ShowImage(PlayVideoRequest request)
        {
            if (!await this.IsAPITokenValid(request.ApiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }
            if (Connector.Game == null)
            {
                return new BadRequestObjectResult("Game Not Available");
            }

            await this.HubContext.Clients.All.SendAsync("RecieveShowImage", request.FileName);

            return new OkObjectResult("Success");
        }

        [HttpPost("ShowMediaPage")]
        public async Task<ActionResult> ShowMediaPage(BasicLocalRequest request)
        {
            if (!await this.IsAPITokenValid(request.ApiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }
            await this.HubContext.Clients.All.SendAsync("RecieveSwitchMediaPage");

            return new OkObjectResult("Success");
        }

        [HttpPost("ShowScoreboardPage")]
        public async Task<ActionResult> ShowScoreboardPage(BasicLocalRequest request)
        {
            if (!await this.IsAPITokenValid(request.ApiToken))
            {
                return new BadRequestObjectResult("UnAuthorized");
            }
            await this.HubContext.Clients.All.SendAsync("RecieveSwitchScoreboardPage");

            return new OkObjectResult("Success");
        }
    }
}