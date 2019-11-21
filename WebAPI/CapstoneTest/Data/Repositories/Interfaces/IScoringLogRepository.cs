using CapstoneTest.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Repositories.Interfaces
{
    public interface IScoringLogRepository : IRepository
    {
        Task<IEnumerable<IScoringLog>> GetByGameAsync(int gameId);

        Task<IEnumerable<IScoringLog>> GetByPlayerAndSeasonAsync(int playerId, int seasonId);

        Task<IEnumerable<IScoringLog>> GetByPlayerAndGameAsync(int playerId, int gameId);

        Task<IEnumerable<IScoringLog>> GetByTeamAndGameAsync(int teamId, int gameId);

        Task<IScoringLog> RecordScore(IScoringLog score);

        Task UndoScore(int scoringLogId);
    }
}
