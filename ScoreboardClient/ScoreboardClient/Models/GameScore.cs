using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models
{
    public class GameScore
    {
        public virtual int HomeTeamScore { get; set; }
        public virtual int AwayTeamScore { get; set; }

        public virtual int[] HomeTeamFouls { get; set; }
        public virtual int[] AwayTeamFouls { get; set; }

        public virtual TimeSpan GameTime { get; set; }
    }
}
