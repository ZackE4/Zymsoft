using CapstoneTest.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Entities.Concrete
{
    public class ScoringLog : IScoringLog
    {
        public virtual int ScoringLogId { get; set; }
        public virtual TimeSpan GameTime { get; set; }
        public virtual int Points { get; set; }
        public virtual int PlayerId { get; set; }
        public virtual int GameId { get; set; }
    }
}
