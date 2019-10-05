﻿using CapstoneTest.Data.Entities.Concrete;
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

        string baseSelectQuery = @"
                            SELECT p.[PlayerId],
                                   p.[PlayerNum],
                                   p.[Position],
                                   p.[FirstName],
                                   p.[LastName],
                                   p.[Picture],
                                   p.[Team_TeamId] as TeamId
                            FROM Players p
                            {0}";

        string insertQuery = @"
                            INSERT INTO Players
                            (
                                PlayerNum,
                                Position,
                                FirstName,
                                LastName,
                                Picture,
                                Team_TeamId
                            )
                            VALUES(
                                @PlayerNum,
                                @Position,
                                @FirstName,
                                @LastName,
                                @Picture,
                                @TeamId
                            )";

        public async Task<IPlayer> AddAsync(IPlayer player)
        {
            await this.DataContext.ExecuteNonQueryAsync(insertQuery, new { @PlayerNum = player.PlayerNum, @Position = player.Position, @FirstName = player.FirstName, @LastName = player.LastName, @Picture = player.Picture, @TeamId = player.TeamId });

            string selectLastQuery = @"
                            SELECT TOP(1) p.[PlayerId],
                                   p.[PlayerNum],
                                   p.[Position],
                                   p.[FirstName],
                                   p.[LastName],
                                   p.[Picture],
                                   p.[Team_TeamId] as TeamId
                            FROM Players p
                            WHERE Team_TeamId = @TeamId
                            ORDER BY PlayerId DESC";

            return (await this.DataContext.QueryAsync<Player>(selectLastQuery, new { @TeamId = player.TeamId })).FirstOrDefault();
        }

        public async Task<IPlayer> GetAsync(int id)
        {
            var query = string.Format(baseSelectQuery, "WHERE PlayerId = @PlayerId;");

            return (await this.DataContext.QueryAsync<Player>(query, new { @PlayerId = id })).FirstOrDefault();
        }

        public async Task<IEnumerable<IPlayer>> GetByTeamAsync(int teamId)
        {
            var query = string.Format(baseSelectQuery, "WHERE Team_TeamId = @TeamId;");

            return (await this.DataContext.QueryAsync<Player>(query, new { @TeamId = teamId }));
        }

        public async Task<IPlayer> UpdateAsync(IPlayer player)
        {
            string updateQuery = @"
                            UPDATE Players
                                SET PlayerNum = @PlayerNum
                                Position = @Position,
                                FirstName = @FirstName,
                                LastName = @LastName,
                                Picture = @Picture,
                                Team_TeamId = @TeamId
                            WHERE PlayerId = @PlayerId";

            await this.DataContext.ExecuteNonQueryAsync(updateQuery, new { @PlayerNum = player.PlayerNum, @Position = player.Position, @FirstName = player.FirstName, @LastName = player.LastName, @Picture = player.Picture, @TeamId = player.TeamId, @PlayerId = player.PlayerId });

            string selectLastQuery = @"
                             SELECT p.[PlayerId],
                                   p.[PlayerNum],
                                   p.[Position],
                                   p.[FirstName],
                                   p.[LastName],
                                   p.[Picture],
                                   p.[Team_TeamId] as TeamId
                            FROM Players p
                            WHERE PlayerId = @PlayerId";

            return (await this.DataContext.QueryAsync<Player>(selectLastQuery, new { @PlayerId = player.PlayerId })).FirstOrDefault();
        }
    }
}
