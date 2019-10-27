using ScoreboardClient.Data.Entities.Concrete;
using ScoreboardClient.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models.ViewModels
{
    public class ScreenViewModel
    {
        public virtual ILeague League { get; set; }
        public virtual IGame Game { get; set; }
        public virtual ITeam HomeTeam { get; set; }
        public virtual ITeam AwayTeam { get; set; }
        public virtual GameScore GameScore {get;set;}
        public virtual TimeSpan GameTime { get; set; }
    }
}
