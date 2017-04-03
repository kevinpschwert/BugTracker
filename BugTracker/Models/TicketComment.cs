using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketComment
    {
        public int Id { get; set; }
        public string Comment { get; set; }        
        public DateTimeOffset Created { get; set; } //Do We need this?
        public int TicketId { get; set; }
        public string UserId { get; set; } // Do we need this?

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}