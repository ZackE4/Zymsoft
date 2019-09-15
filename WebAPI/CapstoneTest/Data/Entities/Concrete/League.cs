using CapstoneTest.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Entities.Concrete
{
    public class League : ILeague
    {
        public virtual int LeagueId { get; set; }
        public virtual string LeagueName { get; set; }
        public virtual string Logo { get; set; }
        public virtual string HashPassword { get; set; }
        public virtual string LeagueKey { get; set; }
    }
}
