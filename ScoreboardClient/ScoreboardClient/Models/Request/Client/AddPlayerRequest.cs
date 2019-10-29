﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreboardClient.Models.Request.Client
{
    public class AddPlayerRequest
    {
        public virtual string ApiToken { get; set; }
        public virtual string LeagueKey { get; set; }
        public virtual int TeamId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string PlayerNum { get; set; }
        public virtual string Position { get; set; }
        public virtual string Picture { get; set; }
    }
}
