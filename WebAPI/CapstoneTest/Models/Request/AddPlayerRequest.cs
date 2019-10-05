using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Models.Request
{
    public class AddPlayerRequest
    {
        public virtual string ApiToken { get; set; }
        public virtual string PlayerNum { get; set; }
        public virtual string Position { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Picture { get; set; }
        public virtual int TeamId { get; set; }
        public virtual string LeagueKey { get; set; }
    }
}
