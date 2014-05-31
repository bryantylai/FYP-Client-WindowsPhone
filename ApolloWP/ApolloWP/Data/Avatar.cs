﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApolloWP.Data
{
    public class Avatar
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public double Experience { get; set; }
        public TimeSpan Duration { get; set; }
        public double Distance { get; set; }
    }
}
