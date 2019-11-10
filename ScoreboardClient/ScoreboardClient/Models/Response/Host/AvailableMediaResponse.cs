using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models.Response.Host
{
    public class AvailableMediaResponse
    {
        public virtual List<AvailableVideo> AvailableVideos { get; set; }
        public virtual List<string> AvailableImages { get; set; }
    }

    public class AvailableVideo
    {
        public virtual string Filename { get; set; }
        public virtual TimeSpan Duration { get; set; }
    }
}
