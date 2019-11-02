using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models.Request.Local
{
    public class LocalRecordFoulRequest : BasicLocalRequest
    {
        public virtual int Period { get; set; }
        public virtual int PlayerId { get; set; }
        public virtual string Side { get; set; }
    }
}
