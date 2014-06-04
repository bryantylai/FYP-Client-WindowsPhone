using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApolloWP.Data.Form
{
    public class DoctorFilterForm
    {
        public string Expertise { get; set; }
        public Coordinate Location { get; set; }
    }

    public class Coordinate
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
