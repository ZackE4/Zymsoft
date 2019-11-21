using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models
{
    public class UndoLogEntry
    {
        public virtual int Id { get; set; }
        public virtual UndoLogType Type { get; set; }
        public virtual int Points { get; set; }
    }

    public enum UndoLogType
    {
        Score, Foul
    }
}
