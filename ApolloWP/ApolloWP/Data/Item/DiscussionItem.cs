using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApolloWP.Data.Item
{
    public class DiscussionItem
    {
        public Guid DiscussionId { get; set; }
        public string Title { get; set; }
        public string CreatorName { get; set; }
    }

    public class DiscussionGeneralItem : DiscussionItem
    {
        public int ReplyCount { get; set; }
        public long LastActive { get; set; }
    }

    public class DiscussionListBoxItem
    {
        public Guid DiscussionId { get; set; }
        public string Title { get; set; }
        public string CreatorName { get; set; }
        public int ReplyCount { get; set; }
        public TimeSpan ActiveTime { get; set; }
        public DateTime ActiveDate { get; set; }
    }

    public class DiscussionDetailedItem : DiscussionItem
    {
        public IEnumerable<ReplyItem> Replies { get; set; }
    }

    public class ReplyItem
    {
        public string ResponderName { get; set; }
        public string Content { get; set; }
        public long RepliedAt { get; set; }
    }
}
