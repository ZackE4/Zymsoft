using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using CapstoneTest.Data.Entities.Interfaces;
using System.Data.SqlClient;

namespace CapstoneTest.Data
{
    public class SqlDataContext
    {
        protected string _connectionString { get; set; }

        public SqlDataContext(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string query, object parameters) where T : IEntity
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryAsync<T>(query, parameters);
            }
        }

        public async Task ExecuteNonQueryAsync<T>(string query, object parameters) where T : IEntity
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task ExecuteNonQueryAsync(string query, object parameters)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

    }
}
