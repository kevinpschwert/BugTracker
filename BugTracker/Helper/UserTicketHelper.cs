using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity;


namespace BugTracker.Helper
{
    public class UserTicketHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();
        UserProjectHelper ph = new UserProjectHelper();

        public ICollection<Ticket> ListUserTickets(string userId)
        {
            //This gets the current User's Id
            //HttpContext.Current.User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(userId);
            var tickets = db.Tickets.Where(u => u.AssignToUserId == user.Id);

            var mytickets = tickets.ToList();
            return (mytickets);
        }

        public ICollection<Ticket> ListTicketstoProjectsOld(string userId, int id)
        {
            ApplicationUser user = db.Users.Find(userId);
            Project projects = db.Projects.Find(id);
            //var ticket = db.Tickets.Include(u => u.AssignToUserId == user.Id).Include(u => u.ProjectId == projects.Id);
            var tickets = db.Tickets.Where(u => u.AssignToUserId == user.Id && u.ProjectId == projects.Id);
            var mytickets = tickets.ToList();
            return (mytickets);
        }

        public ICollection<Ticket> ListTicketstoProjects(string userId, int id)
        {
            ApplicationUser user = db.Users.Find(userId);
            Project projects = db.Projects.Find(id);
            //var ticket = db.Tickets.Include(u => u.AssignToUserId == user.Id).Include(u => u.ProjectId == projects.Id);
            var tickets = db.Tickets.Where(u => u.ProjectId == projects.Id);
            var mytickets = tickets.ToList();
            return (mytickets);
        }

        public ICollection<Ticket> ListTicketstoOwner(string userId, int? id)
        {
            ApplicationUser user = db.Users.Find(userId);
            Project projects = db.Projects.Find(id);
            //var ticket = db.Tickets.Include(u => u.AssignToUserId == user.Id).Include(u => u.ProjectId == projects.Id);
            var tickets = db.Tickets.Where(u => u.AssignToUserId == user.Id && u.Id == id);
            var mytickets = tickets.ToList();
            return (mytickets);
        }

        public ICollection<Ticket> ListOwnerTickets(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            var tickets = db.Tickets.Where(u => u.OwnerUserId == user.Id);

            var mytickets = tickets.ToList();
            return (mytickets);
        }

        public ICollection<TicketNotification> ListTicketNotifications(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            var tickets = db.TicketNotifications.Where(u => u.UserId == user.Id);

            var mytickets = tickets.ToList();
            return (mytickets);
        }

        public ICollection<Ticket> ListProjectTickets(string userId)
        {
            var allTickets = new List<Ticket>();
            foreach (var project in ph.ListUserProjects(userId))
            {
                foreach (var ticket in project.Tickets)
                {
                    allTickets.Add(ticket);
                }
            }
            return allTickets;
        }

        public ICollection<Ticket> ListAssignedTickets(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            var tickets = db.Tickets.Where(u => u.AssignToUserId == user.Id);

            var mytickets = tickets.ToList();
            return (mytickets);
        }

        public ICollection<ApplicationUser> ListCreatedTickets(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            UserRoleHelper rh = new UserRoleHelper();
            var bob = db.Users.ToList();
        
            var alltickets = new List<ApplicationUser>();
            foreach(var person in bob)
            {
                if (rh.IsUserInRole(person.Id, "Submitter"))
                {
                    alltickets.Add(person);
                }
            }          

            return (alltickets);
        }

        public void TicketHistory(Ticket newTicket)
        {
            TicketHistory th = new TicketHistory();
            //This gets the current User's Id
            var user = HttpContext.Current.User.Identity.GetUserId();
            var changed = db.Tickets.AsNoTracking().Where(u => u.Id == newTicket.Id);
            var originaltkt = changed.FirstOrDefault();
            if (originaltkt.Description != newTicket.Description)
            {
                th.Property = "Description";
                th.Changed = DateTimeOffset.Now;
                var gethuman = db.Users.First(u => u.Id == newTicket.AssignToUserId);
                th.OldValue = originaltkt.Description;
                th.NewValue = newTicket.Description;
                th.TicketId = newTicket.Id;
                th.UserId = user;
                TicketNotification tn = new TicketNotification();
                tn.TicketId = newTicket.Id;
                tn.UserId = gethuman.Id;
                tn.Description = "There has been a description change made on ticket " + newTicket.Title;
                tn.Created = DateTimeOffset.Now;
                db.TicketNotifications.Add(tn);
                db.TicketHistories.Add(th);
                db.SaveChanges();
            }
            if (originaltkt.AssignToUserId != newTicket.AssignToUserId)
            {
                th.Property = "Assigned";
                var gethuman = db.Users.First(u => u.Id == newTicket.AssignToUserId);
                th.OldValue = originaltkt.AssignToUser.FirstName;
                th.NewValue = gethuman.FirstName;
                th.Changed = DateTimeOffset.Now;
                th.TicketId = newTicket.Id;
                th.UserId = user;
                TicketNotification tn = new TicketNotification();
                tn.TicketId = newTicket.Id;
                tn.UserId = gethuman.Id;
                tn.Created = DateTimeOffset.Now;
                tn.Description = "You have been assigned to ticket " + newTicket.Title;           
                db.TicketNotifications.Add(tn);
                db.TicketHistories.Add(th);
                db.SaveChanges();
            }   
            if (originaltkt.TicketStatusId != newTicket.TicketStatusId)
            {
                th.Property = "Status";
                var gethuman = db.Users.First(u => u.Id == newTicket.AssignToUserId);
                var newstatus = db.TicketStatuses.First(x => x.Id == newTicket.TicketStatusId);
                th.OldValue = originaltkt.TicketStatus.Name;
                th.NewValue = newstatus.Name;
                th.Changed = DateTimeOffset.Now;
                th.TicketId = newTicket.Id;
                th.UserId = user;
                TicketNotification tn = new TicketNotification();
                tn.TicketId = newTicket.Id;
                tn.UserId = gethuman.Id;
                tn.Created = DateTimeOffset.Now;
                tn.Description = "There has been a status change made on ticket " + newTicket.Title;
                db.TicketNotifications.Add(tn);
                db.TicketHistories.Add(th);
                db.SaveChanges();
            }
            if (originaltkt.TicketTypesId != newTicket.TicketTypesId)
            {
                th.Property = "Type";
                var gethuman = db.Users.First(u => u.Id == newTicket.AssignToUserId);
                var newtype = db.TicketTypes.First(x => x.Id == newTicket.TicketTypesId);
                th.OldValue = originaltkt.TicketTypes.Name;
                th.NewValue = newtype.Name;
                th.Changed = DateTimeOffset.Now;
                th.TicketId = newTicket.Id;
                th.UserId = user;
                TicketNotification tn = new TicketNotification();
                tn.TicketId = newTicket.Id;
                tn.UserId = gethuman.Id;
                tn.Created = DateTimeOffset.Now;
                tn.Description = "There has been a ticket type change made on ticket " + newTicket.Title;
                db.TicketNotifications.Add(tn);
                db.TicketHistories.Add(th);
                db.SaveChanges();
            }
            if (originaltkt.OwnerUserId != newTicket.OwnerUserId)
            {
                th.Property = "Owner";
                var gethuman = db.Users.First(u => u.Id == newTicket.OwnerUserId);
                th.OldValue = originaltkt.OwnerUser.FirstName;
                th.NewValue = gethuman.FirstName;
                th.Changed = DateTimeOffset.Now;
                th.TicketId = newTicket.Id;
                th.UserId = user;
                TicketNotification tn = new TicketNotification();
                tn.TicketId = newTicket.Id;
                tn.UserId = gethuman.Id;
                tn.Created = DateTimeOffset.Now;
                tn.Description = "A new owner has been assigned to ticket " + newTicket.Title;
                db.TicketNotifications.Add(tn);
                db.TicketHistories.Add(th);
                db.SaveChanges();
            }
            if (originaltkt.TicketPriorityId != newTicket.TicketPriorityId)
            {
                th.Property = "Priority";
                var gethuman = db.Users.First(u => u.Id == newTicket.AssignToUserId);
                var newtype = db.TicketPriorities.First(x => x.Id == newTicket.TicketPriorityId);
                th.OldValue = originaltkt.TicketPriority.Name;
                th.NewValue = newtype.Name;
                th.Changed = DateTimeOffset.Now;
                th.TicketId = newTicket.Id;
                th.UserId = user;
                TicketNotification tn = new TicketNotification();
                tn.TicketId = newTicket.Id;
                tn.UserId = gethuman.Id;
                tn.Created = DateTimeOffset.Now;
                tn.Description = "There has been a new priority made on ticket " + newTicket.Title;
                db.TicketNotifications.Add(tn);
                db.TicketHistories.Add(th);
                db.SaveChanges();
            }


        }

        //public ICollection<Ticket> ListTicketstoProjects(string userId, int id)
        //{
        //    ApplicationUser user = db.Users.Find(userId);
        //    Project projects = db.Projects.Find(id);
        //    //var ticket = db.Tickets.Include(u => u.AssignToUserId == user.Id).Include(u => u.ProjectId == projects.Id);
        //    var tickets = db.Tickets.Where(u => u.AssignToUserId == user.Id && u.ProjectId == projects.Id);

        //    var mytickets = tickets.ToList();
        //    return (mytickets);
        //}

    }
}