using CapstoneTest.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Repositories.Interfaces
{
    public interface IFoulLogRepository : IRepository
    {
        Task<IEnumerable<IFoulLog>> GetByGameAsync(int gameId);

        Task<IEnumerable<IFoulLog>> GetByPlayerAsync(int playerId);

        Task<IEnumerable<IFoulLog>> GetByPlayerAndGameAsync(int playerId, int gameId);

        Task<IEnumerable<IFoulLog>> GetByTeamAndGameAsync(int teamId, int gameId);

        Task<IFoulLog> RecordFoul(IFoulLog foul);
    }
}
