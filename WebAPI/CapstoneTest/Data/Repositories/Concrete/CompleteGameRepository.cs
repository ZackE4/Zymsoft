using CapstoneTest.Data.Entities.Concrete;
using CapstoneTest.Data.Entities.Interfaces;
using CapstoneTest.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Repositories.Concrete
{
    public class CompleteGameRepository : BaseRepository, ICompleteGameRepository
    {
        public CompleteGameRepository(string connectionString) : base(connectionString)
        {
        }

        string baseSelectQuery = @"
                            SELECT c.[CompleteGameId],
                                   c.[WinningTeamId],
                                   c.[LosingTeamId],
                                   c.[GameId],
                                   c.[TieFlag],
                                   c.[Date]
                            FROM CompleteGame c
                            {0}";

        string insertQuery = @"
                            INSERT INTO CompleteGame
                            (
                                WinningTeamId,
                                LosingTeamId,
                                GameId,
                                TieFlag,
                                Date
                            )
                            VALUES(
                                @WinningTeamId,
                                @LosingTeamId,
                                @GameId,
                                @TieFlag,
                                @Date
                            )";

        public async Task<ICompleteGame> CompleteGame(ICompleteGame game)
        {
            await this.DataContext.ExecuteNonQueryAsync(insertQuery, new { @WinningTeamId = game.WinningTeamId, @LosingTeamId = game.LosingTeamId, @GameId = game.GameId, @TieFlag = game.TieFlag });

            string selectLastQuery = @"
                            SELECT TOP(1) c.[CompleteGameId],
                                   c.[WinningTeamId],
                                   c.[LosingTeamId],
                                   c.[GameId],
                                   c.[TieFlag],
                                   c.[Date]
                            FROM CompleteGame c
                            WHERE GameId = @GameId
                            ORDER BY CompleteGameId DESC";

            return (await this.DataContext.QueryAsync<CompleteGame>(selectLastQuery, new { @GameId = game.GameId })).FirstOrDefault();
        }

        public async Task<IEnumerable<ICompleteGame>> GetCompleteGamesBySeason(int seasonId)
        {
            var query = string.Format(baseSelectQuery, "INNER JOIN Games g on c.GameId = g.GameId WHERE g.[Season_SeasonId] = @SeasonId");

            return (await this.DataContext.QueryAsync<CompleteGame>(query, new { @SeasonId = seasonId }));
        }

        public async Task<IEnumerable<ICompleteGame>> GetCompleteGamesByTeamAndSeason(int teamId, int seasonId)
        {
            var query = string.Format(baseSelectQuery, @"INNER JOIN Games g on c.GameId = g.GameId 
                                                            WHERE g.[Season_SeasonId] = @SeasonId 
                                                            AND (c.[WinningTeamId] = @TeamId 
                                                            OR c.[LosingTeamId] = @TeamId)");

            return (await this.DataContext.QueryAsync<CompleteGame>(query, new { @TeamId = teamId }));
        }
    }
}
