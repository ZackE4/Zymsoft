using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models.Request.Client
{
    public class UndoFoulScoreRequest
    {
        public virtual string ApiToken { get; set; }
        public virtual string LeagueKey { get; set; }
        public virtual int Id { get; set; }
    }
}
