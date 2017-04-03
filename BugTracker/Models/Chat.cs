using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTimeOffset Created { get; set; }
        [Display(Name = "Owner")]
        public string OwnerUserId { get; set; }
        [Display(Name = "Assign")]
        public string AssignToUserId { get; set; }

        public virtual ApplicationUser OwnerUser { get; set; }
        public virtual ApplicationUser AssignToUser { get; set; }
    }
}