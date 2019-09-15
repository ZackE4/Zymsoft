using CapstoneTest.Data.Entities.Interfaces;
using CapstoneTest.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Repositories.Concrete
{
    public class PlayerRepository : BaseRepository, IPlayerRepository
    {
        public PlayerRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<IPlayer> AddAsync(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public async Task<IPlayer> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IPlayer>> GetByTeamAsync(int teamId)
        {
            throw new NotImplementedException();
        }

        public async Task<IPlayer> UpdateAsync(IPlayer player)
        {
            throw new NotImplementedException();
        }
    }
}
