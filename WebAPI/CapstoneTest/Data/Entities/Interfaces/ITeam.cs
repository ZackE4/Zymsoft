using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Entities.Interfaces
{
    public interface ITeam : IEntity
    {
        int TeamId { get; set; }
        string TeamName { get; set; }
        string CoachName { get; set; }
        string Logo { get; set; }
        int LeagueId { get; set; }
    }
}
