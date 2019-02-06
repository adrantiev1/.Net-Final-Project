using eBikesSystem.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using eBikesWebApp.Admin.Security.POCOs;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using eBikesData.Entities.POCOs;
using eBikesWebApp.Models;
using System.Configuration;
using eBikesWebApp.Admin.Security.DTOs;

namespace eBikesWebApp.Admin.Security
{
    [DataObject]
    public class RegistrationController
    {
        #region Business Process Operations
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<UnregisteredUser> ListAllUnregisteredUsers()
        {
            // Make an in-memory list of employees & customers who have login accounts
            var userManager = HttpContext.Current.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var registered = from user in userManager.Users
                             select new RegisteredUser()
                             {
                                 UserName = user.UserName,
                                 UserId = user.EmployeeId.ToString()
                             };

            // List eBikes Users (employees & customers)
            var controller = new eBikesUserController();
            var eBikesUsers = controller.ListUsers();

            // Determine who's not yet registered (assigning default usernames/emails)
            var unregistered = from person in eBikesUsers
                               where !registered.Any(x => x.UserId == person.Id.ToString())
                               select new UnregisteredUser()
                               {
                                   Id = person.Id,
                                   Name = person.Name,
                                   OtherName = person.OtherName,
                                   Position = person.Position,
                                   AssignedUserName =  $"{person.Name}.{person.OtherName}".Replace(" ", ""),
                                   AssignedEmail = $"{person.Name}.{person.OtherName}@eBikes.tba".Replace(" ", "")
                               };
            return unregistered.ToList();
        }

        public void RegisterUser(UnregisteredUser userInfo)
        {
            // Basic validation
            if (userInfo == null)
                throw new ArgumentNullException(nameof(userInfo), "Data for unregistered users is required");
            if (string.IsNullOrEmpty(userInfo.AssignedUserName))
                throw new ArgumentException("New users must have a username", nameof(userInfo.AssignedUserName));

            var userAccount = new ApplicationUser()
            {
                UserName = userInfo.AssignedUserName,
                Email = userInfo.AssignedEmail,
                EmployeeId = userInfo.Id
                
            };
            

            var userManager = HttpContext.Current.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var identityResult = userManager.Create(userAccount, ConfigurationManager.AppSettings["newUserPassword"]); // or randomPassword
            if (identityResult.Succeeded)
            {
                switch (userInfo.Position)
                {
                    case 3:
                        userManager.AddToRole(userAccount.Id, ConfigurationManager.AppSettings["receivingRole"]);
                        userManager.AddToRole(userAccount.Id, ConfigurationManager.AppSettings["purchasingRole"]);
                        break;
                    case 1:
                        userManager.AddToRole(userAccount.Id, ConfigurationManager.AppSettings["servicesRole"]);
                        break;
                }
            }
            else
            {
                throw new Exception($@"Security changes were not applied:<ul> 
                                       {string.Join(string.Empty,
                                                    identityResult.Errors
                                                    .Select(x => $"<li>{x}</li>"))}</ul>");
            }
        }
        #endregion
    }
}