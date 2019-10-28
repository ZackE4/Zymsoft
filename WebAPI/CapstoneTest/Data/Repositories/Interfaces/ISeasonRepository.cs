using CapstoneTest.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Repositories.Interfaces
{
    public interface ISeasonRepository : IRepository
    {
        Task<IEnumerable<ISeason>> GetByLeagueAsync(int leagueId);
        Task<ISeason> GetCurrentAsync(int leagueId);
        Task<ISeason> CreateAsync(int leagueId, DateTime startDate, DateTime endDate);
    }
}
