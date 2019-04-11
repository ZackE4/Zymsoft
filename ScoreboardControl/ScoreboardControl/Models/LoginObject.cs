﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Models
{
    [Serializable]
    public class LoginObject
    {
        public Logins Login { get; set; }
        public int LeagueId { get; set; }
        public string LeagueName { get; set; }
        public string Logo { get; set; }
        public string LeagueKey { get; set; }

        public LoginObject(Logins login)
        {
            Login = login;
        }
    }
}
