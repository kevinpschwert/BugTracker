 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class Project
    {
        public Project()
        {
            Tickets = new HashSet<Ticket>();
            User = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }        
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> User { get; set; }
        
    }
}

//The INSERT statement conflicted with the FOREIGN KEY constraint \"FK_dbo.Tickets_dbo.Projects_ProjectId\".
//The conflict occurred in database \"kschwert-bugtracker\", table \"dbo.Projects\", column 'Id'.\r\nThe 
// statement has been terminated."}