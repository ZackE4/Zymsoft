using ScoreboardClient.Data.Entities.Concrete;
using ScoreboardClient.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models.ViewModels
{
    public class LeagueReportViewModel : LeagueHomeViewModel
    {
        public virtual IPlayerWithPoints TopScoringPlayer { get; set; }
        public virtual IPlayerWithFouls TopFoulingPlayer { get; set; }
    }
}
