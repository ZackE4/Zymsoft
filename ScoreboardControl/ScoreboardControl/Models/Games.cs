using System;
using System.Collections.Generic;

namespace CapstoneTest.Models
{
    public partial class Games
    {
        public Games()
        {
            FoulLogs = new HashSet<FoulLogs>();
            MediaLogs = new HashSet<MediaLogs>();
            ScoringLogs = new HashSet<ScoringLogs>();
        }

        public int GameId { get; set; }
        public DateTime Date { get; set; }
        public string HomeTeamId { get; set; }
        public string AwayTeamId { get; set; }
        public bool GameComplete { get; set; }
        public int SeasonSeasonId { get; set; }

        public Seasons SeasonSeason { get; set; }
        public ICollection<FoulLogs> FoulLogs { get; set; }
        public ICollection<MediaLogs> MediaLogs { get; set; }
        public ICollection<ScoringLogs> ScoringLogs { get; set; }
    }
}
