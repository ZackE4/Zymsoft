using ScoreboardClient.Data.Entities.Concrete;
using ScoreboardClient.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models.ViewModels
{
    public class LeagueHomeViewModel : BaseAdminViewModel
    {
        public virtual ILeague League { get; set; }
        public virtual ISeason Season { get; set; }

        public virtual List<Team> LeagueTeamList { get; set; }
        public virtual List<CompleteGame> LeagueCompleteGameList { get; set; }
    }
}
