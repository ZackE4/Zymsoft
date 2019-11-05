using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models.ViewModels
{
    public class ManageVideoViewModel:BaseAdminViewModel
    {
        public virtual List<VideoFile> Videos { get; set; }
    }

    public class VideoFile
    {
        public virtual string FileName { get; set; }
        public virtual TimeSpan Duration { get; set; }
        public virtual string FileSize { get; set; }
    }
}
