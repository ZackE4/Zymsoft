using ScoreboardClient.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Data.Entities.Concrete
{
    public class Login : ILogin
    {
        public virtual int LoginId { get; set; }
        public virtual DateTime LoginTimestamp { get; set; }
        public virtual DateTime Expiry { get; set; }
        public virtual string LoginKey { get; set; }
        public virtual int LeagueId { get; set; }
    }
}
