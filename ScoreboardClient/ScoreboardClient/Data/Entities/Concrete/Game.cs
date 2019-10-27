using ScoreboardClient.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Data.Entities.Concrete
{
    public class Game : IGame
    {
        public virtual int GameId { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual int HomeTeamId { get; set; }
        public virtual int AwayTeamId { get; set; }
        public virtual bool GameComplete { get; set; }
        public virtual int SeasonId { get; set; }
    }
}
