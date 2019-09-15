using CapstoneTest.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Repositories.Interfaces
{
    public interface ILoginRepository : IRepository
    {
        Task<ILogin> GetAsync(int leagueId);

        Task AddAsync(ILogin login);

        Task UpdateAsync(ILogin login);

        Task<ILogin> GetByKey(string apiToken);
    }
}
