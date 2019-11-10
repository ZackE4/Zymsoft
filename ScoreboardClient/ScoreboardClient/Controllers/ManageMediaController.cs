using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ScoreboardClient.Models;
using ScoreboardClient.Models.ViewModels;
using Xabe.FFmpeg;

namespace ScoreboardClient.Controllers
{
    public class ManageMediaController : BaseController
    {
        private readonly IHostingEnvironment _env;

        public ManageMediaController(IConfiguration configuration, IHostingEnvironment env) : base(configuration)
        {
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Video(string errorMsg, string actionMsg)
        {
            //Set directory where app should look for FFmpeg 
            FFmpeg.ExecutablesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FFmpeg");
            //Get latest version of FFmpeg. It's great idea if you don't know if you had installed FFmpeg (required for getting video file info).
            await FFmpeg.GetLatestVersion();

            var viewModel = new ManageVideoViewModel();
            if (!string.IsNullOrEmpty(errorMsg))
            {
                viewModel.Messages.Add(new PageMessage() { Message = errorMsg, Type = MessageType.Error });
            }
            if (!string.IsNullOrEmpty(actionMsg))
            {
                viewModel.Messages.Add(new PageMessage() { Message = actionMsg, Type = MessageType.Success });
            }

            string webRootPath = _env.WebRootPath;
            string videoFolderDirectory = "media";
            var filePath = Path.Combine(webRootPath, videoFolderDirectory);

            viewModel.Videos = new List<VideoFile>();
            var rawMediaFiles = System.IO.Directory.GetFiles(filePath);
            var videoFileNames = new List<string>();

            foreach(var file in rawMediaFiles)
            {
                if (Path.GetExtension(file).ToLower() == ".mp4")
                {
                    videoFileNames.Add(file);
                }
            }
            foreach(var videoFile in videoFileNames)
            {
                IMediaInfo mediaInfo = await MediaInfo.Get(videoFile);
                var videoDuration = mediaInfo.VideoStreams.First().Duration;
                double fileSizeInMb = Convert.ToInt32(mediaInfo.Size) / 1000000.0;
                viewModel.Videos.Add(new VideoFile()
                {
                    Duration = videoDuration,
                    FileSize = fileSizeInMb.ToString("0.##"),
                    FileName = Path.GetFileName(videoFile)
                });
            }

            return View(viewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> UploadVideo(UploadVideo uploadVideo)
        {
            if(uploadVideo.VideoFile.ContentType != "video/mp4")
            {
                return RedirectToAction("Video", "ManageMedia", new { errorMsg = $"Error Uploading File: File must be in mp4 video format." });
            }
            if (!uploadVideo.FileName.ToLower().EndsWith(".mp4"))
            {
                uploadVideo.FileName = $"{uploadVideo.FileName}.mp4";
            }


            string webRootPath = _env.WebRootPath;
            string videoFolderDirectory = "media";
            var filePath = Path.Combine(webRootPath, videoFolderDirectory, uploadVideo.FileName);

            if (System.IO.File.Exists(filePath))
            {
                return RedirectToAction("Video", "ManageMedia", new { errorMsg = $"Error Uploading File: A video file with that name already exists, please choose another file name and try again." });
            }

            try
            {
                if (uploadVideo.VideoFile.Length > 0)
                {

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await uploadVideo.VideoFile.CopyToAsync(stream);
                    }
                }
            }
            catch(Exception ex)
            {
                return RedirectToAction("Video", "ManageMedia", new { errorMsg = $"Error Uploading File: {ex.Message}" });
            }
            return RedirectToAction("Video", "ManageMedia", new { actionMsg = "File uploaded successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteVideo(string fileName)
        {
            string webRootPath = _env.WebRootPath;
            string videoFolderDirectory = "media";
            var filePath = Path.Combine(webRootPath, videoFolderDirectory, fileName);

            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                else
                {
                    return RedirectToAction("Video", "ManageMedia", new { errorMsg = $"{fileName} no longer available." });
                }

                return RedirectToAction("Video", "ManageMedia", new { actionMsg = $"{fileName} deleted." });
            }
            catch(Exception ex)
            {
                return RedirectToAction("Video", "ManageMedia", new { errorMsg = $"Error deleting video: {ex.Message}" });
            }
        }

        public IActionResult Players()
        {
            return View();
        }

        public IActionResult Teams()
        {
            return View();
        }

        public IActionResult Images(string errorMsg, string actionMsg)
        {
            var viewModel = new ManageImageViewModel();
            if (!string.IsNullOrEmpty(errorMsg))
            {
                viewModel.Messages.Add(new PageMessage() { Message = errorMsg, Type = MessageType.Error });
            }
            if (!string.IsNullOrEmpty(actionMsg))
            {
                viewModel.Messages.Add(new PageMessage() { Message = actionMsg, Type = MessageType.Success });
            }

            string webRootPath = _env.WebRootPath;
            string imageFolderDirector = "media";
            var filePath = Path.Combine(webRootPath, imageFolderDirector);

            viewModel.Images = new List<ImageFile>();
            var rawMediaFiles = System.IO.Directory.GetFiles(filePath);
            var imageFileNames = new List<string>();

            foreach (var file in rawMediaFiles)
            {
                if (Path.GetExtension(file).ToUpper() == ".APNG" ||
                    Path.GetExtension(file).ToUpper() == ".BMP" ||
                    Path.GetExtension(file).ToUpper() == ".JPG" ||
                    Path.GetExtension(file).ToUpper() == ".JPEG" ||
                    Path.GetExtension(file).ToUpper() == ".PNG" ||
                    Path.GetExtension(file).ToUpper() == ".SVG" ||
                    Path.GetExtension(file).ToUpper() == ".WEBP")
                {
                    imageFileNames.Add(file);
                }
            }
            foreach (var imageFile in imageFileNames)
            {
                double fileSizeInMb = Convert.ToInt32(new System.IO.FileInfo(Path.Combine(filePath, imageFile)).Length) / 1000000.0;
                viewModel.Images.Add(new ImageFile()
                {
                    FileSize = fileSizeInMb.ToString("0.##"),
                    FileName = Path.GetFileName(imageFile)
                });
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(UploadImage uploadImage)
        {
            if (!uploadImage.ImageFile.ContentType.ToLower().StartsWith("image"))
            {
                return RedirectToAction("Video", "ManageMedia", new { errorMsg = $"Error Uploading File: File must be in valid image format." });
            }

            if (!uploadImage.FileName.ToUpper().EndsWith(".APNG") &&
                !uploadImage.FileName.ToUpper().EndsWith(".BMP") &&
                !uploadImage.FileName.ToUpper().EndsWith(".JPG") &&
                !uploadImage.FileName.ToUpper().EndsWith(".JPEG") &&
                !uploadImage.FileName.ToUpper().EndsWith(".PNG") &&
                !uploadImage.FileName.ToUpper().EndsWith(".SVG") &&
                !uploadImage.FileName.ToUpper().EndsWith(".WEBP"))
            {
                string fileExt = uploadImage.ImageFile.ContentType.Split('/')[1];
                uploadImage.FileName = $"{uploadImage.FileName}.{fileExt}";
            }


            string webRootPath = _env.WebRootPath;
            string videoFolderDirectory = "media";
            var filePath = Path.Combine(webRootPath, videoFolderDirectory, uploadImage.FileName);

            if (System.IO.File.Exists(filePath))
            {
                return RedirectToAction("Images", "ManageMedia", new { errorMsg = $"Error Uploading File: An Image file with that name already exists, please choose another file name and try again." });
            }

            try
            {
                if (uploadImage.ImageFile.Length > 0)
                {

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await uploadImage.ImageFile.CopyToAsync(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Images", "ManageMedia", new { errorMsg = $"Error Uploading File: {ex.Message}" });
            }
            return RedirectToAction("Images", "ManageMedia", new { actionMsg = "File uploaded successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage(string fileName)
        {
            string webRootPath = _env.WebRootPath;
            string videoFolderDirectory = "media";
            var filePath = Path.Combine(webRootPath, videoFolderDirectory, fileName);

            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                else
                {
                    return RedirectToAction("Images", "ManageMedia", new { errorMsg = $"{fileName} no longer available." });
                }

                return RedirectToAction("Images", "ManageMedia", new { actionMsg = $"{fileName} deleted." });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Images", "ManageMedia", new { errorMsg = $"Error deleting image: {ex.Message}" });
            }
        }
    }
}