﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneTest.Data.Entities.Interfaces
{
    public interface IPlayerWithPoints : IPlayer
    {
        int Points { get; set; }
    }
}
