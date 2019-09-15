using CapstoneTest.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Repositories.Interfaces
{
    public interface ITeamRepository : IRepository
    {
        Task<ITeam> GetAsync(int id);

        Task<IEnumerable<ITeam>> GetByLeagueAsync(int leagueId);

        Task<ITeam> AddAsync(ITeam team);

        Task<ITeam> UpdateAsync(ITeam team);
    }
}
