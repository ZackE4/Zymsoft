using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Models.Response
{
    public class GameScoreResponse
    {
        public virtual int HomeTeamScore { get; set; }
        public virtual int AwayTeamScore { get; set; }
    }
}
