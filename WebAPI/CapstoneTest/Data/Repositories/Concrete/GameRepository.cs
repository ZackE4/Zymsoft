using CapstoneTest.Data.Entities.Concrete;
using CapstoneTest.Data.Entities.Interfaces;
using CapstoneTest.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Repositories.Concrete
{
    public class GameRepository : BaseRepository, IGameRepository
    {
        public GameRepository(string connectionString) : base(connectionString)
        {
        }

        string baseSelectQuery = @"
                            SELECT g.[GameId],
                                   g.[Date],
                                   g.[HomeTeamId],
                                   g.[AwayTeamId],
                                   g.[GameComplete],
                                   g.[Season_SeasonId] as SeasonId
                            FROM Games g
                            {0}";

        string insertQuery = @"
                            INSERT INTO Games
                            (
                                Date,
                                HomeTeamId,
                                AwayTeamId,
                                GameComplete,
                                Season_SeasonId
                            )
                            VALUES(
                                @Date,
                                @HomeTeamId,
                                @AwayTeamId,
                                @GameComplete,
                                @SeasonId
                            )";

        public async Task<IGame> CreateAsync(int team1Id, int team2Id, int seasonId)
        {
            await this.DataContext.ExecuteNonQueryAsync(insertQuery, new { @Date = DateTime.Now, @HomeTeamId = team1Id, @AwayTeamId = team2Id, @GameComplete = false, @SeasonId = seasonId });

            string selectLastQuery = @"
                            SELECT TOP(1) g.[GameId],
                                   g.[Date],
                                   g.[HomeTeamId],
                                   g.[AwayTeamId],
                                   g.[GameComplete],
                                   g.[Season_SeasonId] as SeasonId
                            FROM Games g
                            WHERE Season_SeasonId = @SeasonId
                            ORDER BY GameId DESC";

            return (await this.DataContext.QueryAsync<Game>(selectLastQuery, new { @SeasonId = seasonId })).FirstOrDefault();
        }

        public async Task<IGame> GetAsync(int id)
        {
            var query = string.Format(baseSelectQuery, "WHERE GameId = @GameId;");

            return (await this.DataContext.QueryAsync<Game>(query, new { @GameId = id })).FirstOrDefault();
        }

        public async Task<IGame> UpdateAsync(IGame game)
        {
            string updateQuery = @"
                            UPDATE Games
                                SET Date = @Date,
                                HomeTeamId = @HomeTeamId,
                                AwayTeamId = @AwayTeamId,
                                GameComplete = @GameComplete,
                                Season_SeasonId = @SeasonId
                            WHERE GameId = @GameId";

            await this.DataContext.ExecuteNonQueryAsync(updateQuery, new { @Date = game.Date, @HomeTeamId = game.HomeTeamId, @AwayTeamId = game.AwayTeamId, @GameComplete = game.GameComplete, @SeasonId = game.SeasonId });

            string selectLastQuery = @"
                             SELECT g.[GameId],
                                   g.[Date],
                                   g.[HomeTeamId],
                                   g.[AwayTeamId],
                                   g.[GameComplete],
                                   g.[Season_SeasonId] as SeasonId
                            FROM Games g
                            WHERE GameId = @GameId";

            return (await this.DataContext.QueryAsync<Game>(selectLastQuery, new { @GameId = game.GameId })).FirstOrDefault();
        }
    }
}
