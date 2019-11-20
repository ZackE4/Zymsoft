using ScoreboardClient.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Data.Entities.Concrete
{
    public class CompleteGame : ICompleteGame
    {
        public virtual int CompleteGameId { get; set; }
        public virtual int WinningTeamId { get; set; }
        public virtual int LosingTeamId { get; set; }
        public virtual int GameId { get; set; }
        public virtual bool TieFlag { get; set; }
        public virtual DateTime Date { get; set; }
    }
}
