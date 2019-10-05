using CapstoneTest.Data.Entities.Concrete;
using CapstoneTest.Data.Entities.Interfaces;
using CapstoneTest.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Repositories.Concrete
{
    public class FoulLogRepository : BaseRepository, IFoulLogRepository
    {
        public FoulLogRepository(string connectionString) : base(connectionString)
        {
        }

        string baseSelectQuery = @"
                            SELECT f.[FouldLogId],
                                   f.[GameTime],
                                   f.[Player_PlayerId] as PlayerId,
                                   f.[Game_GameId] as GameId
                            FROM FoulLogs f
                            {0}";

        string insertQuery = @"
                            INSERT INTO FoulLogs
                            (
                                GameTime,
                                Player_PlayerId,
                                Game_GameId
                            )
                            VALUES(
                                @GameTime,
                                @PlayerId,
                                @GameId
                            )";

        public async Task<IEnumerable<IFoulLog>> GetByGameAsync(int gameId)
        {
            var query = string.Format(baseSelectQuery, "WHERE Game_GameId = @GameId;");

            return (await this.DataContext.QueryAsync<FoulLog>(query, new { @GameId = gameId }));
        }

        public async Task<IEnumerable<IFoulLog>> GetByPlayerAndGameAsync(int playerId, int gameId)
        {
            var query = string.Format(baseSelectQuery, "WHERE Game_GameId = @GameId AND Player_PlayerId = @PlayerId;");

            return (await this.DataContext.QueryAsync<FoulLog>(query, new { @GameId = gameId, @PlayerId = playerId }));
        }

        public async Task<IEnumerable<IFoulLog>> GetByPlayerAsync(int playerId)
        {
            var query = string.Format(baseSelectQuery, "WHERE Player_PlayerId = @PlayerId;");

            return (await this.DataContext.QueryAsync<FoulLog>(query, new { @PlayerId = playerId }));
        }

        public async Task<IFoulLog> RecordFoul(IFoulLog foul)
        {
            await this.DataContext.ExecuteNonQueryAsync(insertQuery, new { @GameTime = foul.GameTime, @PlayerId = foul.PlayerId, @GameId = foul.GameId });

            string selectLastQuery = @"
                            SELECT TOP(1) f.[FouldLogId],
                                   f.[GameTime],
                                   f.[Player_PlayerId] as PlayerId,
                                   f.[Game_GameId] as GameId
                            FROM FoulLogs f
                            WHERE Game_GameId = @GameId
                            ORDER BY FouldLogId DESC";

            return (await this.DataContext.QueryAsync<FoulLog>(selectLastQuery, new { @GameId = foul.GameId })).FirstOrDefault();
        }

        public async Task<IEnumerable<IFoulLog>> GetByTeamAndGameAsync(int teamId, int gameId)
        {
            string SelectQuery = @"
                            SELECT f.[FouldLogId],
                                   f.[GameTime],
                                   f.[Player_PlayerId] as PlayerId,
                                   f.[Game_GameId] as GameId
                            FROM FoulLogs f
                            INNER JOIN Players p on f.Player_PlayerId = p.PlayerId
                            {0}";
            var query = string.Format(SelectQuery, "WHERE f.[Game_GameId] = @GameId AND p.Team_TeamId = @TeamId;");

            return (await this.DataContext.QueryAsync<FoulLog>(query, new { @GameId = gameId, @TeamId = teamId }));
        }
    }
}
