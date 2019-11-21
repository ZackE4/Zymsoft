using CapstoneTest.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Repositories.Interfaces
{
    public interface IPlayerRepository : IRepository
    {
        Task<IPlayer> GetAsync(int id);

        Task<IEnumerable<IPlayer>> GetByTeamAsync(int teamId);

        Task<IPlayer> AddAsync(IPlayer player);

        Task<IPlayer> UpdateAsync(IPlayer player);

        Task<IPlayerWithPoints> GetTopScorerAsync(int seasonId);

        Task<IPlayerWithFouls> GetTopFoulerAsync(int seasonId);
    }
}
