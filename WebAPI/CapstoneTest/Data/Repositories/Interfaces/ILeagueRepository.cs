using CapstoneTest.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Repositories.Interfaces
{
    public interface ILeagueRepository : IRepository
    {
        Task<ILeague> GetAsync(string leagueKey);
    }
}
