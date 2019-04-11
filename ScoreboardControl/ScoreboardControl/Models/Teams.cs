using System;
using System.Collections.Generic;

namespace CapstoneTest.Models
{
    public partial class Teams
    {
        public Teams()
        {
            Players = new HashSet<Players>();
        }

        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string CoachName { get; set; }
        public string Logo { get; set; }
        public int LeagueLeagueId { get; set; }

        public Leagues LeagueLeague { get; set; }
        public ICollection<Players> Players { get; set; }
    }
}
