using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Data.Entities.Interfaces
{
    public interface ISeason : IEntity
    {
        int SeasonId { get; set; }
        DateTime SeasonStart { get; set; }
        DateTime SeasonEnd { get; set; }
        int LeagueId { get; set; }
    }
}
