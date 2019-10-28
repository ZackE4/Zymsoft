using CapstoneTest.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Entities.Concrete
{
    public class Season : ISeason
    {
        public virtual int SeasonId { get; set; }
        public virtual DateTime SeasonStart { get; set; }
        public virtual DateTime SeasonEnd { get; set; }
        public virtual int LeagueId { get; set; }
    }
}
