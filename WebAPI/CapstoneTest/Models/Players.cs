using System;
using System.Collections.Generic;

namespace CapstoneTest.Models
{
    public partial class Players
    {
        public Players()
        {
            FoulLogs = new HashSet<FoulLogs>();
            ScoringLogs = new HashSet<ScoringLogs>();
        }

        public int PlayerId { get; set; }
        public string PlayerNum { get; set; }
        public string Position { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Picture { get; set; }
        public int TeamTeamId { get; set; }

        public Teams TeamTeam { get; set; }
        public ICollection<FoulLogs> FoulLogs { get; set; }
        public ICollection<ScoringLogs> ScoringLogs { get; set; }
    }
}
