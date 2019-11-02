using ScoreboardClient.Data.Entities.Concrete;
using ScoreboardClient.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models.Response.Host
{
    public class FullGameResponse
    {
        public ITeam HomeTeam { get; set; }
        public ITeam AwayTeam { get; set; }
        public GameScore GameScore { get; set; }
        public List<Player> HomeTeamRoster { get; set; }
        public List<Player> AwayTeamRoster { get; set; }
    }
}
