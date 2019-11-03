using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Models.Request
{
    public class CompleteGameRequest
    {
        public virtual string ApiToken { get; set; }
        public virtual string LeagueKey { get; set; }
        public virtual int GameId { get; set; }
    }
}
