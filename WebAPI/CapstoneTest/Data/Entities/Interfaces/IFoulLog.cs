using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Entities.Interfaces
{
    public interface IFoulLog : IEntity
    {
        int FouldLogId { get; set; }
        TimeSpan GameTime { get; set; }
        int PlayerId { get; set; }
        int GameId { get; set; }
    }
}
