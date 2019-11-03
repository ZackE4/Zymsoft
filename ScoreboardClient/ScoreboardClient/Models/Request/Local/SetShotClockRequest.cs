using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models.Request.Local
{
    public class SetShotClockRequest : BasicLocalRequest
    {
        public virtual int Value { get; set; }
    }
}
