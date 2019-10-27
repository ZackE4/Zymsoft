using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Data.Entities.Interfaces
{
    public interface IGame : IEntity
    {
        int GameId { get; set; }
        DateTime Date { get; set; }
        int HomeTeamId { get; set; }
        int AwayTeamId { get; set; }
        bool GameComplete { get; set; }
        int SeasonId { get; set; }
    }
}
