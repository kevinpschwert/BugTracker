using BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helper
{
    public class UserRoleHelper
    {
        private UserManager<ApplicationUser> userManager = new
        UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new
        ApplicationDbContext()));
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserInRole(string userId, string roleName)
        {
            return userManager.IsInRole(userId, roleName);
        }

        public ICollection<string> ListUserRoles(string userId)
        {
            return userManager.GetRoles(userId);
        }

        public bool AddUserToRole(string userId, string roleName)
        {
            var result = userManager.AddToRole(userId, roleName);
            return result.Succeeded;
        }
        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = userManager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }

        public ICollection<ApplicationUser> UserInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach (var user in List)
            {
                if (IsUserInRole(user.Id, roleName))
                    resultList.Add(user);
            }
            return resultList;
        }

        public ICollection<ApplicationUser> UsersNotInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach (var user in List)
            {
                if (!IsUserInRole(user.Id, roleName))
                    resultList.Add(user);
            }
            return resultList;
        }

        public bool TicketOwner(string userId, int? id)
        {
            ApplicationUser user = db.Users.Find(userId);
            Ticket tickets = new Ticket();
            //var tickets = db.Tickets.Where(u => u.AssignToUserId == user.Id && u.Id == id);
            if (db.Tickets.Find(id).OwnerUserId.Contains(userId))
            {
                return true;
            }             
            
            return false;

            //var owner = db.Users.Where(u => u.Id == tickets.OwnerUserId).FirstOrDefault(u => tickets.Id == id);
            
            //return (owner);

        }

        public ICollection<ApplicationUser> ListTicketOwner(string roleName, int? id)
        {
            var resultList = new List<ApplicationUser>();
            var list = userManager.Users.ToList();
            foreach (var user in list)
            {
                if(TicketOwner(user.Id,id))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }

        //public ICollection<IdentityRole> ListRoles(string userId)
        //{
        //    var list = new List<IdentityRole>();
        //    var user = db.Roles.ToList();
        //    foreach(var role in user)
        //    {
        //        if(UserInRole(userId))
        //        {
        //            list.Add(role);
        //        }
        //    }
        //    return user;
        //}
       
    }
}
