using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.ModelBinding;

namespace BugTracker.Helper
{
    public class UserTicketNotificationHelper
    {
       
        ApplicationDbContext db = new ApplicationDbContext();
        public async Task SendEmailEdit(Ticket ticket)
        {

            IdentityMessage message = new IdentityMessage();
            var gethuman = db.Users.First(u => u.Id == ticket.AssignToUserId);
            message.Destination = gethuman.Email;
            message.Body = "There has been a modification made on Ticket " + ticket.Title;
            message.Subject = "Ticket change";
            var email = 
                new MailMessage(new MailAddress("bob@gmail.com"),
                 new MailAddress(message.Destination))
                    {
                        Subject = message.Subject,
                        Body = message.Body,
                        IsBodyHtml = true
                    };
                    var svc = new PersonalEmail();                   

            //return RedirectToAction("Sent");
            var GmailUsername = WebConfigurationManager.AppSettings["username"];
            var GmailPassword = WebConfigurationManager.AppSettings["password"];
            var host = WebConfigurationManager.AppSettings["host"];
            int port = Convert.ToInt32(WebConfigurationManager.AppSettings["port"]);
            using (var smtp = new SmtpClient()
            {
                Host = host,
                Port = port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(GmailUsername, GmailPassword)
            })

                try
            {
                await svc.SendAsync(email);
            }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
        }

        public async Task SendEmailComment(TicketComment ticketComment)
        {

            IdentityMessage message = new IdentityMessage();
            var user = HttpContext.Current.User.Identity.GetUserId();
            var ticket = db.Tickets.FirstOrDefault(x => x.Id == ticketComment.TicketId);
            var gethuman = ticket.AssignToUser.Email;
            var getTicket = db.Tickets.Where(u => u.Id == ticketComment.TicketId && u.AssignToUserId == user);
            message.Destination = gethuman;
            message.Body = "There has been a new comment made on Ticket " + ticketComment.Ticket.Title;
            message.Subject = "Comment added";
            var email =
                new MailMessage(new MailAddress("bob@gmail.com"),
                 new MailAddress(gethuman))
                {
                    Subject = message.Subject,
                    Body = message.Body,
                    IsBodyHtml = true
                };
            var svc = new PersonalEmail();

            //return RedirectToAction("Sent");
            var GmailUsername = WebConfigurationManager.AppSettings["username"];
            var GmailPassword = WebConfigurationManager.AppSettings["password"];
            var host = WebConfigurationManager.AppSettings["host"];
            int port = Convert.ToInt32(WebConfigurationManager.AppSettings["port"]);
            using (var smtp = new SmtpClient()
            {
                Host = host,
                Port = port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(GmailUsername, GmailPassword)
            })

                try
                {
                    await svc.SendAsync(email);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
        }

        public async Task SendEmailNotification(TicketAttachment ticketAttachment)
        {

            IdentityMessage message = new IdentityMessage();
            var user = HttpContext.Current.User.Identity.GetUserId();
            var ticket = db.Tickets.First(x => x.Id == ticketAttachment.Id);
            var gethuman = ticket.AssignToUser.Email;
            var getTicket = db.Tickets.Where(u => u.Id == ticketAttachment.TicketId && u.AssignToUserId == user);
            message.Destination = gethuman;
            message.Body = "There has been a new Attachment made on Ticket " + ticketAttachment.Ticket.Title;
            message.Subject = "Attachment added";
            var email =
                new MailMessage(new MailAddress("bob@gmail.com"),
                 new MailAddress(gethuman))
                {
                    Subject = message.Subject,
                    Body = message.Body,
                    IsBodyHtml = true
                };
            var svc = new PersonalEmail();

            //return RedirectToAction("Sent");
            var GmailUsername = WebConfigurationManager.AppSettings["username"];
            var GmailPassword = WebConfigurationManager.AppSettings["password"];
            var host = WebConfigurationManager.AppSettings["host"];
            int port = Convert.ToInt32(WebConfigurationManager.AppSettings["port"]);
            using (var smtp = new SmtpClient()
            {
                Host = host,
                Port = port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(GmailUsername, GmailPassword)
            })

                try
                {
                    await svc.SendAsync(email);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }
        }
    }
}