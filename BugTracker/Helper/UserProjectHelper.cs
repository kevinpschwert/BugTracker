using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helper
{
    public class UserProjectHelper
    {
        //private UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>();
        ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserOnProject(string userId, int projectId)
        {
            if (db.Projects.Find(projectId).User.Contains(db.Users.Find(userId)))
            {
                return true;
            }
            return false;           
        }

        public ICollection<Project> ListUserProjects(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);

            var projects = user.Projects.ToList();            
            return (projects);
        }

        public void AddUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var newUser = db.Users.Find(userId);

                proj.User.Add(newUser);
                db.SaveChanges();               
            }           
        }

        public void RemoveUserFromProject(string userId, int projectId)
        {
            if (IsUserOnProject(userId, projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var oldUser = db.Users.Find(userId);

                proj.User.Remove(oldUser);
                db.SaveChanges();
            }
        }

        public ICollection<ApplicationUser> UserOnProject(int projectId)
        {
            return db.Projects.Find(projectId).User;
        }

        public ICollection<ApplicationUser> UserNotOnProject(int projectId)
        {
            return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();
        }
    }
}

//public class UserP
//var project = db.Project.Find(projectId);
//var flag = project.User.Any(u => u.Id == userId);
//return (flag);