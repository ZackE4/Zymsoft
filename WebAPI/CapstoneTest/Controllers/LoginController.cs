using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CapstoneTest.Models;
using Newtonsoft.Json;
using CapstoneTest.Data.Repositories.Concrete;
using Microsoft.Extensions.Configuration;
using CapstoneTest.Data.Entities.Concrete;

namespace CapstoneTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : BaseAPIController
    {

        protected LeagueRepository LeagueRepository { get; set; }
        protected LoginRepository LoginRepository { get; set; }

        public LoginController(IConfiguration configuration) : base(configuration)
        {
            this.LeagueRepository = new LeagueRepository(this.ConnectionString);
            this.LoginRepository = new LoginRepository(this.ConnectionString);
        }

        [HttpGet("Test")]
        public async Task<ActionResult> Test()
        {
            var teamRepo = new TeamRepository(this.ConnectionString);
            return new OkObjectResult(await teamRepo.GetAsync(2));
        }

        //GET: api/Login?leagueKey=a&hashedPassword=b
        [HttpGet]
        public async Task<ActionResult> Login(string leagueKey, string hashedPassword)
        {
            //get the league, if not exist return notfound
            var league = await LeagueRepository.GetAsync(leagueKey);
            if (league == null)
            {
                return new BadRequestObjectResult("failed");
            }

            //check hashed password matches if not return error
            if (league.HashPassword != hashedPassword)
            {
                return new BadRequestObjectResult("failed");
            }

            //generate login key, insert into db
            var token = Guid.NewGuid().ToString();

            var login = new Login();
            login.LeagueId = league.LeagueId;
            login.LoginTimestamp = DateTime.Now;
            login.Expiry = DateTime.Now.AddHours(1);
            login.LoginKey = token;


            if (await LoginExistsForLeague(league.LeagueId))
            {
                try
                {
                    var oldLogin = await this.LoginRepository.GetAsync(league.LeagueId);
                    oldLogin.LoginTimestamp = DateTime.Now;
                    oldLogin.Expiry = DateTime.Now.AddHours(1);
                    oldLogin.LoginKey = token;

                    await this.LoginRepository.UpdateAsync(oldLogin);
                }
                catch (Exception ex)
                {
                    // handle somehow
                }
            }
            else
            {
                await this.LoginRepository.AddAsync(login);
            }
            // return login key
            LoginObject loginSuccess = new LoginObject(login);
            loginSuccess.LeagueId = league.LeagueId;
            loginSuccess.LeagueName = league.LeagueName;
            loginSuccess.LeagueKey = league.LeagueKey;
            loginSuccess.Logo = league.Logo;
            return new OkObjectResult(loginSuccess);
        }

        private async Task<bool> LoginExistsForLeague(int leagueId)
        {
            return (await this.LoginRepository.GetAsync(leagueId)) != null;
        }
    }
}