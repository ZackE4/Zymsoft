using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Models.Request
{
    public class CreateGameRequest
    {
        public virtual string ApiToken { get; set; }
        public virtual int HomeTeamId { get; set; }
        public virtual int AwayTeamId { get; set; }
        public virtual int SeasonId { get; set; }
        public virtual string LeagueKey { get; set; }
    }
}
