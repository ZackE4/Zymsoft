using CapstoneTest.Data.Entities.Concrete;
using CapstoneTest.Data.Entities.Interfaces;
using CapstoneTest.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Repositories.Concrete
{
    public class TeamRepository : BaseRepository, ITeamRepository
    {
        public TeamRepository(string connectionString) : base(connectionString)
        {
        }

        string baseSelectQuery = @"
                            SELECT t.[TeamId],
                                   t.[TeamName],
                                   t.[CoachName],
                                   t.[Logo],
                                   t.[League_LeagueId] as LeagueId
                            FROM Teams t
                            {0}";

        string insertQuery = @"
                            INSERT INTO Teams
                            (
                                TeamName,
                                CoachName,
                                Logo,
                                League_LeagueId
                            )
                            VALUES(
                                @TeamName,
                                @CoachName,
                                @Logo,
                                @LeagueId
                            )";

        public async Task<ITeam> AddAsync(ITeam team)
        {
            await this.DataContext.ExecuteNonQueryAsync(insertQuery, new { @TeamName = team.TeamName, @CoachName = team.CoachName, @Logo = team.Logo, @LeagueId = team.LeagueId });

            string selectLastQuery = @"
                            SELECT TOP(1) t.[TeamId],
                                   t.[TeamName],
                                   t.[CoachName],
                                   t.[Logo],
                                   t.[League_LeagueId] as LeagueId
                            FROM Teams t
                            WHERE League_LeagueId = @LeagueId
                            ORDER BY TeamId DESC";

            return (await this.DataContext.QueryAsync<Team>(selectLastQuery, new { @LeagueId = team.LeagueId })).FirstOrDefault();
        }

        public async Task<ITeam> GetAsync(int id)
        {
            var query = string.Format(baseSelectQuery, "WHERE TeamId = @TeamId;");

            return (await this.DataContext.QueryAsync<Team>(query, new { @TeamId = id })).FirstOrDefault();
        }

        public async Task<IEnumerable<ITeam>> GetByLeagueAsync(int leagueId)
        {
            var query = string.Format(baseSelectQuery, "WHERE League_LeagueId = @LeagueId;");

            return (await this.DataContext.QueryAsync<Team>(query, new { @LeagueId = leagueId }));
        }

        public async Task<ITeam> UpdateAsync(ITeam team)
        {
            string updateQuery = @"
                            UPDATE Teams
                                SET TeamName = @TeamName,
                                CoachName = @CoachName,
                                Logo = @Logo,
                                League_LeagueId = @LeagueId
                            WHERE TeamId = @TeamId";

            await this.DataContext.ExecuteNonQueryAsync(updateQuery, new { @TeamName = team.TeamName, @CoachName = team.CoachName, @Logo = team.Logo, @LeagueId = team.LeagueId, @TeamId = team.TeamId });

            string selectLastQuery = @"
                            SELECT t.[TeamId],
                                   t.[TeamName],
                                   t.[CoachName],
                                   t.[Logo],
                                   t.[League_LeagueId] as LeagueId
                            FROM Teams t
                            WHERE TeamId = @TeamId";

            return (await this.DataContext.QueryAsync<Team>(selectLastQuery, new { @TeamId = team.TeamId })).FirstOrDefault();

        }
    }
}
