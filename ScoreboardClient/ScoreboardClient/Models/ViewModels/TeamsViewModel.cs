﻿using ScoreboardClient.Data.Entities.Concrete;
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
        public virtual List<Team> Teams { get; set; }
    }
}
