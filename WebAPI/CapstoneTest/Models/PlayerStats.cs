using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CapstoneTest.Models
{
    [Serializable]
    public partial class PlayerStats : Players
    {
        CapstoneContext context;

        public int FoulCount { get; set; }
        public int PointCount { get; set; }

        public PlayerStats(Players player, CapstoneContext context)
        {
            this.context = context;
            this.PlayerId = player.PlayerId;
            this.PlayerNum = player.PlayerNum;
            this.Position = player.Position;
            this.FirstName = player.FirstName;
            this.LastName = player.LastName;
            this.Picture = player.Picture;
            this.TeamTeamId = player.TeamTeamId;

            this.TeamTeam = player.TeamTeam;
            this.FoulLogs = new HashSet<FoulLogs>();
            this.ScoringLogs = new HashSet<ScoringLogs>();

            this.PointCount = CalculateScore();
            this.FoulCount = CalculateFoulCount();
        }

        public int CalculateScore()
        {
            int score = 0;

            IEnumerable<ScoringLogs> PlayerScoringLogs = context.ScoringLogs.Where(sl => sl.PlayerPlayerId == this.PlayerId);
            foreach (ScoringLogs scoreLog in PlayerScoringLogs)
            {
                if (scoreLog.PlayerPlayerId == this.PlayerId)
                {
                    score += scoreLog.Points;
                }
            }
            return score;
        }

        public int CalculateFoulCount()
        {
            int foulCount = 0;

            IEnumerable<FoulLogs> PlayerFoulLogs = context.FoulLogs.Where(sl => sl.PlayerPlayerId == this.PlayerId);
            foreach (FoulLogs foulLogs in PlayerFoulLogs)
            {
                if (foulLogs.PlayerPlayerId == this.PlayerId)
                {
                    foulCount++;
                }
            }
            return foulCount;
        }
    }
}
