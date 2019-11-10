using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models.ViewModels
{
    public class ManageImageViewModel : BaseAdminViewModel
    {
        public virtual List<ImageFile> Images { get; set; }
    }

    public class ImageFile
    {
        public virtual string FileName { get; set; }
        public virtual string FileSize { get; set; }
    }
}
