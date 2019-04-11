using System;
using System.Collections.Generic;

namespace CapstoneTest.Models
{
    public partial class Leagues
    {
        public Leagues()
        {
            Logins = new HashSet<Logins>();
            Seasons = new HashSet<Seasons>();
            Teams = new HashSet<Teams>();
        }

        public int LeagueId { get; set; }
        public string LeagueName { get; set; }
        public string Logo { get; set; }
        public string HashPassword { get; set; }
        public string LeagueKey { get; set; }

        public ICollection<Logins> Logins { get; set; }
        public ICollection<Seasons> Seasons { get; set; }
        public ICollection<Teams> Teams { get; set; }
    }
}
