using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketViewModel
    {
        public List<Ticket> Ticket { get; set; }
        public List<TicketNotification> TicketNotification { get; set; }
        public List<Project> Project { get; set; }
        public List<ApplicationUser> User { get; set; }
        public List<Chat> Chat { get; set; }
        public List<TicketHistory> TicketHistory { get; set; }
    }
}