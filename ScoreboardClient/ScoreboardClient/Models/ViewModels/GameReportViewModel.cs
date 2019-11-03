using ScoreboardClient.Data.Entities.Concrete;
using ScoreboardClient.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models.ViewModels
{
    public class GameReportViewModel
    {
        public IGame Game { get; set; }
        public ITeam HomeTeam { get; set; }
        public ITeam AwayTeam { get; set; }
        public ILeague League { get; set; }
        public List<Player> HomeTeamPlayerList { get; set; }
        public List<Player> AwayTeamPlayerList { get; set; }
        public List<ScoringLog> ScoringLogs { get; set; }
        public List<FoulLog> FoulLogs { get; set; }
    }
}
