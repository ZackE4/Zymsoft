using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models.Request.Local
{
    public class SetTimeoutsRequest : CallTimeoutRequest
    {
        public virtual int Timeouts { get; set; }
    }
}
