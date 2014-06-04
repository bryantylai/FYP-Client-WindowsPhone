using System;
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
        public string Experience { get; set; }
        public string Duration { get; set; }
        public double Distance { get; set; }
    }
}
