using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models.Request.Local
{
    public class CallTimeoutRequest : BasicLocalRequest
    {
        public virtual string Side { get; set; }
    }
}
