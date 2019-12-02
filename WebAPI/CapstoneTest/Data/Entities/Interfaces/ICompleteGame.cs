using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Entities.Interfaces
{
    public interface ICompleteGame : IEntity
    {
        int CompleteGameId { get; set; }
        int WinningTeamId { get; set; }
        int LosingTeamId { get; set; }
        int GameId { get; set; }
        bool TieFlag { get; set; }
        DateTime Date { get; set; }
    }
}
