using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models.Request.Local
{
    public class SetGameClockRequest : BasicLocalRequest
    {
        public virtual int Minutes { get; set; }
        public virtual int Seconds { get; set; }
    }
}
