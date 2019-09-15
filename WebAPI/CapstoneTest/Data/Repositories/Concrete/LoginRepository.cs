using CapstoneTest.Data.Entities.Concrete;
using CapstoneTest.Data.Entities.Interfaces;
using CapstoneTest.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Repositories.Concrete
{
    public class LoginRepository : BaseRepository, ILoginRepository
    {
        public LoginRepository(string connectionString) : base(connectionString)
        {
        }

        string baseSelectQuery = @"
                            SELECT l.[LoginId],
                                   l.[LoginTimestamp],
                                   l.[Expiry],
                                   l.[LoginKey],
                                   l.[League_LeagueId] as LeagueId
                            FROM Logins l
                            {0}";

        public async Task AddAsync(ILogin login)
        {
            string insertQuery = @"
                            INSERT INTO Logins
                            (
                                LoginTimestamp,
                                Expiry,
                                LoginKey,
                                League_LeagueId
                            )
                            VALUES(
                                @LoginTimestamp,
                                @Expiry,
                                @LoginKey,
                                @LeagueId
                            )";

            await this.DataContext.ExecuteNonQueryAsync(insertQuery, new { @LoginTimestamp = login.LoginTimestamp, @Expiry = login.Expiry, @LoginKey = login.LoginKey, @LeagueId = login.LeagueId});
        }

        public async Task<ILogin> GetAsync(int leagueId)
        {
            var query = string.Format(baseSelectQuery, "WHERE League_LeagueId = @LeagueId;");

            return (await this.DataContext.QueryAsync<Login>(query, new { @LeagueId = leagueId })).FirstOrDefault();
        }

        public async Task UpdateAsync(ILogin login)
        {
            string updateQuery = @"
                    UPDATE Logins
                    SET LoginTimestamp = @LoginTimestamp,
                        Expiry = @Expiry,
                        LoginKey = @LoginKey,
                        League_LeagueId = @LeagueId
                    WHERE LoginId = @LoginId";

            await this.DataContext.ExecuteNonQueryAsync(updateQuery, new { @LoginTimestamp = login.LoginTimestamp, @Expiry = login.Expiry, @LoginKey = login.LoginKey, @LeagueId = login.LeagueId, @LoginId = login.LoginId });
        }

        public async Task<ILogin> GetByKey(string apiToken)
        {
            var query = string.Format(baseSelectQuery, "WHERE LoginKey = @apiToken;");

            return (await this.DataContext.QueryAsync<Login>(query, new { @apiToken = apiToken })).FirstOrDefault();
        }
    }
}
