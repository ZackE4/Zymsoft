using System;
using System.Collections.Generic;

namespace CapstoneTest.Models
{
    public partial class Seasons
    {
        public Seasons()
        {
            Games = new HashSet<Games>();
        }

        public int SeasonId { get; set; }
        public DateTime SeasonStart { get; set; }
        public DateTime SeasonEnd { get; set; }
        public int LeagueLeagueId { get; set; }

        public Leagues LeagueLeague { get; set; }
        public ICollection<Games> Games { get; set; }
    }
}
