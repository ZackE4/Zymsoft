using ScoreboardClient.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models.ViewModels
{
    public class TeamsViewModel : BaseAdminViewModel
    {
        public virtual ILeague League { get; set; }
        public virtual List<ITeam> Teams { get; set; }
    }
}
