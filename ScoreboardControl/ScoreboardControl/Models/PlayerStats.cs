using System;
using System.Collections.Generic;
using System.Linq;

namespace CapstoneTest.Models
{
    [Serializable]
    public partial class PlayerStats : Players
    {

        public int FoulCount { get; set; }
        public int PointCount { get; set; }

    }
}
