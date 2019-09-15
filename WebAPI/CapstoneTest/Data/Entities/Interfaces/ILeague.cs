using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Entities.Interfaces
{
    public interface ILeague : IEntity
    {
        int LeagueId { get; set; }
        string LeagueName { get; set; }
        string Logo { get; set; }
        string HashPassword { get; set; }
        string LeagueKey { get; set; }
    }
}
