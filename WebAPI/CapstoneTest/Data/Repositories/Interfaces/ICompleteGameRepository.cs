using CapstoneTest.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Repositories.Interfaces
{
    public interface ICompleteGameRepository : IRepository
    {
        Task<ICompleteGame> CompleteGame(ICompleteGame game);

        Task<IEnumerable<ICompleteGame>> GetCompleteGamesByTeamAndSeason(int teamId, int seasonId);

        Task<IEnumerable<ICompleteGame>> GetCompleteGamesBySeason(int seasonId);
    }
}
