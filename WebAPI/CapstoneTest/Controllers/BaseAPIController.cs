using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using CapstoneTest.Data.Repositories.Concrete;

namespace CapstoneTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseAPIController : ControllerBase
    {
        protected IConfiguration _configuration { get; set; }

        public IConfiguration Configuration => _configuration;

        public string ConnectionString { get; set; }

        public BaseAPIController(IConfiguration configuration)
        {
            this._configuration = configuration;
            this.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> IsAPITokenValid(string ApiToken)
        {
            var LoginRepository = new LoginRepository(this.ConnectionString);

            var login = await LoginRepository.GetByKey(ApiToken);

            if (login != null)
            {
                return login.Expiry > DateTime.Now;
            }

            return false;
        }

        public async Task<string> GetLeagueId(string leagueKey)
        {
            var LeagueRepository = new LeagueRepository(this.ConnectionString);
            var league = await LeagueRepository.GetAsync(leagueKey);

            if (league == null)
            {
                return null;
            }
            return league.LeagueId.ToString();
        }
    }
}