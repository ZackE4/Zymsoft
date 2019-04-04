using System;
using System.Collections.Generic;

namespace CapstoneTest.Models
{
    public partial class FoulLogs
    {
        public int FouldLogId { get; set; }
        public TimeSpan GameTime { get; set; }
        public int PlayerPlayerId { get; set; }
        public int GameGameId { get; set; }

        public Games GameGame { get; set; }
        public Players PlayerPlayer { get; set; }
    }
}
