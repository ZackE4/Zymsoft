using CapstoneTest.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Repositories.Interfaces
{
    public interface IGameRepository : IRepository
    {
        Task<IGame> GetAsync(int id);

        Task<IGame> CreateAsync(int team1Id, int team2Id, int seasonId);

        Task<IGame> UpdateAsync(IGame game);
    }
}
