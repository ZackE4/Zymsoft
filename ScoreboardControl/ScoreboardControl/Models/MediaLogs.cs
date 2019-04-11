using System;
using System.Collections.Generic;

namespace CapstoneTest.Models
{
    public partial class MediaLogs
    {
        public int MediaLogId { get; set; }
        public string MediaName { get; set; }
        public TimeSpan DurationPlayed { get; set; }
        public TimeSpan GameTime { get; set; }
        public int GameGameId { get; set; }

        public Games GameGame { get; set; }
    }
}
