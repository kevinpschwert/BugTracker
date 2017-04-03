using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTimeOffset Changed { get; set; }
        public string UserId { get; set; }

        //public int SubmitterId { get; set; }
        //public int DeveloperId { get; set; }
        //public string Status { get; set; }
        //public string TicketType { get; set; } 
        //public int Priority { get; set; }
        //public string Title { get; set; }
        //public string Owner { get; set; }
        //public DateTimeOffset ChangeDt { get; set; }        
        //public int ProjectId { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}