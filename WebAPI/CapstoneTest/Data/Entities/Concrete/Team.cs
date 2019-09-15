using CapstoneTest.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Entities.Concrete
{
    public class Team : ITeam
    {
        public virtual int TeamId { get; set; }
        public virtual string TeamName { get; set; }
        public virtual string CoachName { get; set; }
        public virtual string Logo { get; set; }
        public virtual int LeagueId { get; set; }
    }
}
