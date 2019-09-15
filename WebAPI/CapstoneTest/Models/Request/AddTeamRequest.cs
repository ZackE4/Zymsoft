using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Models.Request
{
    public class AddTeamRequest
    {
        public virtual string ApiToken { get; set; }
        public virtual string TeamName { get; set; }
        public virtual string CoachName { get; set; }
        public virtual string Logo { get; set; }
        public virtual string LeagueKey { get; set; }
    }
}
