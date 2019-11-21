using CapstoneTest.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Entities.Concrete
{
    public class PlayerWithFouls : IPlayerWithFouls
    {
        public virtual int PlayerId { get; set; }
        public virtual string PlayerNum { get; set; }
        public virtual string Position { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Picture { get; set; }
        public virtual int TeamId { get; set; }
        public virtual int Fouls { get; set; }
    }
}
