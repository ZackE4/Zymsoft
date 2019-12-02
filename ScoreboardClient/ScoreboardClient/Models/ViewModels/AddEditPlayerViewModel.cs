using ScoreboardClient.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models.ViewModels
{
    public enum AddEditPlayer
    {
        Add, Edit
    }

    public class AddEditPlayerViewModel
    {
        public virtual IPlayer Player { get; set; }
        public virtual AddEditPlayer AddEdit { get; set; }
        public virtual List<ImageFile> PlayerImages { get; set; }
    }
}
