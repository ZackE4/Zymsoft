using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models
{
    public class UploadVideo
    {
        public virtual string FileName { get; set; }
        public virtual IFormFile VideoFile { get; set; }
    }

    public class UploadImage
    {
        public virtual string FileName { get; set; }
        public virtual IFormFile ImageFile { get; set; }
    }
}
