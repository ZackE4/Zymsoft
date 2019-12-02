using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models.Response.Host
{
    public class TimerResponse
    {
        public virtual int Period { get; set; }
        public virtual bool TimerRunning { get; set; }
    }
}
