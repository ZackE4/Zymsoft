using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models
{
    public enum MessageType
    {
        Alert, Success, Error
    }

    public class PageMessage
    {
        public virtual string Message { get; set; }
        public virtual MessageType Type { get; set; }
    }
}
