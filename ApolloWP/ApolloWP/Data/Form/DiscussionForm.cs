using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApolloWP.Data.Form
{
    public class DiscussionForm
    {
        public string Title { get; set; }
        public string Content {get;set;}
    }

    public class ReplyForm
    {
        public Guid DiscussionId { get; set; }
        public string Content { get; set; }
    }
}
