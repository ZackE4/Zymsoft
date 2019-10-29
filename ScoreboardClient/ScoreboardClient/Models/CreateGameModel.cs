using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models
{
    public class CreateGameModel
    {
        public virtual int HomeTeamId { get; set; }
        public virtual int AwayTeamId { get; set; }
    }
}
