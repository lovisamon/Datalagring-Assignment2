using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class Comment
    {
        public long Id { get; set; }
        public long IssueId { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }

        public Comment()
        {
        }

        public Comment(long issueId, string description)
        {
            IssueId = issueId;
            Description = description;
        }

        public Comment(long id, long issueId, string description, DateTime created)
        {
            Id = id;
            IssueId = issueId;
            Description = description;
            Created = created;
        }
    }
}
