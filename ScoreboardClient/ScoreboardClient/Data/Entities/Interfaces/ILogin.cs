using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Data.Entities.Interfaces
{
    public interface ILogin : IEntity
    {
        int LoginId { get; set; }
        DateTime LoginTimestamp { get; set; }
        DateTime Expiry { get; set; }
        string LoginKey { get; set; }
        int LeagueId { get; set; }
    }
}
