using System;
using System.Collections.Generic;

namespace CapstoneTest.Models
{
    public partial class Logins
    {
        public int LoginId { get; set; }
        public DateTime LoginTimestamp { get; set; }
        public DateTime Expiry { get; set; }
        public string LoginKey { get; set; }
        public int LeagueLeagueId { get; set; }

        public Leagues LeagueLeague { get; set; }
    }
}
