using CapstoneTest.Data.Entities.Concrete;
using CapstoneTest.Data.Entities.Interfaces;
using CapstoneTest.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Repositories.Concrete
{
    public class ScoringLogRepository : BaseRepository, IScoringLogRepository
    {
        public ScoringLogRepository(string connectionString) : base(connectionString)
        {
        }

        string baseSelectQuery = @"
                            SELECT s.[ScoringLogId],
                                   s.[GameTime],
                                   s.[Points],
                                   s.[Player_PlayerId] as PlayerId,
                                   s.[Game_GameId] as GameId
                            FROM ScoringLogs s
                            {0}";

        string insertQuery = @"
                            INSERT INTO ScoringLogs
                            (
                                GameTime,
                                Points,
                                Player_PlayerId,
                                Game_GameId
                            )
                            VALUES(
                                @GameTime,
                                @Points,
                                @PlayerId,
                                @GameId
                            )";

        public async Task<IEnumerable<IScoringLog>> GetByGameAsync(int gameId)
        {
            var query = string.Format(baseSelectQuery, "WHERE Game_GameId = @GameId;");

            return (await this.DataContext.QueryAsync<ScoringLog>(query, new { @GameId = gameId }));
        }

        public async Task<IEnumerable<IScoringLog>> GetByPlayerAndGameAsync(int playerId, int gameId)
        {
            var query = string.Format(baseSelectQuery, "WHERE Game_GameId = @GameId AND Player_PlayerId = @PlayerId;");

            return (await this.DataContext.QueryAsync<ScoringLog>(query, new { @GameId = gameId, @PlayerId = playerId }));
        }

        public async Task<IEnumerable<IScoringLog>> GetByPlayerAsync(int playerId)
        {
            var query = string.Format(baseSelectQuery, "WHERE Player_PlayerId = @PlayerId;");

            return (await this.DataContext.QueryAsync<ScoringLog>(query, new { @PlayerId = playerId }));
        }

        public async Task<IScoringLog> RecordScore(IScoringLog score)
        {
            await this.DataContext.ExecuteNonQueryAsync(insertQuery, new { @GameTime = score.GameTime, @Points = score.Points, @PlayerId = score.PlayerId, @GameId = score.GameId });

            string selectLastQuery = @"
                            SELECT TOP(1) s.[ScoringLogId],
                                   s.[GameTime],
                                   s.[Points],
                                   s.[Player_PlayerId] as PlayerId,
                                   s.[Game_GameId] as GameId
                            FROM ScoringLogs s
                            WHERE Game_GameId = @GameId
                            ORDER BY ScoringLogId DESC";

            return (await this.DataContext.QueryAsync<ScoringLog>(selectLastQuery, new { @GameId = score.GameId })).FirstOrDefault();
        }

        public async Task<IEnumerable<IScoringLog>> GetByTeamAndGameAsync(int teamId, int gameId)
        {
            string SelectQuery = @"
                            SELECT s.[ScoringLogId],
                                   s.[GameTime],
                                   s.[Points],
                                   s.[Player_PlayerId] as PlayerId,
                                   s.[Game_GameId] as GameId
                            FROM ScoringLogs s
                            INNER JOIN Players p on s.Player_PlayerId = p.PlayerId
                            {0}";
            var query = string.Format(SelectQuery, "WHERE s.[Game_GameId] = @GameId AND p.Team_TeamId = @TeamId;");

            return (await this.DataContext.QueryAsync<ScoringLog>(query, new { @GameId = gameId, @TeamId = teamId }));
        }
    }
}
