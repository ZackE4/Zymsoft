using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Data.Entities.Interfaces
{
    public interface IScoringLog : IEntity
    {
        int ScoringLogId { get; set; }
        TimeSpan GameTime { get; set; }
        int Points { get; set; }
        int PlayerId { get; set; }
        int GameId { get; set; }
    }
}
