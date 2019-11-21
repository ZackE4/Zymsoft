using ScoreboardClient.Data.Entities.Concrete;
using ScoreboardClient.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models.ViewModels
{
    public class PlayerReportViewModel
    {
        public virtual IPlayer Player { get; set; }
        public virtual ITeam Team { get; set; }
        public virtual ILeague League { get; set; }
        public virtual List<ScoringLog> SeasonScoringLogs {get;set;}
        public virtual List<FoulLog> SeasonFoulLogs {get;set;}
        public virtual List<Game> SeasonGamesPlayedIn { get; set; }
    }
}
