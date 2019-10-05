using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Models.Response
{
    public class GameFoulsResponse
    {
        public virtual int HomeTeamFouls { get; set; }
        public virtual int AwayTeamFouls { get; set; }
    }
}
