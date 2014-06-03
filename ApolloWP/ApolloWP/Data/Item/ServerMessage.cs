using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApolloWP.Data.Item
{
    public class ServerMessage
    {
        public bool IsError { get; set; }
        public string Message { get; set; }
    }

    public class LoginMessage : ServerMessage
    {
        public bool NewAccount { get; set; }
    }

    public class RunMessage : ServerMessage
    {
        public long Duration { get; set; }
        public AvatarProfileItem Avatar { get; set; }
    }
}
