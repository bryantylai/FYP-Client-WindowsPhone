using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApolloWP.Data.Item
{
    public class DoctorItem
    {
        public Guid DoctorId { get; set; }
        public string Name { get; set; }
        public string Expertise { get; set; }
        public string CenterName { get; set; }
        public string Phone { get; set; }
    }
}
