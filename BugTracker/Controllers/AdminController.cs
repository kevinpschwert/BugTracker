using BugTracker.Helper;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        UserRoleHelper uh = new UserRoleHelper();
        UserProjectHelper up = new UserProjectHelper();
        UserTicketHelper ut = new UserTicketHelper();

        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult ListRoles()
        {

            ApplicationUser bob = new ApplicationUser();
            ApplicationUser app = db.Users.Find(User.Identity.GetUserId());
            var list = app.Roles.ToList();
            var user = db.Users.ToList();
            var house = db.Users.Find(User.Identity.GetUserId()).Roles;
            //ApplicationUser user = db.Users.Find(userId);
            //var jim = db.Roles.Where(u => u.Id == bob.Id && u.Users == bob.Roles).ToList();
            var jim = uh.ListUserRoles(User.Identity.GetUserId());
            var isAdmin = uh.UserInRole("Admin");
            var isPm = uh.UserInRole("Project Manager");
            var isSub = uh.UserInRole("Submitter");
            var isDev = uh.UserInRole("Developer");
            var isNotAdmin = uh.UsersNotInRole("Admin");
            var isNotPm = uh.UsersNotInRole("Project Manager");
            var isNotSub = uh.UsersNotInRole("Submitter");
            var isNotDev = uh.UsersNotInRole("Developer");
            ViewBag.Assigned = new MultiSelectList(db.Users, "Id", "DisplayName");
            ViewBag.AddToRoles = new MultiSelectList(db.Roles, "Name", "Name", jim);
            ViewBag.IsAdmin = new MultiSelectList(isAdmin, "Id", "DisplayName");
            ViewBag.IsPm = new MultiSelectList(isPm, "Id", "DisplayName");
            ViewBag.IsSub = new MultiSelectList(isSub, "Id", "DisplayName");
            ViewBag.IsDev = new MultiSelectList(isDev, "Id", "DisplayName");
            ViewBag.IsNotAdmin = new MultiSelectList(isNotAdmin, "Id", "DisplayName");
            ViewBag.IsNotPm = new MultiSelectList(isNotPm, "Id", "DisplayName");
            ViewBag.IsNotSub = new MultiSelectList(isNotSub, "Id", "DisplayName");
            ViewBag.IsNotDev = new MultiSelectList(isNotDev, "Id", "DisplayName");
            //ViewBag.RemoveFromRoles = new SelectList(db.Roles, "Name", "Name");
            //ViewBag.AssignUser = new SelectList(db.Users, "Id", "DisplayName");
            //ViewBag.AssignProject = new SelectList(db.Projects , "Id", "Name");
            var project = db.Projects.ToList();
            return View(user);
        }

        [HttpPost]
        public ActionResult AddToRole(List<string> Assigned, List<string> AddToRoles)
        {
            foreach (var user in Assigned)
            {
                foreach (var role in AddToRoles)
                {
                    var list = uh.AddUserToRole(user, role);
                }
            }

            //foreach (var user in AssignUser)
            //{
            //    foreach (var project in AssignProject)
            //    {
            //        up.AddUserToProject(user, project);
            //    }
            //}


            return RedirectToAction("LandingPage", "Admin");
        }

        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult AddToProjects()
        {
            ViewBag.AssignUser = new SelectList(db.Users, "Id", "DisplayName");
            ViewBag.AssignProject = new SelectList(db.Projects, "Id", "Name");
            var project = db.Projects.ToList();

            return View(project);
        }

        [HttpPost]
        //public ActionResult AddToProjects(List<string> AssignUser, List<int> AssignProject)
        public ActionResult AddToProjects(List<string> AssignUser, int id)
        {
            //Project projects = new Project();
            var project = db.Projects.Where(u => u.Id == id);
            //projects.Id = id;
            foreach (var user in AssignUser)
            {
                up.AddUserToProject(user, id);
            }

            return RedirectToAction("Details", "Projects", new { id });
        }

        //public ActionResult AddToProjects(List<string> AssignUser, List<int> AssignProject)
        //{
        //    foreach (var user in AssignUser)
        //    {
        //        foreach (var project in AssignProject)
        //        {
        //            up.AddUserToProject(user, project);
        //        }
        //    }

        //    return RedirectToAction("ListProjectUsers");
        //}

        public ActionResult ListProjectUsers()
        {
            var projects = db.Projects.ToList();
            return View(projects);
        }

        public ActionResult RemoveFromProject(int? projectId)
        {
            var prj = up.UserOnProject(projectId ?? 1);
            ViewBag.AssignedToProject = new SelectList(prj, "Id", "DisplayName");

            return View();
        }

        [HttpPost]
        public ActionResult RemoveFromProject(List<string> AssignedToProject, int id)
        {

            foreach (var user in AssignedToProject)
            {
                up.RemoveUserFromProject(user, id);
            }

            return RedirectToAction("Details", "Projects", new { id });
        }

        public ActionResult RemoveFromRole()
        {
            ViewBag.Assigned = new SelectList(db.Users, "Id", "DisplayName");
            ViewBag.Roles = new SelectList(db.Roles, "Name", "Name");


            return View();
        }

        [HttpPost]
        public ActionResult RemoveFromRoleAdmin(List<string> IsAdmin)
        {
            foreach (var user in IsAdmin)
            {
                uh.RemoveUserFromRole(user, "Admin");
            }

            return RedirectToAction("ListRoles", "Admin");
        }

        [HttpPost]
        public ActionResult RemoveFromRolePM(List<string> IsPM)
        {
            foreach (var user in IsPM)
            {
                uh.RemoveUserFromRole(user, "Project Manager");
            }

            return RedirectToAction("ListRoles", "Admin");
        }

        [HttpPost]
        public ActionResult RemoveFromRoleDev(List<string> IsDev)
        {
            foreach (var user in IsDev)
            {
                uh.RemoveUserFromRole(user, "Developer");
            }

            return RedirectToAction("ListRoles", "Admin");
        }

        [HttpPost]
        public ActionResult RemoveFromRoleSub(List<string> IsSub)
        {
            foreach (var user in IsSub)
            {
                uh.RemoveUserFromRole(user, "Submitter");
            }

            return RedirectToAction("ListRoles", "Admin");
        }

        [HttpPost]
        public ActionResult AddToRoleAdmin(List<string> IsNotAdmin)
        {
            foreach (var user in IsNotAdmin)
            {
                uh.AddUserToRole(user, "Admin");
            }

            return RedirectToAction("ListRoles", "Admin");
        }

        [HttpPost]
        public ActionResult AddToRolePM(List<string> IsNotPM)
        {
            foreach (var user in IsNotPM)
            {
                uh.AddUserToRole(user, "Project Manager");
            }

            return RedirectToAction("ListRoles", "Admin");
        }

        [HttpPost]
        public ActionResult AddToRoleSub(List<string> IsNotSub)
        {
            foreach (var user in IsNotSub)
            {
                uh.AddUserToRole(user, "Submitter");
            }

            return RedirectToAction("ListRoles", "Admin");
        }

        [HttpPost]
        public ActionResult AddToRoleDev(List<string> IsNotDev)
        {
            foreach (var user in IsNotDev)
            {
                uh.AddUserToRole(user, "Developer");
            }

            return RedirectToAction("ListRoles", "Admin");
        }

        public ActionResult ListRoleUsers()
        {
            var user = db.Users.ToList();
            return View(user);
        }

        public ActionResult LogMeIn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult RegisterMe()
        {
            return View();
        }

        public ActionResult asdfs()
        {

            return View();
        }

        public ActionResult RoleChange(string id)
        {
            var user = db.Users.Find(id);
            var users = uh.UserInRole(id);
            ViewBag.User = new MultiSelectList(users, "Id", "DisplayName");
            ViewBag.Role = new MultiSelectList(db.Roles, "Id", "Name");

            var roleHelper = new UserRoleHelper();
            var userAssigned = roleHelper.ListUserRoles(id);
            ViewBag.Roles = new MultiSelectList(db.Roles, "Id", "Name", userAssigned);

            return View(user);
        }

        [HttpPost]
        public ActionResult RoleChange(string id, List<string> Roles)
        {
            var rh = new UserRoleHelper();

            foreach (var item in db.Roles)
            {
                var name = item.Name;
                rh.RemoveUserFromRole(id, name);
            }

            foreach (var item in Roles)
            {
                var list = rh.AddUserToRole(id, item);
            }

            return RedirectToAction("MyProjects", "Projects");
        }

        public ActionResult Bob()
        {
            return View();
        }

        //public ActionResult AddUser(int ProjectId)
        //{
        //    //Users not already on the project
        //    var userList = up.UsersNotOnProject(ProjectId);
        //    ViewBag.UnAssignedUsers = new MultiSelectList(userList, "Id", "FullName");
        //    return View(db.Projects.FirstOrDefault(p => p.Id == ProjectId));
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(int ProjectId, List<string> UnassignedUsers)
        {
            if (UnassignedUsers == null)
            {
                return RedirectToAction("Index", "Projects");
            }
            foreach (var userId in UnassignedUsers)
            {
                up.AddUserToProject(userId, ProjectId);
            }
            return RedirectToAction("Index", "Projects");
        }

        public ActionResult RemoveUser(int ProjectId)
        {
            //Users currently on the project
            var userList = up.UserOnProject(ProjectId);
            ViewBag.AssignedUsers = new MultiSelectList(userList, "Id", "FullName");
            return View(db.Projects.FirstOrDefault(p => p.Id == ProjectId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveUser(int ProjectId, List<string> AssignedUsers)
        {
            if (AssignedUsers == null)
            {
                return RedirectToAction("Index", "Projects");
            }
            foreach (var userId in AssignedUsers)
            {
                up.RemoveUserFromProject(userId, ProjectId);
            }
            return RedirectToAction("Index", "Projects");
        }

        public ActionResult LandingPage()
        {
            if (User.IsInRole("Admin"))
            {
                var user = User.Identity.GetUserId();
                TicketViewModel tvm = new TicketViewModel()
                {
                    User = db.Users.ToList(),
                    Ticket = db.Tickets.ToList(),
                    Project = db.Projects.ToList(),
                    TicketNotification = db.TicketNotifications.ToList(),
                    TicketHistory = db.TicketHistories.ToList(),
                    Chat = db.Chats.ToList()
                    //Chat = db.Chats.Where(u => u.AssignToUserId == user).ToList()
                };
                ViewBag.ChatData = new SelectList(db.Users, "Id", "DisplayName");
                ViewBag.Recent = db.Tickets.Where(t => t.TicketStatusId == 3);
                return View(tvm);
            }
            else if (User.IsInRole("Project Manager"))
            {
                var user = User.Identity.GetUserId();
                TicketViewModel tvm = new TicketViewModel()
                {
                    User = db.Users.ToList(),
                    Ticket = db.Tickets.ToList(),
                    Project = db.Projects.ToList(),
                    TicketNotification = ut.ListTicketNotifications(User.Identity.GetUserId()).ToList(),
                    TicketHistory = db.TicketHistories.ToList(),
                    Chat = db.Chats.Where(u => u.AssignToUserId == user).ToList()
                };
                ViewBag.ChatData = new SelectList(db.Users, "Id", "DisplayName");
                ViewBag.Recent = db.Tickets.Where(t => t.TicketStatusId == 3);
                return View(tvm);
            }
            else
            {
                var user = User.Identity.GetUserId();
                TicketViewModel tvm = new TicketViewModel()
                {
                    User = db.Users.ToList(),
                    Ticket = ut.ListAssignedTickets(User.Identity.GetUserId()).ToList(),
                    Project = up.ListUserProjects(User.Identity.GetUserId()).ToList(),
                    TicketNotification = ut.ListTicketNotifications(User.Identity.GetUserId()).ToList(),
                    TicketHistory = db.TicketHistories.ToList(),
                    Chat = db.Chats.Where(u => u.AssignToUserId == user).ToList()
                   
                };
                ViewBag.ChatData = new SelectList(db.Users, "Id", "DisplayName");
                return View(tvm);
            }
        }

        [HttpPost]
        public ActionResult LandingPage([Bind(Include = "Body,OwnerUserId")] Chat chat, string ChatData)
        {
            chat.Created = DateTimeOffset.Now;
            var user = User.Identity.GetUserId();
            chat.OwnerUserId = user;
            chat.AssignToUserId = ChatData;
            db.Chats.Add(chat);
            db.SaveChanges();
            if (User.IsInRole("Admin"))
            {

                TicketViewModel tvm = new TicketViewModel()
                {
                    User = db.Users.ToList(),
                    Ticket = db.Tickets.ToList(),
                    Project = db.Projects.ToList(),
                    TicketNotification = db.TicketNotifications.ToList(),
                    TicketHistory = db.TicketHistories.ToList(),
                    Chat = db.Chats.ToList()
                    //Chat = db.Chats.Where(u => u.AssignToUserId == user).ToList()
                };
                ViewBag.ChatData = new SelectList(db.Users, "Id", "DisplayName");
                ViewBag.Recent = db.Tickets.Where(t => t.TicketStatusId == 3);
                return View(tvm);
            }
            else if (User.IsInRole("Project Manager"))
            {

                TicketViewModel tvm = new TicketViewModel()
                {
                    User = db.Users.ToList(),
                    Ticket = db.Tickets.ToList(),
                    Project = db.Projects.ToList(),
                    TicketNotification = ut.ListTicketNotifications(User.Identity.GetUserId()).ToList(),
                    TicketHistory = db.TicketHistories.ToList(),
                    Chat = db.Chats.Where(u => u.AssignToUserId == user).ToList()
                };
                ViewBag.ChatData = new SelectList(db.Users, "Id", "DisplayName");
                ViewBag.Recent = db.Tickets.Where(t => t.TicketStatusId == 3);
                return View(tvm);
            }
            else
            {

                TicketViewModel tvm = new TicketViewModel()
                {
                    User = db.Users.ToList(),
                    Ticket = ut.ListAssignedTickets(User.Identity.GetUserId()).ToList(),
                    Project = up.ListUserProjects(User.Identity.GetUserId()).ToList(),
                    TicketNotification = ut.ListTicketNotifications(User.Identity.GetUserId()).ToList(),
                    TicketHistory = db.TicketHistories.ToList(),
                    Chat = db.Chats.Where(u => u.AssignToUserId == user).ToList()
                };
                ViewBag.ChatData = new SelectList(db.Users, "Id", "DisplayName");
                return View(tvm);
            }         
        }
    }
}

