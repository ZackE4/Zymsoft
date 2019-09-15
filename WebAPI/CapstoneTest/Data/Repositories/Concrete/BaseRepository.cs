using CapstoneTest.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Repositories.Concrete
{
    public class BaseRepository : IRepository
    {
        public string ConnectionString { get; set; }

        protected SqlDataContext _dataContext;

        public SqlDataContext DataContext
        {
            get
            {
                if (_dataContext == null)
                {
                    this._dataContext = new SqlDataContext(ConnectionString);
                }

                return this._dataContext;
            }
        }

        public BaseRepository(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
    }
}
