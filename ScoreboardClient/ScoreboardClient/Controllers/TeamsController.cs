using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ScoreboardClient.Models.ViewModels;

namespace ScoreboardClient.Controllers
{
    public class TeamsController : BaseController
    {
        public TeamsController(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IActionResult> Index(string errorMsg, string actionMsg)
        {
            if(!await this.CheckLoginStatus())
            {
                return RedirectToAction("Index", "Home", new { errorMsg = "You must join a league first." });
            }
            if(Connector.League == null)
            {
                return RedirectToAction("Index", "Home", new { errorMsg = "You must join a league first." });
            }

            var viewModel = new TeamsViewModel();
            viewModel.League = Connector.League;

            //TODO ADD GET TEAMS BY LEAGUE API FUNCTION

            return View();
        }
    }
}