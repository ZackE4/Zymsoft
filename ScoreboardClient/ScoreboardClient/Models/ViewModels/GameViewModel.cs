using ScoreboardClient.Data.Entities.Concrete;
using ScoreboardClient.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models.ViewModels
{
    public class GameViewModel : BaseAdminViewModel
    {
        public virtual ILeague League { get; set; }
        public virtual IGame Game { get; set; }
        public virtual ITeam HomeTeam { get; set; }
        public virtual ITeam AwayTeam { get; set; }
        public virtual bool gameHasStarted { get; set; }
        public virtual bool SavedGameAvailable { get; set; }
        public virtual List<Team> LeagueTeamList { get; set; }

        public virtual string LocalAPIAddress { get; set; }
        public virtual string LocalAPIKey { get; set; }

        public virtual bool CompleteGameAvaialble { get; set; }
    }
}
