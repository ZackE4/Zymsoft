using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Models.Request
{
    public class RecordScoreRequest
    {
        public virtual string ApiToken { get; set; }
        public virtual TimeSpan GameTime { get; set; }
        public virtual int Points { get; set; }
        public virtual int PlayerId { get; set; }
        public virtual int GameId { get; set; }
        public virtual string LeagueKey { get; set; }
    }
}
