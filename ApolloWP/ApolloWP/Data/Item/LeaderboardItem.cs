using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApolloWP.Data.Item
{
    public class LeaderboardItem
    {
        public Guid LeaderboardId { get; set; }
        public Guid PlayerId { get; set; }
        public string PlayerProfileImage { get; set; }
        public string PlayerName { get; set; }
        public string Point { get; set; }
        public bool IsSelf { get; set; }
    }
}
