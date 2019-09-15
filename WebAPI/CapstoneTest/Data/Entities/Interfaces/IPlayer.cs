using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Entities.Interfaces
{
    public interface IPlayer : IEntity
    {
        int PlayerId { get; set; }
        string PlayerNum { get; set; }
        string Position { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Picture { get; set; }
        int TeamId { get; set; }
    }
}
