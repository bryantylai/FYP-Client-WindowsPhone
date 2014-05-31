using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApolloWP.Data.Item
{
    public class AvatarHistoryItem
    {
        public IEnumerable<RunItem> Year { get; set; }
        public IEnumerable<RunItem> Month { get; set; }
        public IEnumerable<RunItem> Week { get; set; }
        public IEnumerable<RunItem> Day { get; set; }
    }
    public class RunItem
    {
        public long RunDate { get; set; }
        public double Distance { get; set; }
        public long Duration { get; set; }
    }
}
