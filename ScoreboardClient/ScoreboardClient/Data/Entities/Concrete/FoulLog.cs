using ScoreboardClient.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Data.Entities.Concrete
{
    public class FoulLog : IFoulLog
    {
        public virtual int FouldLogId { get; set; }
        public virtual TimeSpan GameTime { get; set; }
        public virtual int PlayerId { get; set; }
        public virtual int GameId { get; set; }
    }
}
