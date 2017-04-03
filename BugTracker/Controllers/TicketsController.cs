using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using BugTracker.Helper;
using System.Threading.Tasks;

namespace BugTracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        UserTicketHelper ut = new UserTicketHelper();
        UserRoleHelper uh = new UserRoleHelper();
        UserProjectHelper up = new UserProjectHelper();


        // GET: Tickets
        public ActionResult Index()
        {
            var tickets = new List<Ticket>();
            if(User.IsInRole("Admin"))
            {
                tickets = db.Tickets.Include(t => t.AssignToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketTypes).ToList();
            }
            //else if (User.IsInRole("ProjectManager"))
            //{
            //    var tickets = new List<Ticke>
            //}
            return View(tickets);
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            //Check the user and if they are not assigned to the ticket then redirect to action

            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return RedirectToAction("Tables", "Tickets");
            }            
            var developers = uh.UserInRole("Developer");
            var user = User.Identity.GetUserId();
            var owner = uh.ListTicketOwner(user, id);
            ViewBag.AssignToUserId = new SelectList(developers, "Id", "FirstName", ticket.AssignToUserId);
            //ViewBag.AssignToUserId = new SelectList(db.Users, "Id", "DisplayName", ticket.AssignToUserId);
            ViewBag.OwnerUserId = new SelectList(owner, "Id", "DisplayName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypesId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypesId);            
            if (User.IsInRole("Admin"))
            {
                if (id == null)
                {
                    return RedirectToAction("Tables", "Tickets");
                }
                //Ticket tickets = db.Tickets.Find(id);
                if (ticket == null)
                {
                    return RedirectToAction("Tables", "Tickets");
                }

                return View(ticket);
            }
            else if (User.IsInRole("Project Manager"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var list = ut.ListProjectTickets(User.Identity.GetUserId());
                Ticket tickets = list.FirstOrDefault(t => t.Id == id);
                if (tickets == null)
                {
                    return RedirectToAction("Tables", "Tickets");
                }
                return View(tickets);
            }
            else if (User.IsInRole("Submitter"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var list = ut.ListOwnerTickets(User.Identity.GetUserId());
                Ticket tickets = list.FirstOrDefault(t => t.Id == id);
                if (tickets == null)
                {
                    return RedirectToAction("Tables", "Tickets");
                }
                return View(tickets);
            }
            else 
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var list = ut.ListAssignedTickets(User.Identity.GetUserId());
                Ticket tickets = list.FirstOrDefault(t => t.Id == id);
                if (tickets == null)
                {
                    return RedirectToAction("Tables", "Tickets");
                }
                return View(tickets);
            }
            
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        public ActionResult Create()
        {
            //var create = up.UserOnProject("Submitter");
            ViewBag.AssignToUserId = new SelectList(db.Users, "Id", "DisplayName");
            //ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "DisplayName");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");
            ViewBag.TicketTypesId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        //[Authorize(Roles = "Submitter")]
        public ActionResult Created()
        {

            var user = User.Identity.GetUserId();
            //var create = up.UserOnProject(user);
            var bob = up.ListUserProjects(user);
            ViewBag.AssignToUserId = new SelectList(db.Users, "Id", "DisplayName");
            //ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "DisplayName");
            ViewBag.ProjectId = new SelectList(bob, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");
            ViewBag.TicketTypesId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypesId,TicketPriorityId,TicketStatusId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                //var user = db.Users.Find(User.Identity.GetUserId());
                //ticket.OwnerUserId = user.Id;
              
                ticket.Created = DateTimeOffset.Now;
                ticket.Updated = DateTimeOffset.Now;               
                var user = db.Users.Find(User.Identity.GetUserId());
                string owner = user.Id;                
                ticket.OwnerUserId = owner;
                ticket.AssignToUserId = "33ada9a3-5283-4dff-b3e5-81b6edf10d93";
                //ticket.AssignToUserId = owner;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("MyProjects", "Projects");
            }
           
            //ViewBag.AssignToUserId = new SelectList(db.Users, "Id", "DisplayName", ticket.AssignToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "DisplayName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypesId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            var developers = uh.UserInRole("Developer");
            var user = User.Identity.GetUserId();
            ViewBag.AssignToUserId = new SelectList(developers, "Id", "FirstName", ticket.AssignToUserId);
            //ViewBag.AssignToUserId = new SelectList(db.Users, "Id", "DisplayName", ticket.AssignToUserId);
            ViewBag.OwnerUserId = new SelectList(user, "Id", "DisplayName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypesId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypesId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,AssignToUserId,Description,Updated,ProjectId,TicketTypesId,TicketPriorityId,TicketStatusId,OwnerUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                if (ticket.TicketStatusId == 3)
                {
                    ticket.Created = DateTimeOffset.Now;
                }                
                ut.TicketHistory(ticket);
                ticket.Updated = DateTimeOffset.Now;
                UserTicketNotificationHelper utn = new UserTicketNotificationHelper();
                await utn.SendEmailEdit(ticket);
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = ticket.Id });
            }
            var developers = uh.UserInRole("Developer");
            ViewBag.AssignToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "DisplayName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypesId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Tables()
        {
            var table = db.Tickets.Where(u => u.TicketStatus.Id != 3);
            //var complete = db.Tickets.Where(u => u.TicketStatus.Id == 3);
            //ViewBag.Completed = new SelectList(complete, "Id", "Title", "Description", "Createed", "Updated");
            TicketViewModel TicketViewModel = new TicketViewModel()
            {
                Ticket = table.ToList(),
                TicketNotification = ut.ListTicketNotifications(User.Identity.GetUserId()).ToList()
            };
            
            return View(TicketViewModel);
        }

        //[Authorize(Roles = "Developer")]
        public ActionResult MyTables()
        {
            if (User.Identity.GetUserId() == null)
            {
                return RedirectToAction("LogMeIn", "Admin");
            }
            //var list = ut.ListUserTickets(User.Identity.GetUserId());
            if (User.IsInRole("Project Manager"))
            {
                TicketViewModel TicketViewModel = new TicketViewModel()
                {
                    Ticket = ut.ListProjectTickets(User.Identity.GetUserId()).ToList(),
                    TicketNotification = ut.ListTicketNotifications(User.Identity.GetUserId()).ToList()
                };

                return View(TicketViewModel);
                //var list = ut.ListProjectTickets(User.Identity.GetUserId());
                //return View(list);
            }
            else if(User.IsInRole("Developer"))
            {
                TicketViewModel TicketViewModel = new TicketViewModel()
                {
                    Ticket = ut.ListAssignedTickets(User.Identity.GetUserId()).ToList(),
                    TicketNotification = ut.ListTicketNotifications(User.Identity.GetUserId()).ToList()
                };

                return View(TicketViewModel);

                //var list = ut.ListAssignedTickets(User.Identity.GetUserId());
                //return View(list);
            }
            else if (User.IsInRole("Submitter"))
            {
                TicketViewModel TicketViewModel = new TicketViewModel()
                {
                    Ticket = ut.ListOwnerTickets(User.Identity.GetUserId()).ToList(),
                    TicketNotification = ut.ListTicketNotifications(User.Identity.GetUserId()).ToList()
                };

                return View(TicketViewModel);
                //var user = db.Users.Find(User.Identity.GetUserId());
                //var list = ut.ListOwnerTickets(User.Identity.GetUserId());
                //return View(list);
            }
            else
            {
                TicketViewModel TicketViewModel = new TicketViewModel()
                {
                    Ticket = db.Tickets.ToList(),
                    TicketNotification = ut.ListTicketNotifications(User.Identity.GetUserId()).ToList()
                };

                return View(TicketViewModel);                
            }            
        }

        public ActionResult TicketComments(int id)
        {
            //Ticket tickets = new Ticket();
            //tickets.Id = id;
            var comments = db.Tickets.Find(id);
            return View(comments);
        }

        public ActionResult TicketsProjects(int id)
        {
            if (User.Identity.GetUserId() == null)
            {
                return RedirectToAction("LogMeIn", "Admin");
            }
            var list = ut.ListTicketstoProjects(User.Identity.GetUserId(), id);
            return View(list);
        }

        public ActionResult TicketsOwned()
        {
            if (User.Identity.GetUserId() == null)
            {
                return RedirectToAction("LogMeIn", "Admin");
            }
            var list = ut.ListOwnerTickets(User.Identity.GetUserId());
            return View(list);
        }

        public ActionResult TicketsAssignedTo()
        {
            if (User.Identity.GetUserId() == null)
            {
                return RedirectToAction("LogMeIn", "Admin");
            }
            var list = ut.ListAssignedTickets(User.Identity.GetUserId());
            return View(list);
        }

        public ActionResult TicketsCreated()
        {
            if (User.Identity.GetUserId() == null)
            {
                return RedirectToAction("LogMeIn", "Admin");
            }
            
            var list = ut.ListCreatedTickets(User.Identity.GetUserId());
            return View(list);
        }
    }
}
