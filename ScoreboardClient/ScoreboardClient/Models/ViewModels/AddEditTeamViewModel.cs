using ScoreboardClient.Data.Entities.Concrete;
using ScoreboardClient.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models.ViewModels
{
    public enum AddEditTeam
    {
        Add,Edit
    }

    public class AddEditTeamViewModel : BaseAdminViewModel
    {
        public virtual ITeam Team { get; set; }
        public virtual List<Player> Players { get; set; }
        public virtual AddEditTeam AddEdit { get; set; }
        public virtual ILeague League { get; set; }
        public virtual List<CompleteGame> TeamHistory { get; set; }
    }
}
