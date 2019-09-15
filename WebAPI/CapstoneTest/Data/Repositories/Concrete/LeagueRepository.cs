using CapstoneTest.Data.Entities.Concrete;
using CapstoneTest.Data.Entities.Interfaces;
using CapstoneTest.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Repositories.Concrete
{
    public class LeagueRepository : BaseRepository, ILeagueRepository
    {
        public LeagueRepository(string connectionString) : base(connectionString)
        {
        }

        string baseSelectQuery = @"
                            SELECT l.[LeagueId],
                                   l.[LeagueName],
                                   l.[Logo],
                                   l.[HashPassword],
                                   l.[LeagueKey]
                            FROM Leagues l
                            {0}";

        public async Task<ILeague> GetAsync(string leagueKey)
        {
            var query = string.Format(baseSelectQuery, "WHERE LeagueKey = @LeagueKey;");

            return (await this.DataContext.QueryAsync<League>(query, new { @LeagueKey = leagueKey })).FirstOrDefault();
        }
    }
}
