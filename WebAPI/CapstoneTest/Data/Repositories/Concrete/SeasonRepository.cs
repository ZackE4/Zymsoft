using CapstoneTest.Data.Entities.Concrete;
using CapstoneTest.Data.Entities.Interfaces;
using CapstoneTest.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Repositories.Concrete
{
    public class SeasonRepository : BaseRepository, ISeasonRepository
    {
        public SeasonRepository(string connectionString) : base(connectionString)
        {
        }

        string baseSelectQuery = @"
                            SELECT s.[SeasonId],
                                   s.[SeasonStart],
                                   s.[SeasonEnd],
                                   s.[League_LeagueId] as LeagueId
                            FROM Seasons s
                            {0}";

        string insertQuery = @"
                            INSERT INTO Seasons
                            (
                                SeasonStart,
                                SeasonEnd,
                                League_LeagueId
                            )
                            VALUES(
                                @SeasonStart,
                                @SeasonEnd,
                                @LeagueId
                            )";

        public async Task<ISeason> CreateAsync(int leagueId, DateTime startDate, DateTime endDate)
        {
            await this.DataContext.ExecuteNonQueryAsync(insertQuery, new { @SeasonStart = startDate, @SeasonEnd = endDate, @LeagueId = leagueId });

            string selectLastQuery = @"
                            SELECT TOP(1) s.[SeasonId],
                                   s.[SeasonStart],
                                   s.[SeasonEnd],
                                   s.[League_LeagueId] as LeagueId
                            FROM Seasons s
                            WHERE League_LeagueId = @LeagueId
                            ORDER BY SeasonId DESC";

            return (await this.DataContext.QueryAsync<Season>(selectLastQuery, new { @LeagueId = leagueId })).FirstOrDefault();
        }

        public async Task<IEnumerable<ISeason>> GetByLeagueAsync(int leagueId)
        {
            var query = string.Format(baseSelectQuery, "WHERE s.[League_LeagueId] = @LeagueId;");

            return (await this.DataContext.QueryAsync<Season>(query, new { @LeagueId = leagueId }));
        }

        public async Task<ISeason> GetCurrentAsync(int leagueId)
        {
            var query = string.Format(baseSelectQuery, "WHERE s.[League_LeagueId] = @LeagueId ORDER BY SeasonStart DESC;");

            return (await this.DataContext.QueryAsync<Season>(query, new { @LeagueId = leagueId })).FirstOrDefault();
        }
    }
}
