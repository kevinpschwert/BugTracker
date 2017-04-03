using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using BugTracker.Helper;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        UserProjectHelper up = new UserProjectHelper();
        UserTicketHelper ut = new UserTicketHelper();

        // GET: Projects
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Index()
        {

            TicketViewModel tvm = new TicketViewModel()
            {
                Project = db.Projects.ToList(),
                TicketNotification = ut.ListTicketNotifications(User.Identity.GetUserId()).ToList()
            };
            //var allProjects = db.Projects.ToList();
            //var myProjects = up.ListUserProjects(User.Identity.GetUserId());

            //ViewBag.AssignedToProject = new MultiSelectList(allProjects, "Id", "DisplayName");
            //ViewBag.AssignUser = new SelectList(myProjects, "Id", "DisplayName");

            return View(tvm);
        }

        public ActionResult MyProjects()
        {
            if (User.Identity.GetUserId() == null)
            {
                return RedirectToAction("LogMeIn", "Admin");
            }
            TicketViewModel TicketViewModel = new TicketViewModel()
            {
                Ticket = ut.ListAssignedTickets(User.Identity.GetUserId()).ToList(),
                TicketNotification = ut.ListTicketNotifications(User.Identity.GetUserId()).ToList(),
                Project = up.ListUserProjects(User.Identity.GetUserId()).ToList()
            };
            //var list = up.ListUserProjects(User.Identity.GetUserId());  
            return View(TicketViewModel);
        }

        [HttpPost]
        public ActionResult ToDevelopers(string id)
        {
            Ticket tickets = new Ticket();
            var bob = db.Roles.Find(id);
            tickets.AssignToUserId = bob.Id;
            db.SaveChanges();
            return View();
        }

        public ActionResult Projects()
        {
            var list = db.Projects.ToList();
            return View(list);
        }


        // GET: Projects/Details/5
        //[Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            
            var prj = up.UserOnProject(id);
            var noprj = up.UserNotOnProject(id);
            ViewBag.AssignedToProject = new MultiSelectList(prj, "Id", "DisplayName");
            ViewBag.AssignUser = new SelectList(noprj, "Id", "DisplayName");
            //ViewBag.AssignProject = new SelectList(db.Projects, "Id", "Name");

            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.Created = DateTimeOffset.Now;
                project.Updated = DateTimeOffset.Now;
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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
        
    }
}
